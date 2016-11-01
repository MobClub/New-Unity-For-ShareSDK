
#if UNITY_CAN_USE_METAL

#include "UnityMetalSupport.h"
#include <QuartzCore/QuartzCore.h>
#include <libkern/OSAtomic.h>

#include <stdio.h>

#include "UnityAppController.h"
#include "DisplayManager.h"
#include "CVTextureCache.h"

extern bool _supportsMSAA;

extern "C" void InitRenderingMTL()
{
	_supportsMSAA = true;
}

static inline Class MTLTextureDescriptorClass()
{
	static Class _MTLTextureDescriptorClass = nil;
	if(_MTLTextureDescriptorClass == nil)
		_MTLTextureDescriptorClass = [UnityGetMetalBundle() classNamed:@"MTLTextureDescriptor"];
	return _MTLTextureDescriptorClass;
}


extern "C" void CreateSystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
	DestroySystemRenderingSurfaceMTL(surface);

	MTLPixelFormat colorFormat = surface->srgb ? MTLPixelFormatBGRA8Unorm_sRGB : MTLPixelFormatBGRA8Unorm;

	surface->layer.presentsWithTransaction = NO;
	surface->layer.drawsAsynchronously = YES;

	CGFloat backgroundColorValues[] = {0,0,0,1};
	CGColorSpaceRef colorSpaceRef = CGColorSpaceCreateDeviceRGB();
	CGColorRef backgroundColorRef = CGColorCreate(colorSpaceRef, backgroundColorValues);
	surface->layer.backgroundColor = backgroundColorRef; // retained automatically
	CGColorRelease(backgroundColorRef);
	CGColorSpaceRelease(colorSpaceRef);

	surface->layer.device = surface->device;
	surface->layer.pixelFormat = colorFormat;
	//surface->layer.framebufferOnly = YES;
	surface->layer.framebufferOnly = NO;
	surface->colorFormat = colorFormat;
}

extern "C" void CreateRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
	DestroyRenderingSurfaceMTL(surface);

	const int w = surface->targetW, h = surface->targetH;

	if(w != surface->systemW || h != surface->systemH || surface->useCVTextureCache)
	{
		if(surface->useCVTextureCache)
			surface->cvTextureCache = CreateCVTextureCache();

		if(surface->cvTextureCache)
		{
			surface->cvTextureCacheTexture = CreateReadableRTFromCVTextureCache(surface->cvTextureCache, surface->targetW, surface->targetH, &surface->cvPixelBuffer);
			surface->targetColorRT = GetMetalTextureFromCVTextureCache(surface->cvTextureCacheTexture);
		}
		else
		{
			MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass() new];
			txDesc.textureType = MTLTextureType2D;
			txDesc.width = w;
			txDesc.height = h;
			txDesc.depth = 1;
			txDesc.pixelFormat = surface->srgb ? MTLPixelFormatBGRA8Unorm_sRGB : MTLPixelFormatBGRA8Unorm;
			txDesc.arrayLength = 1;
			txDesc.mipmapLevelCount = 1;
			surface->targetColorRT = [surface->device newTextureWithDescriptor:txDesc];
		}
		surface->targetColorRT.label = @"targetColorRT";
	}

	if(surface->msaaSamples > 1)
	{
		MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass() new];
		txDesc.textureType = MTLTextureType2DMultisample;
		txDesc.width = w;
		txDesc.height = h;
		txDesc.depth = 1;
		txDesc.pixelFormat = surface->srgb ? MTLPixelFormatBGRA8Unorm_sRGB : MTLPixelFormatBGRA8Unorm;
		txDesc.arrayLength = 1;
		txDesc.mipmapLevelCount = 1;
		txDesc.sampleCount = surface->msaaSamples;
		surface->targetAAColorRT = [surface->device newTextureWithDescriptor:txDesc];
		surface->targetAAColorRT.label = @"targetAAColorRT";
	}
}



extern "C" void  DestroyRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
	surface->targetColorRT = nil;
	surface->targetAAColorRT = nil;

	if(surface->cvTextureCacheTexture)	CFRelease(surface->cvTextureCacheTexture);
	if(surface->cvPixelBuffer)			CFRelease(surface->cvPixelBuffer);
	if(surface->cvTextureCache)			CFRelease(surface->cvTextureCache);
	surface->cvTextureCache = 0;
}

extern "C" void CreateSharedDepthbufferMTL(UnityDisplaySurfaceMTL* surface)
{
	DestroySharedDepthbufferMTL(surface);

	MTLTextureDescriptor* depthTexDesc = [MTLTextureDescriptorClass() texture2DDescriptorWithPixelFormat:MTLPixelFormatDepth32Float width:surface->targetW height:surface->targetH mipmapped:NO];
	if(surface->msaaSamples > 1)
	{
		depthTexDesc.textureType = MTLTextureType2DMultisample;
		depthTexDesc.sampleCount = surface->msaaSamples;
	}
	surface->depthRB = [surface->device newTextureWithDescriptor:depthTexDesc];

	MTLTextureDescriptor* stencilTexDesc = [MTLTextureDescriptorClass() texture2DDescriptorWithPixelFormat:MTLPixelFormatStencil8 width:surface->targetW height:surface->targetH mipmapped:NO];
	if(surface->msaaSamples > 1)
	{
		stencilTexDesc.textureType = MTLTextureType2DMultisample;
		stencilTexDesc.sampleCount = surface->msaaSamples;
	}
	surface->stencilRB = [surface->device newTextureWithDescriptor:stencilTexDesc];
}

