#ifndef UnityReplayKit_h
#define UnityReplayKit_h

#if UNITY_REPLAY_KIT_AVAILABLE

#import <Foundation/Foundation.h>
#import <ReplayKit/ReplayKit.h>

@interface UnityReplayKit : NSObject<RPPreviewViewControllerDelegate, RPScreenRecorderDelegate>
{
}

+ (instancetype)sharedInstance;

@property(nonatomic, readonly) BOOL apiAvailable;

@property(nonatomic, readonly) NSString* lastError;
@property(nonatomic, readonly) RPPreviewViewController* previewController;
@property(nonatomic, readonly) BOOL recordingPreviewAvailable;
@property(nonatomic, readonly, getter=isRecording) BOOL recording;

- (BOOL)startRecording:(BOOL)enableMicrophone;
- (BOOL)stopRecording;
- (BOOL)showPreview;
- (BOOL)discardPreview;

- (void)screenRecorder:(RPScreenRecorder*)screenRecorder didStopRecordingWithError:(NSError*)error previewViewController:(RPPreviewViewController*)previewViewController;
- (void)previewControllerDidFinish:(RPPreviewViewController*)previewController;

@end


#endif  // UNITY_REPLAY_KIT_AVAILABLE

#endif  // UnityReplayKit_h
