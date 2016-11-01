#include "WWWConnection.h"

// WARNING: this MUST be c decl (NSString ctor will be called after +load, so we cant really change its value)

// If you need to communicate with HTTPS server with self signed certificate you might consider UnityWWWConnectionSelfSignedCertDelegate
// Though use it on your own risk. Blindly accepting self signed certificate is prone to MITM attack

//const char* WWWDelegateClassName		= "UnityWWWConnectionSelfSignedCertDelegate";
const char* WWWDelegateClassName		= "UnityWWWConnectionDelegate";
const char* WWWRequestProviderClassName = "UnityWWWRequestDefaultProvider";
static NSOperationQueue *webOperationQueue;
static NSOperationQueue *fileOperationQueue;

@interface UnityWWWConnectionDelegate()
@property (readwrite, nonatomic) void*                         udata;
@property (readwrite, retain, nonatomic) NSURL*                url;
@property (readwrite, retain, nonatomic) NSString*             user;
@property (readwrite, retain, nonatomic) NSString*             password;
@property (readwrite, retain, nonatomic) NSMutableURLRequest*  request;
@property (readwrite, retain, nonatomic) NSURLConnection*      connection;
@property (strong, nonatomic)            NSCondition*          condition;
@property (nonatomic)					 BOOL				   manuallyHandleRedirect;
@end


@implementation UnityWWWConnectionDelegate
{
	// link to unity WWW implementation
	void*				_udata;
	
	// connection parameters
	NSMutableURLRequest* _request;
	// connection that we manage
	NSURLConnection*	_connection;
	
	// NSURLConnection do not quite handle user:pass@host urls
	// so we need to extract user/pass ourselves
	NSURL*				_url;
	NSString*			_user;
	NSString*			_password;
	
	// response
	NSString*			_responseHeader;
	NSInteger			_status;
	size_t				_estimatedLength;
	size_t              _dataRecievd;
	int					_retryCount;
}

@synthesize url			= _url;
@synthesize user		= _user;
@synthesize password	= _password;
@synthesize request		= _request;
@synthesize connection	= _connection;

@synthesize udata		= _udata;
@synthesize shouldAbort;

- (NSURL*)extractUserPassFromUrl:(NSURL*)url
{
	self.user		= url.user;
	self.password	= url.password;
	
	// strip user/pass from url
	NSString* newUrl = [NSString stringWithFormat:@"%@://%@%s%s%@%s%s",
		url.scheme, url.host,
		url.port ? ":" : "", url.port ? [[url.port stringValue] UTF8String] : "",
		url.path,
		url.fragment ? "#" : "", url.fragment ? [url.fragment UTF8String] : ""
		];
	return [NSURL URLWithString:newUrl];
}

- (id)initWithURL:(NSURL*)url udata:(void*)udata;
{
	self->_retryCount = 0;
	if((self = [super init]))
	{
		self.url	= url.user != nil ? [self extractUserPassFromUrl:url] : url;
		self.udata	= udata;
		
		if([url.scheme caseInsensitiveCompare:@"http"] == NSOrderedSame)
			NSLog(@"You are using download over http. Currently unity adds NSAllowsArbitraryLoads to Info.plist to simplify transition, but it will be removed soon. Please consider updating to https.");
	}
	
	return self;
}

+ (id)newDelegateWithURL:(NSURL*)url udata:(void*)udata
{
	Class target = NSClassFromString([NSString stringWithUTF8String:WWWDelegateClassName]);
	NSAssert([target isSubclassOfClass:[UnityWWWConnectionDelegate class]], @"You MUST subclass UnityWWWConnectionDelegate");
	
	return [[target alloc] initWithURL:url udata:udata];
}

+ (id)newDelegateWithCStringURL:(const char*)url udata:(void*)udata
{
	return [UnityWWWConnectionDelegate newDelegateWithURL:[NSURL URLWithString:[NSString stringWithUTF8String: url]] udata:udata];
}

+ (NSMutableURLRequest*)newRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers
{
	Class target = NSClassFromString([NSString stringWithUTF8String:WWWRequestProviderClassName]);
	NSAssert([target conformsToProtocol:@protocol(UnityWWWRequestProvider)], @"You MUST implement UnityWWWRequestProvider protocol");
	
	return [target allocRequestForHTTPMethod:method url:url headers:headers];
}

- (void)cleanup
{
	[_connection cancel];
	self.condition = nil;
	_connection = nil;
	_request = nil;
}

- (NSURLRequest *)connection:(NSURLConnection *)connection
					willSendRequest:(NSURLRequest *)request
					 redirectResponse:(NSURLResponse *)response;
{
	if (response && self.manuallyHandleRedirect)
	{
		// notify TransportiPhone of the redirect and signal to process the next response.
		if([response isKindOfClass:[NSHTTPURLResponse class]])
		{
			NSHTTPURLResponse *httpresponse = (NSHTTPURLResponse*)response;
			NSMutableDictionary *headers = [httpresponse.allHeaderFields mutableCopy];
			// grab the correct URL from the request that would have
			// automatically been called through NSURLConnection.
			// The reason we do this is that WebRequestProto's state needs to
			// get updated internally, so we intercept redirects, cancel the current
			// NSURLConnection, notify WebRequestProto and let it construct a new
			// request from the updated URL
			[headers setObject:[request.URL absoluteString] forKey:@"Location"];
			httpresponse = [[NSHTTPURLResponse alloc] initWithURL:response.URL statusCode:httpresponse.statusCode HTTPVersion:nil headerFields:headers];
			[self handleResponse:httpresponse];
		}
		else
		{
			[self handleResponse:response];
		}
		SignalConnection(self);
		[connection cancel];
		return nil;
	}
	return request;
}

