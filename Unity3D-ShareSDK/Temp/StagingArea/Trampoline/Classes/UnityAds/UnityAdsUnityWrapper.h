//
//  UnityAdsUnityWrapper.h
//  Copyright (c) 2013 Unity Technologies. All rights reserved.
//

#include "UnityAdsConfig.h"
#if USE_UNITYADS

#import <Foundation/Foundation.h>
#import <UnityAds/UnityAds.h>

extern UIViewController* UnityGetGLViewController();

@interface UnityAdsUnityWrapper : NSObject <UnityAdsDelegate> {
}

- (id)initWithGameId:(NSString*)gameId testModeOn:(bool)testMode debugModeOn:(bool)debugMode;

@end

// CALLBACKS

extern "C" {
	void UnityAdsOnCampaignsAvailable();
	void UnityAdsOnCampaignsFetchFailed();
	void UnityAdsOnShow();
	void UnityAdsOnHide();
	void UnityAdsOnVideoCompleted(const char *rewardItemKey, bool skipped);
	void UnityAdsOnVideoStarted();
}

#endif
