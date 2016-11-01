
#include "OrientationSupport.h"
#include "iAD.h"

#include "UnityAppController+ViewHandling.h"
#include "UnityView.h"

#if !UNITY_TVOS
@implementation UnityADBanner

@synthesize view = _view;
@synthesize adVisible = _showingBanner;

- (void)initImpl:(UIView*)parent layout:(ADBannerLayout)layout type:(ADBannerType)type
{
	UnityRegisterViewControllerListener((id<UnityViewControllerListener>)self);

	_view = [[ADBannerView alloc] initWithAdType:(ADAdType)type];
	_view.contentScaleFactor = [UIScreen mainScreen].scale;
	_view.bounds = parent.bounds;
	_view.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;

	_view.delegate = self;

	_bannerLayout	= layout;
	_showingBanner	= NO;

	[parent addSubview:_view];
	[self layoutBannerImpl];

	UnitySetViewTouchProcessing(_view, touchesTransformedToUnityViewCoords);
}

- (float)layoutXImpl:(UIView*)parent
{
	bool	rectBanner	= _view.adType == ADAdTypeMediumRectangle;
	float	x			= parent.bounds.size.width/2;
	if(_bannerLayout == adbannerManual)
	{
		x = rectBanner ? _userLayoutCenter.x : parent.bounds.size.width/2;
	}
	else if(rectBanner)
	{
		int horz = (_bannerLayout & layoutMaskHorz) >> layoutShiftHorz;
		if(horz == layoutMaskLeft)			x = _view.bounds.size.width / 2;
		else if(horz == layoutMaskRight)	x = parent.bounds.size.width - _view.bounds.size.width / 2;
		else if(horz == layoutMaskCenter)	x = parent.bounds.size.width / 2;
		else								x = _userLayoutCenter.x;
	}

	return x;
}

- (float)layoutYImpl:(UIView*)parent
{
	if(!_showingBanner)
		return parent.bounds.size.height + _view.bounds.size.height;

	bool	rectBanner	= _view.adType == ADAdTypeMediumRectangle;
	float	y			= 0;
	if(_bannerLayout == adbannerManual)
	{
		y = _userLayoutCenter.y;
	}
	else
	{
		int vert = rectBanner ? (_bannerLayout & layoutMaskVert) : (_bannerLayout & 1);

		if(vert == layoutMaskTop)			y = _view.bounds.size.height / 2;
		else if(vert == layoutMaskBottom)	y = parent.bounds.size.height - _view.bounds.size.height / 2;
		else if(vert == layoutMaskCenter)	y = parent.bounds.size.height / 2;
		else								y = _userLayoutCenter.y;
	}

	return y;
}

- (void)layoutBannerImpl
{
	UIView* parent = _view.superview;

	float cx = [self layoutXImpl:parent];
	float cy = [self layoutYImpl:parent];

	CGRect rect = _view.bounds;
	rect.size = [_view sizeThatFits:parent.bounds.size];

	_view.center = CGPointMake(cx,cy);
	_view.bounds = rect;

	[parent layoutSubviews];
}

- (id)initWithParent:(UIView*)parent layout:(ADBannerLayout)layout type:(ADBannerType)type
{
	if( (self = [super init]) )
		[self initImpl:parent layout:layout type:type];
	return self;
}
- (id)initWithParent:(UIView*)parent layout:(ADBannerLayout)layout;
{
	if( (self = [super init]) )
		[self initImpl:parent layout:layout type:adbannerBanner];
	return self;
}

- (void)dealloc
{
	// dtor might be called from a separate thread by a garbage collector
	// so we need a new autorelease pool in case threre are autoreleased objects
	@autoreleasepool
	{
		UnityUnregisterViewControllerListener((id<UnityViewControllerListener>)self);
		UnityDropViewTouchProcessing(_view);

		_view.delegate = nil;
		[_view performSelectorOnMainThread:@selector(removeFromSuperview) withObject:nil waitUntilDone:NO];
		_view = nil;
	}
}

- (void)interfaceWillChangeOrientation:(NSNotification*)notification
{
	_view.hidden = YES;
}
- (void)interfaceDidChangeOrientation:(NSNotification*)notification
{
	if(_showingBanner)
		_view.hidden = NO;

	[self layoutBannerImpl];
}

- (void)layoutBanner:(ADBannerLayout)layout
{
	_bannerLayout = layout;
	[self layoutBannerImpl];
}

- (void)positionForUserLayout:(CGPoint)center
{
	_userLayoutCenter = center;
	[self layoutBannerImpl];
}

