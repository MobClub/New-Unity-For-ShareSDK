
#include "SplashScreen.h"
#include "UnityViewControllerBase.h"
#include "OrientationSupport.h"
#include "Unity/ObjCRuntime.h"
#include <cstring>

extern "C" const char* UnityGetLaunchScreenXib();

#include <utility>

static SplashScreen*			_splash      = nil;
static SplashScreenController*	_controller  = nil;
static bool						_isOrientable = false; // true for iPads and iPhone 6+
static bool						_usesLaunchscreen = false;
static ScreenOrientation		_nonOrientableDefaultOrientation = portrait;

// we will create and show splash before unity is inited, so we can use only plist settings
static bool	_canRotateToPortrait			= false;
static bool	_canRotateToPortraitUpsideDown	= false;
static bool	_canRotateToLandscapeLeft		= false;
static bool	_canRotateToLandscapeRight		= false;

#if !UNITY_TVOS
typedef id (*WillRotateToInterfaceOrientationSendFunc)(struct objc_super*, SEL, UIInterfaceOrientation, NSTimeInterval);
typedef id (*DidRotateFromInterfaceOrientationSendFunc)(struct objc_super*, SEL, UIInterfaceOrientation);
#endif
typedef id (*ViewWillTransitionToSizeSendFunc)(struct objc_super*, SEL, CGSize, id<UIViewControllerTransitionCoordinator>);

static const char* GetScaleSuffix(float scale, float maxScale)
{
	if (scale > maxScale)
		scale = maxScale;
	if (scale <= 1.0)
		return "";
	if (scale <= 2.0)
		return "@2x";
	return "@3x";
}

@implementation SplashScreen
{
	UIImageView* m_ImageView;
	UIView* m_XibView;
}

- (id)initWithFrame:(CGRect)frame
{
	self = [super initWithFrame:frame];
	return self;
}

/* The following launch images are produced by Xcode6:

	LaunchImage.png
	LaunchImage@2x.png
	LaunchImage-568h@2x.png
	LaunchImage-700@2x.png
	LaunchImage-700-568h@2x.png
	LaunchImage-700-Landscape@2x~ipad.png
	LaunchImage-700-Landscape~ipad.png
	LaunchImage-700-Portrait@2x~ipad.png
	LaunchImage-700-Portrait~ipad.png
	LaunchImage-800-667h@2x.png
	LaunchImage-800-Landscape-736h@3x.png
	LaunchImage-800-Portrait-736h@3x.png
	LaunchImage-Landscape@2x~ipad.png
	LaunchImage-Landscape~ipad.png
	LaunchImage-Portrait@2x~ipad.png
	LaunchImage-Portrait~ipad.png
*/
- (void)updateOrientation:(ScreenOrientation)orient
{
	UnityReportResizeView(self.bounds.size.width * self.contentScaleFactor, self.bounds.size.height * self.contentScaleFactor, orient);


	bool orientPortrait  = (orient == portrait || orient == portraitUpsideDown);
	bool orientLandscape = (orient == landscapeLeft || orient == landscapeRight);

	bool rotateToPortrait  = _canRotateToPortrait || _canRotateToPortraitUpsideDown;
	bool rotateToLandscape = _canRotateToLandscapeLeft || _canRotateToLandscapeRight;

	const char* orientSuffix = "";
	if (_isOrientable)
	{
		if (orientPortrait && rotateToPortrait)
			orientSuffix = "-Portrait";
		else if (orientLandscape && rotateToLandscape)
			orientSuffix = "-Landscape";
		else if (rotateToPortrait)
			orientSuffix = "-Portrait";
		else
			orientSuffix = "-Landscape";
	}

	bool isIphone = UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone;
	NSString* xibName = isIphone ? @"LaunchScreen-iPhone" : @"LaunchScreen-iPad";
	bool hasLaunchScreen = [[NSBundle mainBundle] pathForResource:xibName ofType:@"nib"] != nullptr;

	if (_usesLaunchscreen && hasLaunchScreen)
	{
		// Launch screen uses the same aspect-filled image for all iPhone and/or
		// all iPads, as configured in Unity. We need a special case if there's
		// a launch screen and iOS is configured to use it.
		if (self->m_XibView == nil)
		{
			self->m_XibView = [[[NSBundle mainBundle] loadNibNamed:xibName owner:nil options:nil] objectAtIndex:0];
			[self addSubview:self->m_XibView];
		}
		return;
	}

	UIImage* image = nil;
	CGSize size = [[UIScreen mainScreen] bounds].size;

	// Try asset catalog on iOS 7.0+. Note, that we can't be sure that asset
	// catalog is used, because the deployment target might have been lower and
	// thus old launch images are used.
	if (_ios70orNewer)
	{
		NSString* name = @"LaunchImage";

		// Here we work around an iOS bug that results in incorrect images
		// being fetched from launch image asset catalogs.
		if (!isIphone)									  // any iPad
			name = @"LaunchImage~iPad";
		else if (size.height == 568 || size.width == 568) // iPhone 5
			name = @"LaunchImage~568h";
		else if (size.height == 667 || size.width == 667) // iPhone 6
			name = @"LaunchImage~667h";
		else if (size.height == 736 || size.width == 736) // iPhone 6+
			name = @"LaunchImage~736h";
		image = [UIImage imageNamed:name];
	}

	if (image == nil)
	{
		// Old launch image from file
		NSString* imageName;
		if (!isIphone)
		{
			// iPads
			const char* iOSSuffix = _ios70orNewer ? "-700" : "";
			const char* scaleSuffix = GetScaleSuffix([UIScreen mainScreen].scale, 2.0);
			imageName = [NSString stringWithFormat:@"LaunchImage%s%s%s~ipad",
							      iOSSuffix, orientSuffix, scaleSuffix];
		}
		else
		{
			// iPhones
			float scale = [UIScreen mainScreen].scale;
			const char* scaleSuffix = GetScaleSuffix(scale, 3.0);
			const char* iOSSuffix = _ios70orNewer ? "-700" : "";
			const char* rezolutionSuffix = "";

			if (size.height == 568 || size.width == 568) // iPhone5
				rezolutionSuffix = "-568h";
			else if (size.height == 667 || size.width == 667) // iPhone6
			{
				rezolutionSuffix = "-667h";
				iOSSuffix = "-800";

				if (scale > 2.0) // iPhone6+ in display zoom mode
					scaleSuffix = "@2x";
			}
			else if (size.height == 736 || size.width == 736) // iPhone6+
			{
				rezolutionSuffix = "-736h";
				iOSSuffix = "-800";
			}
			imageName = [NSString stringWithFormat:@"LaunchImage%s%s%s%s",
								  iOSSuffix, orientSuffix, rezolutionSuffix, scaleSuffix];
		}

		NSString* imagePath = [[NSBundle mainBundle] pathForResource:imageName ofType:@"png"];

		image = [UIImage imageWithContentsOfFile:imagePath];
	}

	if (self->m_ImageView == nil)
	{
		self->m_ImageView = [[UIImageView alloc] initWithImage:image];
		[self addSubview:self->m_ImageView];
	}
	else
	{
		self->m_ImageView.image = image;
	}
}

