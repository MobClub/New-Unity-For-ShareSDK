#pragma once

#if UNITY_TVOS

#import <UIKit/UIKit.h>
#import <GameController/GCController.h>

@class UnityView;

@interface UnityViewControllerBase : GCEventViewController
{
}
@end

// this is default view controller implementation
//
@interface UnityDefaultTVViewController : UnityViewControllerBase
{
}
@end

#endif