- (void)showBanner:(BOOL)show
{
	_view.hidden = NO;
	_showingBanner = show;
	[self layoutBannerImpl];
}

- (BOOL)bannerViewActionShouldBegin:(ADBannerView*)banner willLeaveApplication:(BOOL)willLeave
{
	if(!willLeave)
		UnityPause(1);
	return YES;
}

- (void)bannerViewActionDidFinish:(ADBannerView*)banner
{
	UnityPause(0);
	UnityADBannerViewWasClicked();
}

- (void)bannerViewDidLoadAd:(ADBannerView*)banner
{
	UnityADBannerViewWasLoaded();
}

- (void)bannerView:(ADBannerView*)banner didFailToReceiveAdWithError:(NSError*)error
{
	::printf("ADBannerView error: %s\n", [[error localizedDescription] UTF8String]);
	_showingBanner = NO;
	UnityADBannerViewFailedToLoad();
	[self layoutBannerImpl];
}

@end

#endif

#if !UNITY_TVOS
@implementation UnityInterstitialAd
{
	// on ios9+ interstitialAdActionDidFinish will be called from deep inside of iAD framework, when tweaking internal view controller
	// it will arrive before showing actual ad (internal dismiss previous call)
	// and multiple times when we actuall dismiss ad
	// so we will use bool var to do magic ONLY when actually needed
	BOOL	_didShowAd;
}

@synthesize interstitial = _interstitial;

- (void)_unloadAD
{
	_interstitial.delegate = nil;
	_interstitial = nil;
}
- (void)_loadAD
{
	_interstitial = [[ADInterstitialAd alloc] init];
	_interstitial.delegate = self;
}
- (void)_handleReloadAD
{
	[self _unloadAD];
	if(_autoReload)
		[self _loadAD];
}

- (id)initWithController:(UIViewController*)presentController autoReload:(BOOL)autoReload
{
	if( (self = [super init]) )
	{
		_presentController	= presentController;
		_autoReload			= autoReload;
		[self _loadAD];
	}

	return self;
}
- (void)dealloc
{
	UnityUnregisterViewControllerListener((id<UnityViewControllerListener>)self);

	// dtor might be called from a separate thread by a garbage collector
	// so we need a new autorelease pool in case there are autoreleased objects
	@autoreleasepool
	{
		[self _unloadAD];
	}
}

- (void)show
{
	// we care about view controller events ONLY when we are showing ad
	UnityRegisterViewControllerListener((id<UnityViewControllerListener>)self);

	_didShowAd = YES;
	[_interstitial presentFromViewController:_presentController];
}
- (void)reloadAD
{
	[self _unloadAD];
	[self _loadAD];
}

- (BOOL)interstitialAdActionShouldBegin:(ADInterstitialAd*)interstitialAd willLeaveApplication:(BOOL)willLeave
{
	UnityADInterstitialADWasViewed();
	return YES;
}
- (void)interstitialAd:(ADInterstitialAd*)interstitialAd didFailWithError:(NSError*)error
{
	::printf("ADInterstitialAd error: %s\n", [[error localizedDescription] UTF8String]);
	[self reloadAD];
}
- (void)interstitialAdDidLoad:(ADInterstitialAd*)interstitialAd
{
	UnityADInterstitialADWasLoaded();
}
- (void)interstitialAdDidUnload:(ADInterstitialAd*)interstitialAd
{
	[self _handleReloadAD];
}
- (void)interstitialAdActionDidFinish:(ADInterstitialAd*)interstitialAd
{
	// interstitialAdActionDidFinish will be called multiple times on actual ad finish, and actually before we can reload ad
	// so we dispatch reload request to happen a bit later (and when we get back to main loop)
	if(_didShowAd)
	{
		_didShowAd = NO;
		[self performSelectorOnMainThread:@selector(_handleReloadAD) withObject:nil waitUntilDone:NO];
	}
}

// viewDidDisappear/viewWillAppear will arrive when ad is shown/dismissed, OR when ad is transitioning from first screen to animated sequence
// so it will be (exit on first screen)
// viewDidDisappear -> viewWillAppear
// or (user went into animated sequence)
// viewDidDisappear -> [viewWillAppear -> viewDidDisappear] (due to transition) -> viewWillAppear (when we get back)
// when we transition to animated sequence we will be paused automatically, so we care only about first viewDidDisappear/viewWillAppear

- (void)viewDidDisappear:(NSNotification*)notification
{
	UnityPause(1);
}
- (void)viewWillAppear:(NSNotification*)notification
{
	UnityPause(0);
	UnityUnregisterViewControllerListener((id<UnityViewControllerListener>)self);
}

@end
#endif