- (void)connection:(NSURLConnection*)connection didReceiveResponse:(NSURLResponse*)response
{
	[self handleResponse:response];
}

-(void)handleResponse:(NSURLResponse*)response
{
	if (self.shouldAbort)
	{
		SignalConnection(self);
		[self.connection cancel];
		return;
	}
	// on ios pre-5.0 NSHTTPURLResponse was not created for "file://"" connections, so play safe here
	// TODO: remove that once we have 5.0 as requirement
	self->_status = 200;
	if([response isMemberOfClass:[NSHTTPURLResponse class]])
	{
		NSHTTPURLResponse* httpResponse = (NSHTTPURLResponse*)response;
		NSDictionary* respHeader = [httpResponse allHeaderFields];
		NSEnumerator* headerEnum = [respHeader keyEnumerator];
		
		self->_status = [httpResponse statusCode];
		
		NSMutableString* headerString = [NSMutableString stringWithCapacity:1024];
		[headerString appendFormat:@"Status: HTTP/1.1 %d %@\n", (int)self->_status,
			[NSHTTPURLResponse localizedStringForStatusCode:self->_status]];
		
		for(id headerKey = [headerEnum nextObject] ; headerKey ; headerKey = [headerEnum nextObject])
			[headerString appendFormat:@"%@: %@\n", (NSString*)headerKey, (NSString*)[respHeader objectForKey:headerKey]];
		
		self->_responseHeader	= headerString;
		
		long long contentLength = [response expectedContentLength];
		
		// ignore any data that we might have recieved during a redirect
		self->_estimatedLength	=  contentLength > 0 && (self->_status / 100 != 3) ? contentLength : 0;
		self->_dataRecievd = 0;
		
		// status 2xx are all success
		// in case of error status we do not cancel right away to actually get server response:
		//   sometimes it might contain info useful for developers (custom webapp)
		// instead we just keep on getting data while it is here and simply remember that there was error
		// status 3xx are all redirects, we should not report them as errors as well
		if(self->_status / 100 > 3)
		{
			UnityReportWWWStatusError(self.udata, (int)self->_status, [[NSHTTPURLResponse localizedStringForStatusCode: self->_status] UTF8String]);
		}
	}
	UnityReportWWWReceivedResponse(self.udata, (int)self->_status, (unsigned int)(unsigned long)self->_estimatedLength, [self->_responseHeader UTF8String]);
}

- (void)connection:(NSURLConnection*)connection didReceiveData:(NSData*)data
{
	if(self.shouldAbort)
	{
		SignalConnection(self);
		[connection cancel];
		return;
	}
	
	UnityReportWWWReceivedData(self.udata, data.bytes, (unsigned int)[data length], (unsigned int)self->_estimatedLength);
}

static void SignalConnection(UnityWWWConnectionDelegate* delegate)
{
	// Signal the condition variable in case it is waiting
	NSCondition *condition = delegate.condition;
	
	delegate.request = nil;
	delegate.condition = nil;

	[condition lock];
	[condition signal];
	[condition unlock];
}

static void WaitOnCondition(UnityWWWConnectionDelegate* delegate)
{
	NSCondition *condition = delegate.condition;
	[condition lock];
	[condition wait];
	[condition unlock];
}

- (void)connection:(NSURLConnection*)connection didFailWithError:(NSError*)error
{
	UnityReportWWWStatusError(self.udata, (int)[error code], [[error localizedDescription] UTF8String]);
	UnityReportWWWFinishedLoadingData(self.udata);
	SignalConnection(self);
}

- (void)connectionDidFinishLoading:(NSURLConnection*)connection
{
	self.connection = nil;
	UnityReportWWWFinishedLoadingData(self.udata);
	SignalConnection(self);
}

- (void)connection:(NSURLConnection*)connection didSendBodyData:(NSInteger)bytesWritten totalBytesWritten:(NSInteger)totalBytesWritten totalBytesExpectedToWrite:(NSInteger)totalBytesExpectedToWrite
{
	UnityReportWWWSentData(self.udata, (unsigned int)totalBytesWritten, (unsigned int)totalBytesExpectedToWrite);
}

