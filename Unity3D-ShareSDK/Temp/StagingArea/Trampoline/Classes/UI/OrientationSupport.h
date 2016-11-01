#pragma once

#include <CoreGraphics/CGAffineTransform.h>

#if !UNITY_TVOS
ScreenOrientation		ConvertToUnityScreenOrientation(UIInterfaceOrientation hwOrient);
UIInterfaceOrientation	ConvertToIosScreenOrientation(ScreenOrientation orient);
#endif
ScreenOrientation UIViewControllerOrientation(UIViewController* controller);

CGAffineTransform		TransformForOrientation(ScreenOrientation curOrient);
CGAffineTransform		TransformBetweenOrientations(ScreenOrientation fromOrient, ScreenOrientation toOrient);

ScreenOrientation		OrientationAfterTransform(ScreenOrientation curOrient, CGAffineTransform transform);

void					OrientView(UIViewController* host, UIView* view, ScreenOrientation to);
