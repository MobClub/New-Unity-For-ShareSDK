#include "iAD.h"

//==============================================================================
//
//  iAD Unity Interface

bool UnityAD_BannerTypeAvailable(int type)
{
	if(type == adbannerBanner || type == adbannerMediumRect)
		return true;

	return false;
}

void* UnityAD_CreateBanner(int type, int layout)
{
#if UNITY_IOS
	UnityADBanner* banner = [[UnityADBanner alloc] initWithParent:UnityGetGLView() layout:(ADBannerLayout)layout type:(ADBannerType)type];
	return (__bridge_retained void*)banner;
#else
	return NULL;
#endif
}

void UnityAD_DestroyBanner(void* target)
{
#if UNITY_IOS
	UnityADBanner* banner = (__bridge_transfer UnityADBanner*)target;
	banner = nil;
#endif
}

void UnityAD_ShowBanner(void* target, bool show)
{
#if UNITY_IOS
	[(__bridge UnityADBanner*)target showBanner:show];
#endif
}

void UnityAD_MoveBanner(void* target, float /*x_*/, float y_)
{
#if UNITY_IOS
	UnityADBanner* banner = (__bridge UnityADBanner*)target;

	UIView* view   = banner.view;
	UIView* parent = view.superview;

	float x = parent.bounds.size.width/2;
	float h = view.bounds.size.height;
	float y = parent.bounds.size.height * y_ + h/2;

	[banner positionForUserLayout:CGPointMake(x, y)];
	[banner layoutBanner:adbannerManual];
	[parent layoutSubviews];
#endif
}

void UnityAD_BannerPosition(void* target, float* x, float* y)
{
#if UNITY_IOS
	UIView* view   = ((__bridge UnityADBanner*)target).view;
	UIView* parent = view.superview;

	CGPoint	c	= view.center;
	CGSize	ext	= view.bounds.size, pext = parent.bounds.size;

	*x = (c.x - ext.width/2)  / pext.width;
	*y = (c.y - ext.height/2) / pext.height;
#endif
}

void UnityAD_BannerSize(void* target, float* w, float* h)
{
#if UNITY_IOS
	UIView* view   = ((__bridge UnityADBanner*)target).view;
	UIView* parent = view.superview;

	CGSize ext = view.bounds.size, pext = parent.bounds.size;

	*w = ext.width  / pext.width;
	*h = ext.height / pext.height;
#endif
}

void UnityAD_LayoutBanner(void* target, int layout)
{
#if UNITY_IOS
	[(__bridge UnityADBanner*)target layoutBanner:(ADBannerLayout)layout];
#endif
}

bool UnityAD_BannerAdLoaded(void* target)
{
#if UNITY_IOS
	return ((__bridge UnityADBanner*)target).view.bannerLoaded;
#else
	return false;
#endif
}

bool UnityAD_BannerAdVisible(void* target)
{
#if UNITY_IOS
	return ((__bridge UnityADBanner*)target).adVisible;
#else
	return false;
#endif
}


bool UnityAD_InterstitialAvailable()
{
#if UNITY_IOS
	return UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad || _ios70orNewer;
#else
	return false;
#endif
}

void* UnityAD_CreateInterstitial(bool autoReload)
{
#if UNITY_IOS
	if(!UnityAD_InterstitialAvailable())
	{
		::printf("ADInterstitialAd is not available.\n");
		return 0;
	}

	UnityInterstitialAd* ad = [[UnityInterstitialAd alloc] initWithController:UnityGetGLViewController() autoReload:autoReload];
	return (__bridge_retained void*)ad;
#else
    return NULL;
#endif
}
void UnityAD_DestroyInterstitial(void* target)
{
#if UNITY_IOS
	if(target)
	{
		UnityInterstitialAd* ad = (__bridge_transfer UnityInterstitialAd*)target;
		ad = nil;
	}
#endif
}

void UnityAD_ShowInterstitial(void* target)
{
#if UNITY_IOS
	if(target)
		[(__bridge UnityInterstitialAd*)target show];
#endif
}

void UnityAD_ReloadInterstitial(void* target)
{
#if UNITY_IOS
	if(target)
		[(__bridge UnityInterstitialAd*)target reloadAD];
#endif
}

bool UnityAD_InterstitialAdLoaded(void* target)
{
#if UNITY_IOS
	return target ? ((__bridge UnityInterstitialAd*)target).interstitial.loaded : false;
#else
	return false;
#endif
}
