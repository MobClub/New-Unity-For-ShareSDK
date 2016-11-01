#pragma once

#include <stdint.h>
#include <stdarg.h>

#include "UnityForwardDecls.h"
#include "UnityRendering.h"

// unity plugin functions

// audio plugin api
typedef int     (*UnityPluginGetAudioEffectDefinitionsFunc)(struct UnityAudioEffectDefinition*** descptr);

// OLD rendering plugin api (will become obsolete soon)
typedef	void	(*UnityPluginSetGraphicsDeviceFunc)(void* device, int deviceType, int eventType);
typedef	void	(*UnityPluginRenderMarkerFunc)(int marker);

// new rendering plugin api
typedef void	(*UnityPluginLoadFunc)(struct IUnityInterfaces* unityInterfaces);
typedef void	(*UnityPluginUnloadFunc)();


// log handler function
#ifdef __cplusplus
	typedef bool (*LogEntryHandler)(LogType logType, const char* log, va_list list);
#endif

//
// these are functions referenced in trampoline and implemented in unity player lib
//

#ifdef __cplusplus
extern "C" {
#endif

// life cycle management

void	UnityParseCommandLine(int argc, char* argv[]);
void	UnityInitApplicationNoGraphics(const char* appPathName);
void	UnityInitApplicationGraphics();
void	UnityCleanup();
void	UnityLoadApplication();
void	UnityPlayerLoop();					// normal player loop
void	UnityBatchPlayerLoop();				// batch mode like player loop, without rendering (usable for background processing)
void	UnitySetPlayerFocus(int focused);	// send OnApplicationFocus() message to scripts
void	UnityPause(int pause);
int		UnityIsPaused();					// 0 if player is running, 1 if paused
void	UnityWillPause();					// send the message that app will pause
void	UnityWillResume();					// send the message that app will resume
void	UnityInputProcess();
void	UnityDeliverUIEvents();				// unity processing impacting UI will be called in there


// rendering

int		UnityGetRenderingAPIs(int capacity, int* outAPIs);
int		UnityHasRenderingAPIExtension(const char* extension);
void	UnityFinishRendering();

// for Create* functions if surf is null we will actuially create new one, otherwise we update the one provided
// gles: one and only one of texid/rbid should be non-zero
// metal: resolveTex should be non-nil only if tex have AA
UnityRenderBufferHandle	UnityCreateExternalSurfaceGLES(UnityRenderBufferHandle surf, int isColor, unsigned texid, unsigned rbid, unsigned glesFormat, const UnityRenderBufferDesc* desc);
UnityRenderBufferHandle	UnityCreateExternalSurfaceMTL(UnityRenderBufferHandle surf, int isColor, MTLTextureRef tex, const UnityRenderBufferDesc* desc);
UnityRenderBufferHandle	UnityCreateExternalColorSurfaceMTL(UnityRenderBufferHandle surf, MTLTextureRef tex, MTLTextureRef resolveTex, const UnityRenderBufferDesc* desc);
UnityRenderBufferHandle	UnityCreateExternalDepthSurfaceMTL(UnityRenderBufferHandle surf, MTLTextureRef tex, MTLTextureRef stencilTex, const UnityRenderBufferDesc* desc);
// creates "dummy" surface - will indicate "missing" buffer (e.g. depth-only RT will have color as dummy)
UnityRenderBufferHandle	UnityCreateDummySurface(UnityRenderBufferHandle surf, int isColor, const UnityRenderBufferDesc* desc);

// disable rendering to render buffers (all Cameras that were rendering to one of buffers would be reset to use backbuffer)
void	UnityDisableRenderBuffers(UnityRenderBufferHandle color, UnityRenderBufferHandle depth);
// destroys render buffer
void	UnityDestroyExternalSurface(UnityRenderBufferHandle surf);
// sets current render target
void	UnitySetRenderTarget(UnityRenderBufferHandle color, UnityRenderBufferHandle depth);
// final blit to backbuffer
void	UnityBlitToBackbuffer(UnityRenderBufferHandle srcColor, UnityRenderBufferHandle dstColor, UnityRenderBufferHandle dstDepth);
// get native renderbuffer from handle
UnityRenderBuffer	UnityNativeRenderBufferFromHandle(UnityRenderBufferHandle rb);

// This must match the one in ApiEnumsGLES.h
typedef enum UnityFramebufferTarget
{
	kDrawFramebuffer = 0,
	kReadFramebuffer,
	kFramebufferTargetCount
}UnityFramebufferTarget;
void	UnityBindFramebuffer(UnityFramebufferTarget target, int fbo);
void	UnityRegisterFBO(UnityRenderBufferHandle color, UnityRenderBufferHandle depth, unsigned fbo);

// OLD metal plugins support (will become obsolete soon)

MTLCommandEncoderRef	UnityCurrentMTLCommandEncoder();
void					UnityEndCurrentMTLCommandEncoder();
MTLTextureRef			UnityRenderBufferMTLTexture(UnityRenderBuffer buffer);
MTLTextureRef			UnityRenderBufferAAResolvedMTLTexture(UnityRenderBuffer buffer);
MTLTextureRef			UnityRenderBufferStencilMTLTexture(UnityRenderBuffer buffer);


// controling player internals

// TODO: needs some cleanup
void	UnitySetAudioSessionActive(int active);
void	UnityGLInvalidateState();
void	UnityReloadResources();
int		UnityIsCaptureScreenshotRequested();
void	UnityCaptureScreenshot();
void	UnitySendMessage(const char* obj, const char* method, const char* msg);

EAGLContext*		UnityGetDataContextGLES();
MTLCommandBufferRef	UnityCurrentMTLCommandBuffer();

#ifdef __cplusplus
	void	UnitySetLogEntryHandler(LogEntryHandler newHandler);
#endif


// plugins support

// WARNING: old UnityRegisterRenderingPlugin will become obsolete soon
void	UnityRegisterRenderingPlugin(UnityPluginSetGraphicsDeviceFunc setDevice, UnityPluginRenderMarkerFunc renderMarker);

void	UnityRegisterRenderingPluginV5(UnityPluginLoadFunc loadPlugin, UnityPluginUnloadFunc unloadPlugin);
void	UnityRegisterAudioPlugin(UnityPluginGetAudioEffectDefinitionsFunc getAudioEffectDefinitions);


// resolution/orientation handling

void	UnityGetRenderingResolution(unsigned* w, unsigned* h);
void	UnityGetSystemResolution(unsigned* w, unsigned* h);

void	UnityRequestRenderingResolution(unsigned w, unsigned h);

int		UnityIsOrientationEnabled(unsigned /*ScreenOrientation*/ orientation);
int		UnityShouldAutorotate();
int		UnityRequestedScreenOrientation(); // returns ScreenOrientation

int		UnityReportResizeView(unsigned w, unsigned h, unsigned /*ScreenOrientation*/ contentOrientation);	// returns ScreenOrientation
void	UnityReportBackbufferChange(UnityRenderBufferHandle colorBB, UnityRenderBufferHandle depthBB);



// player settings

int		UnityDisableDepthAndStencilBuffers();
int		UnityUseAnimatedAutorotation();
int		UnityGetDesiredMSAASampleCount(int defaultSampleCount);
int		UnityGetSRGBRequested();
int		UnityGetShowActivityIndicatorOnLoading();
int		UnityGetAccelerometerFrequency();
int		UnityGetTargetFPS();
int		UnityGetAppBackgroundBehavior();


// push notifications
#if !UNITY_TVOS
void	UnitySendLocalNotification(UILocalNotification* notification);
#endif
void	UnitySendRemoteNotification(NSDictionary* notification);
void	UnitySendDeviceToken(NSData* deviceToken);
void	UnitySendRemoteNotificationError(NSError* error);

// native events

void	UnityADBannerViewWasClicked();
void	UnityADBannerViewWasLoaded();
void	UnityADBannerViewFailedToLoad();
void	UnityADInterstitialADWasLoaded();
void	UnityADInterstitialADWasViewed();
void	UnityUpdateDisplayList();


// profiler

void*	UnityCreateProfilerCounter(const char*);
void	UnityDestroyProfilerCounter(void*);
void	UnityStartProfilerCounter(void*);
void	UnityEndProfilerCounter(void*);


// sensors

void	UnitySensorsSetGyroRotationRate(int idx, float x, float y, float z);
void	UnitySensorsSetGyroRotationRateUnbiased(int idx, float x, float y, float z);
void	UnitySensorsSetGravity(int idx, float x, float y, float z);
void	UnitySensorsSetUserAcceleration(int idx, float x, float y, float z);
void	UnitySensorsSetAttitude(int idx, float x, float y, float z, float w);
void	UnityDidAccelerate(float x, float y, float z, double timestamp);
void	UnitySetJoystickPosition (int joyNum, int axis, float pos);
int		UnityStringToKey(const char *name);
void	UnitySetKeyState (int key, int /*bool*/ state);

// WWW connection handling

void	UnityReportWWWStatusError(void* udata, int status, const char* error);

void	UnityReportWWWReceivedResponse(void* udata, int status, unsigned expectedDataLength, const char* respHeader);
void	UnityReportWWWReceivedData(void* udata, const void* buffer, unsigned totalRead, unsigned expectedTotal);
void	UnityReportWWWFinishedLoadingData(void* udata);
void	UnityReportWWWSentData(void* udata, unsigned totalWritten, unsigned expectedTotal);

// AVCapture

void	UnityReportAVCapturePermission();
void	UnityDidCaptureVideoFrame(intptr_t tex, void* udata);

// logging override

#ifdef __cplusplus
} // extern "C"
#endif


