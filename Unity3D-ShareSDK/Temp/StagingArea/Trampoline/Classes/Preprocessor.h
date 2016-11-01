#pragma once

#include <Availability.h>

//------------------------------------------------------------------------------
//
// ensuring proper compiler/xcode/whatever selection
//

#ifndef __clang__
#error please use clang compiler.
#endif

// NOT the best way but apple do not care about adding extensions properly
#if __clang_major__ < 6
#error please use xcode 6.0 or newer
#endif

#if !defined(__IPHONE_8_0) || __IPHONE_OS_VERSION_MAX_ALLOWED < __IPHONE_8_0
#error please use ios sdk 8.0 or newer
#endif

#if !defined(__IPHONE_6_0) || __IPHONE_OS_VERSION_MIN_REQUIRED < __IPHONE_6_0
#error please target ios 6.0 or newer
#endif


//------------------------------------------------------------------------------
//
// defines for sdk/target version
//

#define UNITY_PRE_IOS7_TARGET (__IPHONE_OS_VERSION_MIN_REQUIRED < __IPHONE_7_0)

#if !TARGET_IPHONE_SIMULATOR && !TARGET_TVOS_SIMULATOR
	#define UNITY_CAN_USE_METAL		1
#else
	#define UNITY_CAN_USE_METAL		0
#endif

#define UNITY_USES_REMOTE_NOTIFICATIONS 1

#define USE_IL2CPP_PCH 0

#define UNITY_TVOS TARGET_OS_TV
#define UNITY_IOS !TARGET_OS_TV

#if !defined(__IPHONE_9_0)
	#define UNITY_REPLAY_KIT_AVAILABLE 0
#else
	#define UNITY_REPLAY_KIT_AVAILABLE (defined(UNITY_REPLAY_KIT_USED) && (UNITY_IOS && (__IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_9_0)))
#endif