- (void)layoutSubviews
{
	if (self->m_XibView)
		self->m_XibView.frame = self.bounds;
	else if (self->m_ImageView)
		self->m_ImageView.frame = self.bounds;
}

+ (SplashScreen*)Instance
{
	return _splash;
}

- (void) FreeSubviews
{
	m_ImageView = nil;
	m_XibView = nil;
}

@end

@implementation SplashScreenController

#if !UNITY_TVOS
static void WillRotateToInterfaceOrientation_DefaultImpl(id self_, SEL _cmd, UIInterfaceOrientation toInterfaceOrientation, NSTimeInterval duration)
{
	if(_isOrientable)
		[_splash updateOrientation:ConvertToUnityScreenOrientation(toInterfaceOrientation)];

	UNITY_OBJC_FORWARD_TO_SUPER(self_, [UIViewController class], @selector(willRotateToInterfaceOrientation:duration:), WillRotateToInterfaceOrientationSendFunc, toInterfaceOrientation, duration);
}
static void DidRotateFromInterfaceOrientation_DefaultImpl(id self_, SEL _cmd, UIInterfaceOrientation fromInterfaceOrientation)
{
	if(!_isOrientable)
		OrientView((SplashScreenController*)self_, _splash, _nonOrientableDefaultOrientation);

	UNITY_OBJC_FORWARD_TO_SUPER(self_, [UIViewController class], @selector(didRotateFromInterfaceOrientation:), DidRotateFromInterfaceOrientationSendFunc, fromInterfaceOrientation);
}
#endif

static void ViewWillTransitionToSize_DefaultImpl(id self_, SEL _cmd, CGSize size, id<UIViewControllerTransitionCoordinator> coordinator)
{
	UnityViewControllerBase* self = (UnityViewControllerBase*)self_;

	ScreenOrientation curOrient = UIViewControllerOrientation(self);
	ScreenOrientation newOrient = OrientationAfterTransform(curOrient, [coordinator targetTransform]);

	if(_isOrientable)
		[_splash updateOrientation:newOrient];

	[coordinator animateAlongsideTransition:nil completion:^(id<UIViewControllerTransitionCoordinatorContext> context){
		if(!_isOrientable)
			OrientView(self, _splash, _nonOrientableDefaultOrientation);
	}];
	UNITY_OBJC_FORWARD_TO_SUPER(self_, [UIViewController class], @selector(viewWillTransitionToSize:withTransitionCoordinator:), ViewWillTransitionToSizeSendFunc, size, coordinator);
}


