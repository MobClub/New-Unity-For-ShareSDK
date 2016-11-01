
//==============================================================================
//
//  ReplayKit Unity Interface


#import "UnityReplayKit.h"

extern "C"
{

#if UNITY_REPLAY_KIT_AVAILABLE 
int UnityReplayKitAPIAvailable()
{
    return [UnityReplayKit sharedInstance].apiAvailable ? 1 : 0;
}
    
int UnityReplayKitRecordingAvailable()
{
    return [UnityReplayKit sharedInstance].recordingPreviewAvailable ? 1 : 0;
}
    
const char* UnityReplayKitLastError()
{
    NSString* err = [UnityReplayKit sharedInstance].lastError;
    if (err == nil)
    {
        return NULL;
    }
    const char* error = [err cStringUsingEncoding:NSUTF8StringEncoding];
    if (error != NULL)
    {
        error = strdup(error);
    }
    return error;
}

int UnityReplayKitStartRecording(int enableMicrophone)
{
    bool enableMic = enableMicrophone != 0;
    return [[UnityReplayKit sharedInstance] startRecording:enableMic] ? 1 : 0;
}

int UnityReplayKitIsRecording()
{
    return [UnityReplayKit sharedInstance].isRecording ? 1 : 0;
}
    
int UnityReplayKitStopRecording()
{
    return [[UnityReplayKit sharedInstance] stopRecording] ? 1 : 0;
}

int UnityReplayKitDiscard()
{
    return [[UnityReplayKit sharedInstance] discardPreview] ? 1 : 0;
}
    
int UnityReplayKitPreview()
{
    return [[UnityReplayKit sharedInstance] showPreview] ? 1 : 0;
}

#else

// Impl when ReplayKit is not available.

int UnityReplayKitAPIAvailable()        { return 0; }
int UnityReplayKitRecordingAvailable()  { return 0; }
const char* UnityReplayKitLastError()   { return NULL; }
int UnityReplayKitStartRecording(int enableMicrophone) { return 0; }
int UnityReplayKitIsRecording()         { return 0; }
int UnityReplayKitStopRecording()       { return 0; }
int UnityReplayKitDiscard()             { return 0; }
int UnityReplayKitPreview()             { return 0; }

#endif  // UNITY_REPLAY_KIT_AVAILABLE

}  // extern "C"