extern "C" void DestroySharedDepthbufferMTL(UnityDisplaySurfaceMTL* surface)
{
	surface->depthRB = nil;
	surface->stencilRB = nil;
}

extern "C" void CreateUnityRenderBuffersMTL(UnityDisplaySurfaceMTL* surface)
{
	UnityRenderBufferDesc sys_desc = { surface->systemW, surface->systemH, 1, 1, 1 };
	UnityRenderBufferDesc tgt_desc = { surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1 };

	// drawable (final color texture) we will be updating on every frame
	// in case of rendering to native + AA, we will also update native target every frame

	if(surface->targetAAColorRT)
		surface->unityColorBuffer	= UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetAAColorRT, surface->targetColorRT, &tgt_desc);
	else if(surface->targetColorRT)
		surface->unityColorBuffer	= UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetColorRT, nil, &tgt_desc);
	else
		surface->unityColorBuffer	= UnityCreateDummySurface(surface->unityColorBuffer, true, &sys_desc);

	surface->unityDepthBuffer		= UnityCreateExternalDepthSurfaceMTL(surface->unityDepthBuffer, surface->depthRB, surface->stencilRB, &tgt_desc);

	surface->systemColorBuffer = UnityCreateDummySurface(surface->systemColorBuffer, true, &sys_desc);
	surface->systemDepthBuffer = UnityCreateDummySurface(surface->systemDepthBuffer, false, &sys_desc);
}


extern "C" void DestroySystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
	surface->systemColorRB = nil;
}

extern "C" void DestroyUnityRenderBuffersMTL(UnityDisplaySurfaceMTL* surface)
{
	UnityDestroyExternalSurface(surface->unityColorBuffer);
	UnityDestroyExternalSurface(surface->systemColorBuffer);
	surface->unityColorBuffer = surface->systemColorBuffer = 0;

	UnityDestroyExternalSurface(surface->unityDepthBuffer);
	UnityDestroyExternalSurface(surface->systemDepthBuffer);
	surface->unityDepthBuffer = surface->systemDepthBuffer = 0;
}


extern "C" void UnityPrepareScreenshot()
{
	UnitySetRenderTarget(GetMainDisplaySurface()->unityColorBuffer, GetMainDisplaySurface()->unityDepthBuffer);
}

extern "C" void PreparePresentMTL(UnityDisplaySurfaceMTL* surface)
{
	if(surface->targetColorRT)
		UnityBlitToBackbuffer(surface->unityColorBuffer, surface->systemColorBuffer, surface->systemDepthBuffer);
	APP_CONTROLLER_RENDER_PLUGIN_METHOD(onFrameResolved);
}
extern "C" void PresentMTL(UnityDisplaySurfaceMTL* surface)
{
	if(surface->drawable)
		[UnityCurrentMTLCommandBuffer() presentDrawable:surface->drawable];
}

extern "C" void StartFrameRenderingMTL(UnityDisplaySurfaceMTL* surface)
{
	// in case of skipping present we want to nullify prev drawable explicitly to poke ARC
	surface->drawable		= nil;
	surface->drawable		= [surface->layer nextDrawable];

	// on A7 SoC nextDrawable may be nil before locking the screen
	if (!surface->drawable)
		return;

	surface->systemColorRB	= [surface->drawable texture];

	// screen disconnect notification comes asynchronously
	// even better when preparing render we might still have [UIScreen screens].count == 2, but drawable would be nil already
	if(surface->systemColorRB)
	{
		UnityRenderBufferDesc sys_desc = { surface->systemW, surface->systemH, 1, 1, 1};
		UnityRenderBufferDesc tgt_desc = { surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1};

		if(surface->targetColorRT)
			surface->systemColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->systemColorBuffer, surface->systemColorRB, nil, &sys_desc);
		else if(surface->targetAAColorRT)
			surface->unityColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetAAColorRT, surface->systemColorRB, &tgt_desc);
		else
			surface->unityColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->systemColorRB, nil, &tgt_desc);
	}
	else
	{
		UnityDisableRenderBuffers(surface->unityColorBuffer, surface->unityDepthBuffer);
	}
}
extern "C" void EndFrameRenderingMTL(UnityDisplaySurfaceMTL* surface)
{
	surface->systemColorRB	= nil;
	surface->drawable		= nil;
}

#else

extern "C" void InitRenderingMTL()											{}

extern "C" void CreateSystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)	{}
extern "C" void CreateRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)			{}
extern "C" void DestroyRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)			{}
extern "C" void CreateSharedDepthbufferMTL(UnityDisplaySurfaceMTL*)			{}
extern "C" void DestroySharedDepthbufferMTL(UnityDisplaySurfaceMTL*)		{}
extern "C" void CreateUnityRenderBuffersMTL(UnityDisplaySurfaceMTL*)		{}
extern "C" void DestroySystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)	{}
extern "C" void DestroyUnityRenderBuffersMTL(UnityDisplaySurfaceMTL*)		{}
extern "C" void StartFrameRenderingMTL(UnityDisplaySurfaceMTL*)				{}
extern "C" void EndFrameRenderingMTL(UnityDisplaySurfaceMTL*)				{}
extern "C" void PreparePresentMTL(UnityDisplaySurfaceMTL*)					{}
extern "C" void PresentMTL(UnityDisplaySurfaceMTL*)							{}

#endif
