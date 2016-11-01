
#if UNITY_TVOS

#include "UnityViewControllerBaseTV.h"
#include "OrientationSupport.h"
#include "Keyboard.h"
#include "UnityView.h"
#include "iAD.h"
#include "PluginBase/UnityViewControllerListener.h"
#include "UnityAppController.h"
#include "UnityAppController+ViewHandling.h"
#include "Unity/ObjCRuntime.h"

#include <math.h>

@implementation UnityViewControllerBase

- (id)init
{
	return [super init];
}

- (void)viewDidDisappear:(BOOL)animated
{
	[super viewDidDisappear:animated];
	AppController_SendUnityViewControllerNotification(kUnityViewDidDisappear);
}

- (void)viewWillDisappear:(BOOL)animated
{
	[super viewWillDisappear:animated];
	AppController_SendUnityViewControllerNotification(kUnityViewWillDisappear);
}

- (void)viewDidAppear:(BOOL)animated
{
	[super viewDidAppear:animated];
	AppController_SendUnityViewControllerNotification(kUnityViewDidAppear);
}

- (void)viewWillAppear:(BOOL)animated
{
	[super viewWillAppear:animated];
	AppController_SendUnityViewControllerNotification(kUnityViewWillAppear);
}
@end

@implementation UnityDefaultTVViewController
@end

extern "C" void UnityNotifyAutoOrientationChange()
{
}

#endif // UNITY_TVOS