// touches processing

#ifdef __cplusplus
extern "C" {
#endif

void	UnitySetViewTouchProcessing(UIView* view, int /*ViewTouchProcessing*/ processingPolicy);
void	UnityDropViewTouchProcessing(UIView* view);

void	UnitySendTouchesBegin(NSSet* touches, UIEvent* event);
void	UnitySendTouchesEnded(NSSet* touches, UIEvent* event);
void	UnitySendTouchesCancelled(NSSet* touches, UIEvent* event);
void	UnitySendTouchesMoved(NSSet* touches, UIEvent* event);

void	UnityCancelTouches();

#ifdef __cplusplus
} // extern "C"
#endif


//
// these are functions referenced and implemented in trampoline
//

#ifdef __cplusplus
extern "C" {
#endif

// UnityAppController.mm
UIViewController*		UnityGetGLViewController();
UIView*					UnityGetGLView();
UIWindow*				UnityGetMainWindow();
enum ScreenOrientation	UnityCurrentOrientation();

// Unity/DisplayManager.mm
float					UnityScreenScaleFactor(UIScreen* screen);

#ifdef __cplusplus
} // extern "C"
#endif


//
// these are functions referenced in unity player lib and implemented in trampoline
//

#ifdef __cplusplus
extern "C" {
#endif

// iPhone_Sensors.mm
void			UnityInitJoysticks();
void			UnityCoreMotionStart();
void			UnityCoreMotionStop();
int				UnityIsGyroEnabled(int idx);
int				UnityIsGyroAvailable();
void			UnityUpdateGyroData();
void			UnitySetGyroUpdateInterval(int idx, float interval);
float			UnityGetGyroUpdateInterval(int idx);
void			UnityUpdateJoystickData();
int				UnityGetJoystickCount();
void			UnityGetJoystickName(int idx, char* buffer, int maxLen);
void			UnityGetJoystickAxisName(int idx, int axis, char* buffer, int maxLen);
void			UnityGetNiceKeyname(int key, char* buffer, int maxLen);

// UnityAppController+Rendering.mm
void			UnityInitMainScreenRenderingCallback();
void			UnityGfxInitedCallback();
void			UnityPresentContextCallback(struct UnityFrameStats const* frameStats);
void			UnityFramerateChangeCallback(int targetFPS);
int				UnitySelectedRenderingAPI();

NSBundle*			UnityGetMetalBundle();
MTLDeviceRef		UnityGetMetalDevice();
MTLCommandQueueRef	UnityGetMetalCommandQueue();

EAGLContext*		UnityGetDataContextEAGL();

UnityRenderBufferHandle	UnityBackbufferColor();
UnityRenderBufferHandle	UnityBackbufferDepth();

// UI/ActivityIndicator.mm
void			UnityStartActivityIndicator();
void			UnityStopActivityIndicator();

// UI/Keyboard.mm
void			UnityKeyboard_Create(unsigned keyboardType, int autocorrection, int multiline, int secure, int alert, const char* text, const char* placeholder);
void			UnityKeyboard_Show();
void			UnityKeyboard_Hide();
void			UnityKeyboard_GetRect(float* x, float* y, float* w, float* h);
void			UnityKeyboard_SetText(const char* text);
NSString*		UnityKeyboard_GetText();
int				UnityKeyboard_IsActive();
void			UnityKeyboard_PrepareToShow();
int				UnityKeyboard_IsDone();
int				UnityKeyboard_WasCanceled();
void			UnityKeyboard_SetInputHidden(int hidden);
int				UnityKeyboard_IsInputHidden();

// UI/UnityViewControllerBase.mm
void			UnityNotifyAutoOrientationChange();

// Unity/AVCapture.mm
int				UnityGetAVCapturePermission(int captureTypes);
void			UnityRequestAVCapturePermission(int captureTypes);

// Unity/CameraCapture.mm
void			UnityEnumVideoCaptureDevices(void* udata, void(*callback)(void* udata, const char* name, int frontFacing));
void*			UnityInitCameraCapture(int device, int w, int h, int fps, void* udata);
void			UnityStartCameraCapture(void* capture);
void			UnityPauseCameraCapture(void* capture);
void			UnityStopCameraCapture(void* capture);
void			UnityCameraCaptureExtents(void* capture, int* w, int* h);
void			UnityCameraCaptureReadToMemory(void* capture, void* dst, int w, int h);
int				UnityCameraCaptureVideoRotationDeg(void* capture);
int				UnityCameraCaptureVerticallyMirrored(void* capture);


// Unity/DeviceSettings.mm
const char*		UnityDeviceUniqueIdentifier();
const char*		UnityVendorIdentifier();
const char*		UnityAdvertisingIdentifier();
int				UnityAdvertisingTrackingEnabled();
const char*		UnityDeviceName();
const char*		UnitySystemName();
const char*		UnitySystemVersion();
const char*		UnityDeviceModel();
int				UnityDeviceCPUCount();
int				UnityDeviceGeneration();
float			UnityDeviceDPI();
const char*		UnitySystemLanguage();

// Unity/DisplayManager.mm
EAGLContext*	UnityGetMainScreenContextGLES();
EAGLContext*	UnityGetContextEAGL();
void			UnityStartFrameRendering();

// Unity/Filesystem.mm
const char*		UnityApplicationDir();
const char*		UnityDocumentsDir();
const char*		UnityLibraryDir();
const char*		UnityCachesDir();
int				UnityUpdateNoBackupFlag(const char* path, int setFlag); // Returns 1 if successful, otherwise 0

// Unity/WWWConnection.mm
void*			UnityStartWWWConnectionGet(void* udata, const void* headerDict, const char* url);
void*			UnityStartWWWConnectionPost(void* udata, const void* headerDict, const char* url, const void* data, unsigned length);
void			UnityDestroyWWWConnection(void* connection);
void			UnityShouldCancelWWW(const void* connection);

//Apple TV Remote
int			UnityGetAppleTVRemoteAllowExitToMenu();
void		UnitySetAppleTVRemoteAllowExitToMenu(int val);
int			UnityGetAppleTVRemoteAllowRotation();
void		UnitySetAppleTVRemoteAllowRotation(int val);
int			UnityGetAppleTVRemoteReportAbsoluteDpadValues();
void		UnitySetAppleTVRemoteReportAbsoluteDpadValues(int val);
int			UnityGetAppleTVRemoteTouchesEnabled();
void		UnitySetAppleTVRemoteTouchesEnabled(int val);

#ifdef __cplusplus
} // extern "C"
#endif


#ifdef __OBJC__
	// This is basically a wrapper for [NSString UTF8String] with additional strdup.
	//
	// Apparently multiple calls on UTF8String will leak memory (NSData objects) that are collected
	// only when @autoreleasepool is exited. This function serves as documentation for this and as a
	// handy wrapper.
	inline char* AllocCString(NSString* value)
	{
		if(value == nil)
			return 0;

		const char* str = [value UTF8String];
		return str ? strdup(str) : 0;
	}
#endif