- (id)init
{
	if( (self = [super init]) )
	{
#if !UNITY_TVOS
		AddViewControllerRotationHandling(
			[SplashScreenController class],
			(IMP)&WillRotateToInterfaceOrientation_DefaultImpl, (IMP)&DidRotateFromInterfaceOrientation_DefaultImpl,
			(IMP)&ViewWillTransitionToSize_DefaultImpl
		);
#endif
	}
	return self;
}

- (void)create:(UIWindow*)window
{
	NSArray* supportedOrientation = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
	bool isIphone = UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone;
	bool isIpad = !isIphone;

	// splash will be shown way before unity is inited so we need to override autorotation handling with values read from info.plist
	_canRotateToPortrait			= [supportedOrientation containsObject:@"UIInterfaceOrientationPortrait"];
	_canRotateToPortraitUpsideDown	= [supportedOrientation containsObject:@"UIInterfaceOrientationPortraitUpsideDown"];
	_canRotateToLandscapeLeft		= [supportedOrientation containsObject:@"UIInterfaceOrientationLandscapeRight"];
	_canRotateToLandscapeRight		= [supportedOrientation containsObject:@"UIInterfaceOrientationLandscapeLeft"];

	CGSize size = [[UIScreen mainScreen] bounds].size;

	// iPads and iPhone 6+ have orientable splash screen
	_isOrientable = isIpad || (size.height == 736 || size.width == 736);

	// Launch screens are used only on iOS8+ iPhones
	const char* xib = UnityGetLaunchScreenXib();
#if !UNITY_TVOS
	_usesLaunchscreen = false;
	if (_ios80orNewer && xib != NULL)
	{
		const char* expectedName = isIphone ? "LaunchScreen-iPhone" : "LaunchScreen-iPad";
		if (std::strcmp(xib, expectedName) == 0)
			_usesLaunchscreen = true;
	}
#else
	_usesLaunchscreen = false;
#endif

	if (_usesLaunchscreen && !(_canRotateToPortrait || _canRotateToPortraitUpsideDown))
		_nonOrientableDefaultOrientation = landscapeLeft;
	else
		_nonOrientableDefaultOrientation = portrait;

	_splash = [[SplashScreen alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
	_splash.contentScaleFactor = [UIScreen mainScreen].scale;

	if (_isOrientable)
	{
		_splash.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
		_splash.autoresizesSubviews = YES;
	}
	else if (_canRotateToPortrait || _canRotateToPortraitUpsideDown)
	{
		_canRotateToLandscapeLeft = false;
		_canRotateToLandscapeRight = false;
	}
	// launch screens always use landscapeLeft in landscape on non-orientable
	// devices
	if (!_isOrientable && _usesLaunchscreen && _canRotateToLandscapeLeft)
		_canRotateToLandscapeRight = false;

	self.view = _splash;

#if !UNITY_TVOS
	self.wantsFullScreenLayout = TRUE;
#endif

	[window addSubview:_splash];
	window.rootViewController = self;
	[window bringSubviewToFront:_splash];

	ScreenOrientation orient = UIViewControllerOrientation(self);
	[_splash updateOrientation:orient];
	
	if (!_isOrientable)
		orient = _nonOrientableDefaultOrientation;

	OrientView([SplashScreenController Instance], _splash, orient);
}

- (BOOL)shouldAutorotate
{
	return YES;
}

- (NSUInteger)supportedInterfaceOrientations
{
	NSUInteger ret = 0;

	if(_canRotateToPortrait)			ret |= (1 << UIInterfaceOrientationPortrait);
	if(_canRotateToPortraitUpsideDown)	ret |= (1 << UIInterfaceOrientationPortraitUpsideDown);
	if(_canRotateToLandscapeLeft)		ret |= (1 << UIInterfaceOrientationLandscapeRight);
	if(_canRotateToLandscapeRight)		ret |= (1 << UIInterfaceOrientationLandscapeLeft);

	return ret;
}

+ (SplashScreenController*)Instance
{
	return _controller;
}

@end

void ShowSplashScreen(UIWindow* window)
{
	_controller = [[SplashScreenController alloc] init];
	[_controller create:window];
}

void HideSplashScreen()
{
	if(_splash)
	{
		[_splash removeFromSuperview];
		[_splash FreeSubviews];
	}

	_splash = nil;
	_controller = nil;
}