- (BOOL)connection:(NSURLConnection*)connection handleAuthenticationChallenge:(NSURLAuthenticationChallenge*)challenge
{
	return NO;
}
- (void)connection:(NSURLConnection*)connection willSendRequestForAuthenticationChallenge:(NSURLAuthenticationChallenge*)challenge
{
	
	if ([[challenge protectionSpace] authenticationMethod] == NSURLAuthenticationMethodServerTrust) {
		[challenge.sender performDefaultHandlingForAuthenticationChallenge:challenge];
	}
	else
	{
		
		BOOL authHandled = [self connection:connection handleAuthenticationChallenge:challenge];
		
		if(authHandled == NO)
		{
			self->_retryCount++;
			
			// Empty user or password
			if(self->_retryCount > 1 || self.user == nil || [self.user length] == 0 || self.password == nil || [self.password length]  == 0)
			{
				[[challenge sender] cancelAuthenticationChallenge:challenge];
				return;
			}
			
			NSURLCredential* newCredential =
			[NSURLCredential credentialWithUser:self.user password:self.password persistence:NSURLCredentialPersistenceNone];
			
			[challenge.sender useCredential:newCredential forAuthenticationChallenge:challenge];
		}
	}
}

@end


@implementation UnityWWWConnectionSelfSignedCertDelegate

- (BOOL)connection:(NSURLConnection*)connection handleAuthenticationChallenge:(NSURLAuthenticationChallenge*)challenge
{
	if([[challenge.protectionSpace authenticationMethod] isEqualToString:@"NSURLAuthenticationMethodServerTrust"])
	{
		[challenge.sender useCredential:[NSURLCredential credentialForTrust:challenge.protectionSpace.serverTrust]
			forAuthenticationChallenge:challenge];
		
		return YES;
	}
	
	return [super connection:connection handleAuthenticationChallenge:challenge];
}

@end


@implementation UnityWWWRequestDefaultProvider
+ (NSMutableURLRequest*)allocRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers
{
	NSMutableURLRequest* request = [[NSMutableURLRequest alloc] init];
	[request setURL:url];
	[request setHTTPMethod:method];
	[request setAllHTTPHeaderFields:headers];
	
	return request;
}
@end

//
// unity interface
//

extern "C" void UnitySendWWWConnection(void* connection, const void* data, unsigned length, bool blockImmediately)
{
	UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;

	NSMutableURLRequest* request = delegate.request;

	if (data != nil && length > 0)
	{
		[request setHTTPBody:[NSData dataWithBytes:data length:length]];
		[request setValue:[NSString stringWithFormat:@"%d", length] forHTTPHeaderField:@"Content-Length"];
	}

  static dispatch_once_t onceToken;
  dispatch_once(&onceToken, ^{
    webOperationQueue = [[NSOperationQueue alloc] init];
    webOperationQueue.maxConcurrentOperationCount = [NSProcessInfo processInfo].activeProcessorCount * 5;
	webOperationQueue.name = @"com.unity3d.WebOperationQueue";

	fileOperationQueue = [[NSOperationQueue alloc] init];
	fileOperationQueue.maxConcurrentOperationCount = [NSProcessInfo processInfo].activeProcessorCount;
	fileOperationQueue.name = @"com.unity3d.FileOperationQueue";
  });

	NSOperationQueue *queueToUse = webOperationQueue;
	if ([request.URL isFileURL])
	{
		queueToUse = fileOperationQueue;
	}
	// Start connection on non-main thread
	// This is important because didReceiveData handler may stall in case a target buffer is not ready.
	// We have limited accumulator buffer pretty much for everything (assetbundles, downloadhandler) and stall until the data is consumed.
  [queueToUse addOperation:[NSBlockOperation blockOperationWithBlock:^{
    NSRunLoop *loop = [NSRunLoop currentRunLoop];
    delegate.connection = [[NSURLConnection alloc] initWithRequest:request delegate:delegate startImmediately:NO];
    [delegate.connection scheduleInRunLoop:loop forMode:NSRunLoopCommonModes];
    [delegate.connection start];
    [loop run];
  }]];

	if (blockImmediately)
	{
		delegate.manuallyHandleRedirect = YES;
		WaitOnCondition(delegate);
	}
}

extern "C" void* UnityStartWWWConnectionCustom(void* udata, const char* methodString, const void* headerDict, const char* url)
{
	UnityWWWConnectionDelegate*	delegate = [UnityWWWConnectionDelegate newDelegateWithCStringURL:url udata:udata];
	
	delegate.request = [UnityWWWConnectionDelegate newRequestForHTTPMethod:[NSString stringWithUTF8String:methodString] url:delegate.url headers:(__bridge NSDictionary*)headerDict];
	
	// Initialize the condition variable
	delegate.condition = [[NSCondition alloc] init];
	
	return (__bridge_retained void*)delegate;
}

extern "C" bool UnityBlockWWWConnectionIsDone(void* connection)
{
	UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;
	return (delegate.request == nil);
}

extern "C" void UnityBlockWWWConnectionUntilDone(void* connection)
{
	UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;
	WaitOnCondition(delegate);
}

extern "C" void UnityDestroyWWWConnection(void* connection)
{
	UnityWWWConnectionDelegate* delegate = (__bridge_transfer UnityWWWConnectionDelegate*)connection;
	
	[delegate cleanup];
	delegate = nil;
}

extern "C" void UnityShouldCancelWWW(const void* connection)
{
	UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;
	[delegate.connection cancel];
	
	delegate.shouldAbort = YES;
	SignalConnection(delegate);
}
