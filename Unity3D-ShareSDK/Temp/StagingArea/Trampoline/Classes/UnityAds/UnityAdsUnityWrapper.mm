//
//  UnityAdsUnityWrapper.m
//  Copyright(c) 2015 Unity Technologies. All rights reserved.
//

#import "UnityAdsUnityWrapper.h"

#if USE_UNITYADS

static UnityAdsUnityWrapper *unityAds = NULL;

void UnityPause(int pause);

extern "C"
{
	NSString* UnityAdsCreateNSString(const char* string)
	{
		return string ? [NSString stringWithUTF8String: string] : [NSString stringWithUTF8String: ""];
	}

	char* UnityAdsMakeStringCopy(const char* string)
	{
		if (string == NULL)
		  return NULL;
		char* res =(char*)malloc(strlen(string) + 1);
		strcpy(res, string);
		return res;
	}
}

@interface UnityAdsUnityWrapper() <UnityAdsDelegate>
@property(nonatomic, strong) NSString* gameId;
@end

@implementation UnityAdsUnityWrapper

-(id)initWithGameId:(NSString*)gameId testModeOn:(bool)testMode debugModeOn:(bool)debugMode
{
	self = [super init];

	if (self != nil)
	{
		self.gameId = gameId;

		[[UnityAds sharedInstance] setDelegate:self];
		[[UnityAds sharedInstance] setDebugMode:debugMode];
		[[UnityAds sharedInstance] setTestMode:testMode];
		[[UnityAds sharedInstance] startWithGameId:gameId andViewController:UnityGetGLViewController()];
	}

	return self;
}

-(void)unityAdsVideoCompleted:(NSString *)rewardItemKey skipped:(BOOL)skipped
{
	UnityAdsOnVideoCompleted(UnityAdsMakeStringCopy(rewardItemKey != nil ? [rewardItemKey UTF8String] : [@"" UTF8String]), skipped);
}

-(void)unityAdsWillShow
{
}

-(void)unityAdsDidShow
{
  UnityAdsOnShow();
  UnityPause(1);
}

-(void)unityAdsWillHide
{
}

-(void)unityAdsDidHide
{
	UnityPause(0);
	UnityAdsOnHide();
}

-(void)unityAdsWillLeaveApplication
{
}

-(void)unityAdsVideoStarted
{
	UnityAdsOnVideoStarted();
}

-(void)unityAdsFetchCompleted
{
	UnityAdsOnCampaignsAvailable();
}

-(void)unityAdsFetchFailed
{
	UnityAdsOnCampaignsFetchFailed();
}


extern "C"
{
	void UnityAdsEngineInit(const char* gameId, bool testMode, bool debugMode, const char* unityVersion)
	{
		if (unityAds == NULL)
		{
			[[UnityAds sharedInstance] setUnityVersion: UnityAdsCreateNSString(unityVersion)];
			unityAds = [[UnityAdsUnityWrapper alloc] initWithGameId:UnityAdsCreateNSString(gameId) testModeOn:testMode debugModeOn:debugMode];
		}
	}

	bool UnityAdsEngineShow(const char* rawZoneId, const char* rawRewardItemKey, const char* rawOptionsString)
	{
		NSString* zoneId = UnityAdsCreateNSString(rawZoneId);
		NSString* rewardItemKey = UnityAdsCreateNSString(rawRewardItemKey);
		NSString* optionsString = UnityAdsCreateNSString(rawOptionsString);

		NSMutableDictionary* optionsDictionary = nil;
		if ([optionsString length] > 0)
		{
			optionsDictionary = [[NSMutableDictionary alloc] init];
			[[optionsString componentsSeparatedByString:@","] enumerateObjectsUsingBlock:^(id rawOptionPair, NSUInteger idx, BOOL *stop)
			{
				NSArray *optionPair = [rawOptionPair componentsSeparatedByString:@":"];
				[optionsDictionary setValue:optionPair[1] forKey:optionPair[0]];
			}];
		}
		if ([[UnityAds sharedInstance] canShowZone:zoneId])
		{
			if ([rewardItemKey length] > 0)
			{
				[[UnityAds sharedInstance] setZone:zoneId withRewardItem:rewardItemKey];
			}
			else
			{
			   if ([zoneId length] > 0)
			   {
			       [[UnityAds sharedInstance] setZone:zoneId];
			   }
			}
			return [[UnityAds sharedInstance] show:optionsDictionary];
		}
		return false;
	}

	bool UnityAdsEngineCanShowAds(const char * rawZoneId)
	{
		return [[UnityAds sharedInstance] canShowZone:UnityAdsCreateNSString(rawZoneId)];
	}

	void UnityAdsEngineSetDebugMode(bool debugMode)
	{
		[[UnityAds sharedInstance] setDebugMode:debugMode];
	}

	void UnityAdsEngineSetCampaignDataURL(const char* url)
	{
		[[UnityAds sharedInstance] setCampaignDataURL:UnityAdsCreateNSString(url)];
	}

}

@end
#else
extern "C"
{
	void UnityAdsEngineInit(const char *gameId, bool testMode, bool debugMode, const char* unityVersion) { }

	bool UnityAdsEngineShow(const char * rawZoneId, const char * rawRewardItemKey, const char * rawOptionsString) { return false; }

	bool UnityAdsEngineCanShowAds(const char * rawZoneId) { return false; }

	void UnityAdsEngineSetDebugMode(bool debugMode) { }

	void UnityAdsEngineSetCampaignDataURL(const char* url) { }
}
#endif
