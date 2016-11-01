#pragma once

#include "Unity/GlesHelper.h"
#include "iPhone_Sensors.h"

@interface UnityView : UnityRenderingView
{
}

// we take scale factor into account because gl backbuffer size depends on it
- (id)initWithFrame:(CGRect)frame scaleFactor:(CGFloat)scale;
- (id)initWithFrame:(CGRect)frame;
- (id)initFromMainScreen;

// in here we will go through subviews and call onUnityUpdateViewLayout selector (if present)
// that allows to handle simple overlay child view layout without doing view controller magic
- (void)layoutSubviews;

#if !UNITY_TVOS
// will simply update content orientation (it might be tweaked in layoutSubviews, due to disagreement between unity and view controller)
- (void)willRotateToOrientation:(UIInterfaceOrientation)toOrientation fromOrientation:(UIInterfaceOrientation)fromOrientation;
#endif

// will recreate gles backing if needed and repaint once to make sure we dont have black frame creeping in
- (void)didRotate;

- (void)recreateGLESSurfaceIfNeeded;
- (void)recreateGLESSurface;

- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event;

#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
- (void)pressesBegan:(NSSet<UIPress*>*)presses withEvent:(UIEvent*)event;
- (void)pressesEnded:(NSSet<UIPress*>*)presses withEvent:(UIEvent*)event;
#endif

// will match script-side Screen.orientation
@property (nonatomic, readonly) ScreenOrientation contentOrientation;

@end
