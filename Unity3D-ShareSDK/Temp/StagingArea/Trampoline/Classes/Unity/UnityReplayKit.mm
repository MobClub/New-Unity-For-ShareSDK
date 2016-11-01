#if UNITY_REPLAY_KIT_AVAILABLE

#import "UnityReplayKit.h"
#import "UnityAppController.h"


static UnityReplayKit* _replayKit = nil;

@implementation UnityReplayKit

+ (UnityReplayKit*)sharedInstance
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _replayKit = [[UnityReplayKit alloc] init];
    });
	return _replayKit;
}

- (BOOL)apiAvailable {
    return [RPScreenRecorder class] != nil;
}

- (BOOL)recordingPreviewAvailable
{
	return _previewController != nil;
}

- (BOOL)startRecording:(BOOL)enableMicrophone
{
	RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
	if (recorder == nil)
	{
		_lastError = [NSString stringWithUTF8String:"Failed to get Screen Recorder"];
		return NO;
	}
    
	recorder.delegate = self;
    __block BOOL success = YES;
	[recorder startRecordingWithMicrophoneEnabled:enableMicrophone handler:^(NSError* error){
		if (error != nil)
        {
			_lastError = [error description];
            success = NO;
        }
	}];
	
	return success;
}

- (BOOL)isRecording
{
	RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
	if (recorder == nil)
	{
		_lastError = [NSString stringWithUTF8String:"Failed to get Screen Recorder"];
		return NO;
	}
	return recorder.isRecording;
}

- (BOOL)stopRecording
{
	RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
	if (recorder == nil)
	{
		_lastError = [NSString stringWithUTF8String:"Failed to get Screen Recorder"];
		return NO;
	}
    
    __block BOOL success = YES;
	[recorder stopRecordingWithHandler:^(RPPreviewViewController* previewViewController, NSError* error){
		if (error != nil)
		{
			_lastError = [error description];
			success = NO;
			return;
		}
		if (previewViewController != nil)
		{
			[previewViewController setPreviewControllerDelegate:self];
			_previewController = previewViewController;
		}
	}];
	
	return success;
}

- (void)screenRecorder:(RPScreenRecorder*)screenRecorder didStopRecordingWithError:(NSError*)error previewViewController:(RPPreviewViewController*)previewViewController
{
    if (error != nil)
    {
		_lastError = [error description];
    }
	_previewController = previewViewController;
}

- (BOOL)showPreview
{
	if (_previewController == nil)
	{
		_lastError = [NSString stringWithUTF8String:"No recording available"];
		return NO;
	}
	
	[_previewController setModalPresentationStyle:UIModalPresentationFullScreen];
    [GetAppController().rootViewController presentViewController:_previewController animated:YES completion:^()
    {
        _previewController = nil;
    }];
	return YES;
}

- (BOOL)discardPreview
{
	if (_previewController == nil)
    {
		return YES;
    }
    
	RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
	if (recorder == nil)
	{
		_lastError = [NSString stringWithUTF8String:"Failed to get Screen Recorder"];
		return NO;
	}
    
	[recorder discardRecordingWithHandler:^()
    {
        _previewController = nil;
    }];
    // TODO - the above callback doesn't seem to be working at the moment.
    _previewController = nil;
	
    return YES;
}

- (void)previewControllerDidFinish:(RPPreviewViewController*)previewController
{
	if (previewController != nil)
    {
		[previewController dismissViewControllerAnimated:YES completion:nil];
    }
}

@end

#endif  // UNITY_REPLAY_KIT_AVAILABLE
