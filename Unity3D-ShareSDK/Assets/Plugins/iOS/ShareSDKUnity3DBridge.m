//
//  ShareSDKUnity3DBridge.m
//  Unity-iPhone
//
//  Created by 陈 剑东 on 15/7/28.
//  Copyright (c) 2015年 mob. All rights reserved.
//

#import "ShareSDKUnity3DBridge.h"
#import <ShareSDK/ShareSDK.h>
#import <ShareSDK/ShareSDK+Base.h>
#import <ShareSDKExtension/ShareSDK+Extension.h>
#import <ShareSDK/SSDKFriendsPaging.h>
#import <ShareSDKUI/ShareSDK+SSUI.h>
#import <ShareSDKExtension/SSEShareHelper.h>
#import <ShareSDKConnector/ShareSDKConnector.h>
#import <MOBFoundation/MOBFJson.h>
#import <MOBFoundation/MOBFRegex.h>
#import <MOBFoundation/MOBFDevice.h>

#define __SHARESDK_WECHAT__
#define __SHARESDK_QQ__
#define __SHARESDK_SINA_WEIBO__
#define __SHARESDK_GOOGLE_PLUS__
#define __SHARESDK_RENREN__


#ifdef __SHARESDK_WECHAT__
#import "WXApi.h"
#endif

#ifdef __SHARESDK_QQ__
#import <TencentOpenAPI/TencentOAuth.h>
#import <TencentOpenAPI/QQApiInterface.h>
#endif

#ifdef __SHARESDK_SINA_WEIBO__
#import "WeiboSDK.h"
#endif

#ifdef __SHARESDK_RENREN__
#import <RennSDK/RennSDK.h>
#endif

#ifdef __SHARESDK_GOOGLE_PLUS__
#import <GooglePlus/GooglePlus.h>
#endif
static UIView *_refView = nil;

#if defined (__cplusplus)
extern "C" {
#endif
    
    /**
     *	@brief	配置SDK并初始化
     *
     *	@param 	appKey      ShareSDK的AppKey
     *  @param  configInfo  配置信息
     */
    extern void __iosShareSDKRegisterAppAndSetPltformsConfig (void *appKey, void*configInfo);
    
    /**
     *  用户授权
     *
     *  @param reqID    流水号
     *  @param platType 平台类型
     *  @param observer 观察回调对象名称
     */
    extern void __iosShareSDKAuthorize (int reqID, int platType, void *observer);
    
    /**
     *	@brief	取消用户授权
     *
     *	@param 	platType 	平台类型
     */
    extern void __iosShareSDKCancelAuthorize (int platType);
    
    /**
     *	@brief	判断用户是否授权
     *
     *	@param 	platType 	平台类型
     *
     *	@return	YES 表示已经授权，NO 表示尚未授权
     */
    extern bool __iosShareSDKHasAuthorized (int platType);
    
    /**
     *	@brief	检测是否安装客户端
     *
     *	@param 	platType 	平台类型
     *
     *	@return	YES 表示已经安装，NO 表示尚未安装
     */
    extern bool __iosShareSDKIsClientInstalled (int platType);
    
    /**
     *	@brief	获取用户信息
     *
     *  @param  reqID       流水号
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKGetUserInfo (int reqID, int platType, void *observer);
    
    /**
     *	@brief	分享内容
     *
     *  @param  reqID       流水号
     *	@param 	platType 	平台类型
     *	@param 	content 	分享内容
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKShare (int reqID, int platType, void *content, void *observer);
    
    /**
     *	@brief	一键分享内容
     *
     *  @param  reqID       流水号
     *	@param 	platTypes 	平台类型列表
     *	@param 	content 	分享内容
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKOneKeyShare (int reqID, void *platTypes, void *content, void *observer);
    
    /**
     *	@brief	显示分享菜单
     *
     *  @param  reqID       流水号
     *	@param 	platTypes 	平台类型列表
     *	@param 	content 	分享内容
     *	@param 	x 	弹出菜单的箭头的横坐标，仅用于iPad
     *	@param 	y 	弹出菜单的箭头的纵坐标，仅用于iPad
     *	@param 	direction 	菜单箭头方向，仅用于iPad
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKShowShareMenu (int reqID, void *platTypes, void *content, int x, int y, void *observer);
    
    /**
     *	@brief	显示分享编辑界面
     *
     *  @param  reqID       流水号
     *	@param 	platType 	平台类型
     *	@param 	content 	分享内容
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKShowShareView (int reqID, int platType, void *content, void *observer);
    
    /**
     *	@brief	获取授权用户好友列表
     *
     *  @param  reqID       流水号
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    /**
     *  @brief 获取授权用户好友列表
     *
     *  @param reqID    流水号
     *  @param platType 平台类型
     *  @param count    单页好友数量
     *  @param page     页码/游标cusor
     *  @param observer 观察回调对象名称
     */
    extern void __iosShareSDKGetFriendsList (int reqID, int platType, int count , int page, void *observer);
    
    /**
     *	@brief	获取授权信息
     *
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern const char* __iosShareSDKGetCredential (int platType);
    
    /**
     *	@brief	关注/添加好友
     *
     *	@param 	platType 	平台类型
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareSDKFollowFriend (int reqID, int platType,void *account, void *observer);
    
    
#if defined (__cplusplus)
}
#endif


#if defined (__cplusplus)
extern "C" {
#endif
    
    NSMutableDictionary* __parseWithHashtable (void*configInfo)
    {
        NSString* confCs = [NSString stringWithCString:configInfo encoding:NSUTF8StringEncoding];
        NSMutableDictionary *dic = [NSMutableDictionary dictionaryWithDictionary:[MOBFJson objectFromJSONString:confCs]];
        return dic;
    }
    
    void __setWechatParams(NSDictionary* value,NSMutableDictionary* params,SSDKPlatformType subType)
    {
        NSString* text = nil;
        NSString* title = nil;
        NSString* url = nil;
        SSDKImage* thumbImg = nil;
        SSDKImage* image = nil;
        NSString* musicFileUrl = nil;
        NSString* extInfo = nil;
        NSData* fileData = nil;
        NSData* emoData = nil;
        SSDKContentType type = SSDKContentTypeText;
        
        if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]]) {
            text = [value objectForKey:@"content"];
        }
        if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
            title = [value objectForKey:@"title"];
        }
        if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]]) {
            url = [value objectForKey:@"url"];
        }
        if ([[value objectForKey:@"thumbImg"] isKindOfClass:[NSString class]]) {
            
            NSString* imgPath = [value objectForKey:@"thumbImg"];
            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
            {
                thumbImg = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
            }else
            {
                UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                thumbImg = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
            }
        }
        if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]]) {
            
            NSString* imgPath = [value objectForKey:@"image"];
            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
            {
                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
            }else
            {
                UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
            }
            
        }
        if ([[value objectForKey:@"musicFileUrl"] isKindOfClass:[NSString class]]) {
            musicFileUrl = [value objectForKey:@"musicFileUrl"];
        }
        if ([[value objectForKey:@"extInfo"] isKindOfClass:[NSString class]]) {
            extInfo = [value objectForKey:@"extInfo"];
        }
        if ([[value objectForKey:@"fileData"] isKindOfClass:[NSString class]]) {
            fileData = [NSData dataWithContentsOfFile:[value objectForKey:@"fileData"]];
        }
        if ([[value objectForKey:@"emoticonData"] isKindOfClass:[NSString class]]) {
            emoData = [NSData dataWithContentsOfFile:[value objectForKey:@"emoticonData"]];
        }
        if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]]) {
            type = [[value objectForKey:@"type"] integerValue];
        }
        [params SSDKSetupWeChatParamsByText:text
                                      title:title
                                        url:[NSURL URLWithString:url]
                                 thumbImage:thumbImg
                                      image:image
                               musicFileURL:[NSURL URLWithString:musicFileUrl]
                                    extInfo:extInfo
                                   fileData:fileData
                               emoticonData:emoData
                                       type:type
                         forPlatformSubType:subType];
        
    }

    void __setQQParams(NSDictionary* value,NSMutableDictionary* params,SSDKPlatformType subType)
    {
        NSString* text = nil;
        NSString* title = nil;
        NSString* url = nil;
        SSDKImage* thumbImg = nil;
        SSDKImage* image = nil;
        SSDKContentType type = SSDKContentTypeText;
        
        if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]]) {
            text = [value objectForKey:@"content"];
        }
        if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
            title = [value objectForKey:@"title"];
        }
        if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]]) {
            url = [value objectForKey:@"url"];
        }
        if ([[value objectForKey:@"thumbImg"] isKindOfClass:[NSString class]]) {
            
            NSString* imgPath = [value objectForKey:@"thumbImg"];
            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
            {
                thumbImg = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
            }else
            {
                UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                thumbImg = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
            }
        }
        if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]]) {
            
            NSString* imgPath = [value objectForKey:@"image"];
            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
            {
                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
            }else
            {
                UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
            }
            
        }
        if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]]) {
            type = [[value objectForKey:@"type"] integerValue];
        }
        [params SSDKSetupQQParamsByText:text
                                  title:title
                                    url:[NSURL URLWithString:url]
                             thumbImage:thumbImg
                                  image:image
                                   type:type
                     forPlatformSubType:subType];

    }
    
    NSMutableDictionary* __getShareParamsWithString(NSString* dataStr)
    {
        NSMutableDictionary* params = [NSMutableDictionary dictionary];
        
        NSMutableArray* imageArray = [NSMutableArray array];
        NSString *text = nil;
        NSString *title = nil;
        NSString *url = nil;
        SSDKContentType type = SSDKContentTypeText;
        
        NSDictionary* shareParamsDic = [MOBFJson objectFromJSONString:dataStr];
        
        if (shareParamsDic) {
            
            if ([[shareParamsDic objectForKey:@"content"] isKindOfClass:[NSString class]])
            {
                text = [shareParamsDic objectForKey:@"content"];
            }
            
            id img = [shareParamsDic objectForKey:@"image"];
            if ([img isKindOfClass:[NSString class]])
            {
                SSDKImage* image = nil;
                NSString* imgPath = img;
                if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                {
                    image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                }
                else
                {
                    UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                    image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                }
                
                [imageArray addObject:image];
              
                
            }
            else if([img isKindOfClass:[NSArray class]])
            {
                NSArray* paths = [img copy];
                for (NSString* path in paths)
                {
                    
                    SSDKImage* image = nil;
                    
                    if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:path])
                    {
                        image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:path]];
                    }
                    else
                    {
                        UIImage* localImg = [UIImage imageWithContentsOfFile:path];
                        image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                    }
                    
                    [imageArray addObject:image];
                    
                }
            }
            
            if ([[shareParamsDic objectForKey:@"title"] isKindOfClass:[NSString class]])
            {
                title = [shareParamsDic objectForKey:@"title"];
                
            }
            
            if ([[shareParamsDic objectForKey:@"url"] isKindOfClass:[NSString class]])
            {
                url = [shareParamsDic objectForKey:@"url"];
                
            }
            if ([[shareParamsDic objectForKey:@"type"] isKindOfClass:[NSString class]])
            {
                type = (SSDKContentType)[[shareParamsDic objectForKey:@"type"] integerValue];
            }
            
            
            [params SSDKSetupShareParamsByText:text
                                        images:imageArray
                                           url:[NSURL URLWithString:url]
                                         title:title type:type];
            
            
            
            if (shareParamsDic)
            {
                //新浪微博
                id value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeSinaWeibo]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    NSString* title = nil;
                    SSDKImage* image = nil;
                    NSString* url = nil;
                    double lat;
                    double lng;
                    NSString* objID = nil;
                    SSDKContentType type = SSDKContentTypeWebPage;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]])
                    {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        NSString* imgPath =  [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }
                        else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];

                        }

                    }
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]])
                    {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"lat"] isKindOfClass:[NSString class]])
                    {
                        lat = [[value objectForKey:@"lat"] doubleValue];
                    }
                    if ([[value objectForKey:@"lng"] isKindOfClass:[NSString class]])
                    {
                        lng = [[value objectForKey:@"lng"] doubleValue];
                    }
                    if ([[value objectForKey:@"objID"] isKindOfClass:[NSString class]])
                    {
                        objID = [value objectForKey:@"objID"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
 
                  [params SSDKSetupSinaWeiboShareParamsByText:text
                                                        title:title
                                                        image:image
                                                          url:[NSURL URLWithString:url]
                                                     latitude:lat
                                                    longitude:lng
                                                     objectID:objID
                                                         type:type];
                    
                }
                //腾讯微博
                value  = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeTencentWeibo]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    NSMutableArray* images = [NSMutableArray array];
                    double lat;
                    double lng;
                    SSDKContentType type = SSDKContentTypeImage;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        NSString* imgPath =  [value objectForKey:@"image"];
                        SSDKImage* image = nil;
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }
                        else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        [images addObject:image];
                        
                    }else if([[value objectForKey:@"image"] isKindOfClass:[NSArray class]])
                    {
                        
                        NSArray* paths = [value objectForKey:@"image"];
                        
                        for (NSString* path in paths)
                        {
                            
                            SSDKImage* image = nil;
                            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:path])
                            {
                                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:path]];
                            }
                            else
                            {
                                UIImage* localImg = [UIImage imageWithContentsOfFile:path];
                                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            }

                            [images addObject:image];
                            
                        }
                    }
                    
                    if ([[value objectForKey:@"lat"] isKindOfClass:[NSNumber class]])
                    {
                        lat = [[value objectForKey:@"lat"] doubleValue];
                    }
                    if ([[value objectForKey:@"lng"] isKindOfClass:[NSNumber class]])
                    {
                        lng = [[value objectForKey:@"lng"] doubleValue];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    
                    
                    [params SSDKSetupTencentWeiboShareParamsByText:text
                                                            images:images
                                                          latitude:lat
                                                         longitude:lng
                                                              type:type];
       
                }
                //豆瓣
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeDouBan]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    NSString* title = nil;
                    NSString* url = nil;
                    NSString* urlDesc = nil;
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]]) {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                       
                        }
                        
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]]) {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"urlDesc"] isKindOfClass:[NSString class]]) {
                        urlDesc = [value objectForKey:@"urlDesc"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]]) {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                   [params SSDKSetupDouBanParamsByText:text
                                                 image:image
                                                 title:title
                                                   url:[NSURL URLWithString:url]
                                               urlDesc:urlDesc
                                                  type:type];
                 
                }
                //QQ系列
                value  =  [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeQQFriend]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    __setQQParams(value,params,SSDKPlatformSubTypeQQFriend);
                    
                }
                value  =  [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeQZone]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    __setQQParams(value,params,SSDKPlatformSubTypeQZone);
                }
    
                //微信系列
                value  =  [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatSession]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    __setWechatParams(value,params,SSDKPlatformSubTypeWechatSession);
                }
                value  =  [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatTimeline]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    __setWechatParams(value,params,SSDKPlatformSubTypeWechatTimeline);
                }
                value  =  [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatFav]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    __setWechatParams(value,params,SSDKPlatformSubTypeWechatFav);
                }
                
                //人人网
                [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeRenren]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    NSString* url = nil;
                    NSString* albumId = nil;
                    SSDKContentType type = SSDKContentTypeImage;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]]) {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        
                    }
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]])
                    {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"albumId"] isKindOfClass:[NSString class]])
                    {
                        albumId = [value objectForKey:@"albumId"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]]) {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    [params SSDKSetupRenRenParamsByText:text
                                                  image:image
                                                    url:[NSURL URLWithString:url]
                                                albumId:albumId
                                                   type:type];
                }
                //开心网
                [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeKaixin]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                    }
                    SSDKContentType type = SSDKContentTypeText;
                    [params SSDKSetupKaiXinParamsByText:text
                                                  image:image
                                                   type:type];
                }
                //Facebook
                value  =  [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeFacebook]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    
                    [params SSDKSetupFacebookParamsByText:text image:image type:type];
                }
                //Twitter
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeTwitter]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    NSMutableArray* images = [NSMutableArray array];
                    double lat;
                    double lng;
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        NSString* imgPath =  [value objectForKey:@"image"];
                        SSDKImage* image = nil;
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }
                        else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        [images addObject:image];
                        
                    }else if([[value objectForKey:@"image"] isKindOfClass:[NSArray class]])
                    {
                        
                        NSArray* paths = [value objectForKey:@"image"];
                        
                        for (NSString* path in paths)
                        {
                            
                            SSDKImage* image = nil;
                            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:path])
                            {
                                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:path]];
                            }
                            else
                            {
                                UIImage* localImg = [UIImage imageWithContentsOfFile:path];
                                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            }
                            
                            [images addObject:image];
                            
                        }
                    }
                    
                    if ([[value objectForKey:@"lat"] isKindOfClass:[NSNumber class]])
                    {
                        lat = [[value objectForKey:@"lat"] doubleValue];
                    }
                    if ([[value objectForKey:@"lng"] isKindOfClass:[NSNumber class]])
                    {
                        lng = [[value objectForKey:@"lng"] doubleValue];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    [params SSDKSetupTwitterParamsByText:text images:images latitude:lat longitude:lng type:type];
                }
                //GooglePlus
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeGooglePlus]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    NSString* url = nil;
                    NSString* title = nil;
                    NSString* urlDesc = nil;
                    NSString* deepLinkId = nil;
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]])
                    {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"urlDesc"] isKindOfClass:[NSString class]])
                    {
                        urlDesc = [value objectForKey:@"urlDesc"];
                    }
                   
                    if ([[value objectForKey:@"deepLinkId"] isKindOfClass:[NSString class]])
                    {
                        deepLinkId = [value objectForKey:@"deepLinkId"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    [params SSDKSetupGooglePlusParamsByText:text
                                                      image:image
                                                        url:[NSURL URLWithString:url]
                                                      title:title
                                                    urlDesc:urlDesc
                                                 deepLinkId:deepLinkId
                                                       type:type];
                }
                //Mail
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeMail]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text  = nil;
                    NSString* title = nil;
                    NSMutableArray* images = [NSMutableArray array];
                    NSMutableArray* attachments = [NSMutableArray array];
                    NSMutableArray* recipients = [NSMutableArray array];
                    NSMutableArray* ccRecipients = [NSMutableArray array];
                    NSMutableArray* bccRecipients = [NSMutableArray array];
                    SSDKContentType type = SSDKContentTypeText;
                    
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]]) {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        NSString* imgPath =  [value objectForKey:@"image"];
                        SSDKImage* image = nil;
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }
                        else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        [images addObject:image];
                        
                    }else if([[value objectForKey:@"image"] isKindOfClass:[NSArray class]])
                    {
                        
                        NSArray* paths = [value objectForKey:@"image"];
                        
                        for (NSString* path in paths)
                        {
                            
                            SSDKImage* image = nil;
                            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:path])
                            {
                                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:path]];
                            }
                            else
                            {
                                UIImage* localImg = [UIImage imageWithContentsOfFile:path];
                                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            }
                            
                            [images addObject:image];
                            
                        }
                    }
                    
                    if ([[value objectForKey:@"attachmentsPaths"] isKindOfClass:[NSString class]])
                    {
                        NSData* attachementsData = [NSData dataWithContentsOfFile:[value objectForKey:@"attachmentsPath"]];
                        [attachments addObject:attachementsData];
                    }
                    else if ([[value objectForKey:@"attachmentsPaths"] isKindOfClass:[NSArray class]])
                    {
                        NSArray* paths = [value objectForKey:@"attachmentsPaths"];
                        for (NSString* path in paths)
                        {
                            
                            NSData* attachementsData = [NSData dataWithContentsOfFile:path];
                            [attachments addObject:attachementsData];
                        }
                    }
                    if ([[value objectForKey:@"recipients"] isKindOfClass:[NSString class]])
                    {
                        [recipients addObject:[value objectForKey:@"recipients"]];
                    }
                    else if ([[value objectForKey:@"recipients"] isKindOfClass:[NSArray class]])
                    {
                        NSArray* recipientsArray = [value objectForKey:@"recipients"];
                        for (NSString* recipient in recipientsArray)
                        {
                            [recipients addObject:recipient];
                        }
                    }
                    if ([[value objectForKey:@"ccRecipients"] isKindOfClass:[NSString class]])
                    {
                        [ccRecipients addObject:[value objectForKey:@"ccRecipients"]];
                    }
                    else if ([[value objectForKey:@"ccRecipients"] isKindOfClass:[NSArray class]])
                    {
                        NSArray* recipientsArray = [value objectForKey:@"ccRecipients"];
                        for (NSString* recipient in recipientsArray)
                        {
                            [ccRecipients addObject:recipient];
                        }
                    }
                    if ([[value objectForKey:@"bccRecipients"] isKindOfClass:[NSString class]])
                    {
                        [bccRecipients addObject:[value objectForKey:@"bccRecipients"]];
                    }
                    else if ([[value objectForKey:@"bccRecipients"] isKindOfClass:[NSArray class]])
                    {
                        NSArray* recipientsArray = [value objectForKey:@"bccRecipients"];
                        for (NSString* recipient in recipientsArray)
                        {
                            [bccRecipients addObject:recipient];
                        }
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    
                    [params SSDKSetupMailParamsByText:text
                                                title:title
                                               images:images
                                          attachments:attachments
                                           recipients:recipients
                                         ccRecipients:ccRecipients
                                        bccRecipients:bccRecipients
                                                 type:type];
                }
                //SMS
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeSMS]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text  = nil;
                    NSString* title = nil;
                    NSMutableArray* images = [NSMutableArray array];
                    NSMutableArray* attachments = [NSMutableArray array];
                    NSMutableArray* recipients = [NSMutableArray array];
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]]) {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        NSString* imgPath =  [value objectForKey:@"image"];
                        SSDKImage* image = nil;
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }
                        else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        [images addObject:image];
                        
                    }else if([[value objectForKey:@"image"] isKindOfClass:[NSArray class]])
                    {
                        
                        NSArray* paths = [value objectForKey:@"image"];
                        
                        for (NSString* path in paths)
                        {
                            
                            SSDKImage* image = nil;
                            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:path])
                            {
                                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:path]];
                            }
                            else
                            {
                                UIImage* localImg = [UIImage imageWithContentsOfFile:path];
                                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            }
                            
                            [images addObject:image];
                            
                        }
                    }
                    
                    if ([[value objectForKey:@"attachmentsPaths"] isKindOfClass:[NSString class]])
                    {
                        NSData* attachementsData = [NSData dataWithContentsOfFile:[value objectForKey:@"attachmentsPath"]];
                        [attachments addObject:attachementsData];
                    }
                    else if ([[value objectForKey:@"attachmentsPaths"] isKindOfClass:[NSArray class]])
                    {
                        NSArray* paths = [value objectForKey:@"attachmentsPaths"];
                        for (NSString* path in paths)
                        {
                            
                            NSData* attachementsData = [NSData dataWithContentsOfFile:path];
                            [attachments addObject:attachementsData];
                        }
                    }
                    if ([[value objectForKey:@"recipients"] isKindOfClass:[NSString class]])
                    {
                        [recipients addObject:[value objectForKey:@"recipients"]];
                    }
                    else if ([[value objectForKey:@"recipients"] isKindOfClass:[NSArray class]])
                    {
                        NSArray* recipientsArray = [value objectForKey:@"recipients"];
                        for (NSString* recipient in recipientsArray)
                        {
                            [recipients addObject:recipient];
                        }
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    
                     [params SSDKSetupSMSParamsByText:text
                                                title:title
                                               images:images
                                          attachments:attachments
                                           recipients:recipients
                                                 type:type];
                }
                //Copy
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeCopy]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    NSMutableArray* images = [NSMutableArray array];
                    NSString* url = nil;
                    SSDKContentType type = SSDKContentTypeImage;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        NSString* imgPath =  [value objectForKey:@"image"];
                        SSDKImage* image = nil;
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }
                        else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                        [images addObject:image];
                        
                    }else if([[value objectForKey:@"image"] isKindOfClass:[NSArray class]])
                    {
                        
                        NSArray* paths = [value objectForKey:@"image"];
                        
                        for (NSString* path in paths)
                        {
                            
                            SSDKImage* image = nil;
                            if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:path])
                            {
                                image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:path]];
                            }
                            else
                            {
                                UIImage* localImg = [UIImage imageWithContentsOfFile:path];
                                image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            }
                            
                            [images addObject:image];
                            
                        }
                    }
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]])
                    {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]])
                    {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                    [params SSDKSetupCopyParamsByText:text
                                               images:images
                                                  url:[NSURL URLWithString:url]
                                                 type:type];
                }
                //Pocket
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypePocket]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* url = nil;
                    NSString* title = nil;
                    NSString* tags = nil;
                    NSString* tweetId = nil;
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]])
                    {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]])
                    {
                        title = [value objectForKey:@"title"];
                    }
                    id tagValue = [value objectForKey:@"tags"];
                    if ([tagValue isKindOfClass:[NSString class]])
                    {
                        tags = tagValue;
                    }
                    else if ([tagValue isKindOfClass:[NSArray class]])
                    {
                        NSArray* tagsArr = tagValue;
                        tags = [tagsArr componentsJoinedByString:@","];
                    }
                    if ([[value objectForKey:@"tweetId"] isKindOfClass:[NSString class]])
                    {
                        tweetId = [value objectForKey:@"tweetId"];
                    }
                    [params SSDKSetupPocketParamsByUrl:[NSURL URLWithString:url]
                                                 title:title
                                                  tags:tags
                                               tweetId:tweetId];
                }
                //Instagram
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeInstagram]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    SSDKImage* image = nil;
                    CGFloat menuX;
                    CGFloat menuY;
                    
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]])
                    {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                        }
                    }
                    if ([[value objectForKey:@"menuX"] isKindOfClass:[NSString class]])
                    {
                        menuX = [[value objectForKey:@"menuX"] floatValue];
                    }
                    if ([[value objectForKey:@"menuY"] isKindOfClass:[NSString class]])
                    {
                        menuX = [[value objectForKey:@"menuY"] floatValue];
                    }
                    
                    CGPoint point = CGPointMake(menuX, menuY);
                    [params SSDKSetupInstagramByImage:image menuDisplayPoint:point];
                }
                //LinkedIn
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeLinkedIn]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    NSString* title = nil;
                    NSString* url = nil;
                    NSString* urlDesc = nil;
                    NSString* visibility = nil;
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]]) {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            
                        }
                        
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]]) {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"urlDesc"] isKindOfClass:[NSString class]]) {
                        urlDesc = [value objectForKey:@"urlDesc"];
                    }
                    if ([[value objectForKey:@"visibility"] isKindOfClass:[NSString class]]) {
                        visibility = [value objectForKey:@"visibility"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]]) {
                        type = [[value objectForKey:@"type"] integerValue];
                    }

                    [params SSDKSetupLinkedInParamsByText:text
                                                    image:image
                                                      url:[NSURL URLWithString:url]
                                                    title:title
                                                  urlDesc:urlDesc
                                               visibility:visibility
                                                     type:type];
                }
                //Tumblr
                value = [shareParamsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeTumblr]];
                if ([value isKindOfClass:[NSDictionary class]])
                {
                    NSString* text = nil;
                    SSDKImage* image = nil;
                    NSString* title = nil;
                    NSString* url = nil;
                    NSString* blogName = nil;
                    SSDKContentType type = SSDKContentTypeText;
                    
                    if ([[value objectForKey:@"content"] isKindOfClass:[NSString class]])
                    {
                        text = [value objectForKey:@"content"];
                    }
                    if ([[value objectForKey:@"image"] isKindOfClass:[NSString class]]) {
                        
                        NSString* imgPath = [value objectForKey:@"image"];
                        if ([MOBFRegex isMatchedByRegex:@"\\w://.*" options:MOBFRegexOptionsNoOptions inRange:NSMakeRange(0, 10) withString:imgPath])
                        {
                            image = [[SSDKImage alloc]initWithURL:[NSURL URLWithString:imgPath]];
                        }else
                        {
                            UIImage* localImg = [UIImage imageWithContentsOfFile:imgPath];
                            image = [[SSDKImage alloc]initWithImage:localImg format:SSDKImageFormatJpeg settings:nil];
                            
                        }
                        
                    }
                    if ([[value objectForKey:@"title"] isKindOfClass:[NSString class]]) {
                        title = [value objectForKey:@"title"];
                    }
                    if ([[value objectForKey:@"url"] isKindOfClass:[NSString class]]) {
                        url = [value objectForKey:@"url"];
                    }
                    if ([[value objectForKey:@"blogName"] isKindOfClass:[NSString class]]) {
                        blogName = [value objectForKey:@"blogName"];
                    }
                    if ([[value objectForKey:@"type"] isKindOfClass:[NSString class]]) {
                        type = [[value objectForKey:@"type"] integerValue];
                    }
                   [params SSDKSetupTumblrParamsByText:text
                                                 image:image
                                                   url:[NSURL URLWithString:url]
                                                 title:title
                                              blogName:blogName
                                                  type:type];
                }
                
            }
        }
        return params;
    }
    
    void __iosShareSDKRegisterAppAndSetPltformsConfig (void *appKey, void*configInfo)
    {
        NSString* appKeyStr = [NSString stringWithCString:appKey encoding:NSUTF8StringEncoding];

        NSMutableDictionary* platformsDic = [NSMutableDictionary dictionary];
        if (configInfo)
        {
            
            platformsDic = __parseWithHashtable(configInfo);

        }
        NSArray* activePlatforms = [platformsDic allKeys];
        
        [ShareSDK registerApp:appKeyStr activePlatforms:activePlatforms onImport:^(SSDKPlatformType platformType) {
            switch (platformType)
            {
                    
                case SSDKPlatformTypeSinaWeibo:
#ifdef __SHARESDK_SINA_WEIBO__
                    [ShareSDKConnector connectWeibo:[WeiboSDK class]];
#endif
                    break;
                    
                case SSDKPlatformTypeQQ:
#ifdef __SHARESDK_QQ__
                    [ShareSDKConnector connectQQ:[QQApiInterface class] tencentOAuthClass:[TencentOAuth class]];
#endif
                    break;
                    
                case SSDKPlatformTypeWechat:
#ifdef __SHARESDK_WECHAT__
                    [ShareSDKConnector connectWeChat:[WXApi class]];
#endif
                    break;
                case SSDKPlatformTypeRenren:
#ifdef __SHARESDK_RENREN__
                    [ShareSDKConnector connectRenren:[RennClient class]];
#endif
                    break;
                case SSDKPlatformTypeGooglePlus:
#ifdef __SHARESDK_GOOGLE_PLUS__
                    [ShareSDKConnector connectGooglePlus:[GPPSignIn class]
                                              shareClass:[GPPShare class]];
#endif
                    break;
                default:
                    break;
            }
        } onConfiguration:^(SSDKPlatformType platformType, NSMutableDictionary *appInfo) {
            
            NSString* appId = nil;
            NSString* appKey = nil;
            NSString* appSecret = nil;
            NSString* redirectUri = nil;
            NSString* authType = nil;
            NSMutableDictionary* confDic = [NSMutableDictionary dictionary];
            
            switch (platformType)
            {
                case SSDKPlatformTypeSinaWeibo:
                {
                    
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeSinaWeibo]];
                    appKey = [confDic objectForKey:@"app_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    NSInteger sinaAuthType = [[confDic objectForKey:@"auth_type"] integerValue];
                    switch (sinaAuthType)
                    {
                        case 1:
                            authType = SSDKAuthTypeBoth;
                            break;
                        case 2:
                            authType = SSDKAuthTypeSSO;
                            break;
                        case 3:
                            authType = SSDKAuthTypeWeb;
                            break;
                        default:
                            break;
                    }
                    [appInfo SSDKSetupSinaWeiboByAppKey:appKey
                                              appSecret:appSecret
                                            redirectUri:redirectUri
                                               authType:authType];
                    break;
                }
                case SSDKPlatformTypeTencentWeibo:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeTencentWeibo]];
                    appKey = [confDic objectForKey:@"app_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    [appInfo SSDKSetupTencentWeiboByAppKey:appKey
                                                 appSecret:appSecret
                                               redirectUri:redirectUri];
                    break;
                }
                case SSDKPlatformTypeDouBan:
                {
                    confDic =[platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeDouBan]];
                    appKey = [confDic objectForKey:@"api_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    
                    [appInfo SSDKSetupDouBanByApiKey:appKey
                                              secret:appSecret
                                         redirectUri:redirectUri];
                    break;
                }
                case SSDKPlatformTypeQQ:
                {
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeQQFriend]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeQQFriend]];
                        
                    }
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeQZone]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeQZone]];
                    }
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeQQ]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeQQ]];
                    }
                    appId = [confDic objectForKey:@"app_id"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    NSInteger qqAuthType = [[confDic objectForKey:@"auth_type"] integerValue];
                    switch (qqAuthType)
                {
                    case 1:
                        authType = SSDKAuthTypeBoth;
                        break;
                    case 2:
                        authType = SSDKAuthTypeSSO;
                        break;
                    case 3:
                        authType = SSDKAuthTypeWeb;
                        break;
                    default:
                        break;
                }
                    
                    [appInfo SSDKSetupQQByAppId:appId
                                         appKey:appSecret
                                       authType:authType];
                    break;
                }
                case SSDKPlatformTypeRenren:
                {
                    confDic =[platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeRenren]];
                    appId = [confDic objectForKey:@"app_id"];
                    appKey = [confDic objectForKey:@"app_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    NSInteger renrenAuthType = [[confDic objectForKey:@"auth_type"] integerValue];
                    switch (renrenAuthType)
                {
                    case 1:
                        authType = SSDKAuthTypeBoth;
                        break;
                    case 2:
                        authType = SSDKAuthTypeSSO;
                        break;
                    case 3:
                        authType = SSDKAuthTypeWeb;
                        break;
                    default:
                        break;
                }
                    [appInfo SSDKSetupRenRenByAppId:appId
                                             appKey:appKey
                                          secretKey:appSecret
                                           authType:authType];
                    break;
                }                    
                case SSDKPlatformTypeKaixin:
                {
                    confDic =[platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeKaixin]];
                    appKey = [confDic objectForKey:@"api_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    [appInfo SSDKSetupKaiXinByApiKey:appKey
                                           secretKey:appSecret
                                         redirectUri:redirectUri];
                    break;
                }
                case SSDKPlatformTypeFacebook:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeFacebook]];
                    appKey = [confDic objectForKey:@"app_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    NSInteger facebookAuthType = [[confDic objectForKey:@"auth_type"] integerValue];
                    switch (facebookAuthType)
                {
                    case 1:
                        authType = SSDKAuthTypeBoth;
                        break;
                    case 2:
                        authType = SSDKAuthTypeSSO;
                        break;
                    case 3:
                        authType = SSDKAuthTypeWeb;
                        break;
                    default:
                        break;
                }
                    [appInfo SSDKSetupFacebookByAppKey:appKey
                                             appSecret:appSecret
                                              authType:authType];
                    break;
                }
                case SSDKPlatformTypeTwitter:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeTwitter]];
                    appKey = [confDic objectForKey:@"app_key"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    [appInfo SSDKSetupTwitterByConsumerKey:appKey
                                            consumerSecret:appSecret
                                               redirectUri:redirectUri];
                    break;
                }
                case SSDKPlatformTypeGooglePlus:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeGooglePlus]];
                    appId = [confDic objectForKey:@"client_id"];
                    appKey = [confDic objectForKey:@"client_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    NSInteger googlePlusAuthType = [[confDic objectForKey:@"auth_type"] integerValue];
                    switch (googlePlusAuthType)
                {
                    case 1:
                        authType = SSDKAuthTypeBoth;
                        break;
                    case 2:
                        authType = SSDKAuthTypeSSO;
                        break;
                    case 3:
                        authType = SSDKAuthTypeWeb;
                        break;
                    default:
                        break;
                }
               
                    [appInfo SSDKSetupGooglePlusByClientID:appId
                                              clientSecret:appKey
                                               redirectUri:redirectUri
                                                  authType:authType];
                    break;
                }
                case SSDKPlatformTypeWechat:
                {
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatSession]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatSession]];
        
                    }
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatTimeline]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatTimeline]];
                    }
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatFav]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformSubTypeWechatFav]];
                    }
                    if ([activePlatforms containsObject:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeWechat]])
                    {
                        confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeWechat]];
                    }
                    appId = [confDic objectForKey:@"app_id"];
                    appSecret = [confDic objectForKey:@"app_secret"];
                    [appInfo SSDKSetupWeChatByAppId:appId
                                          appSecret:appSecret];
                    break;
                }
                case SSDKPlatformTypePocket:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypePocket]];
                    appKey = [confDic objectForKey:@"consumer_key"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    NSInteger pocketAuthType = [[confDic objectForKey:@"auth_type"] integerValue];
                    switch (pocketAuthType)
                    {
                        case 1:
                            authType = SSDKAuthTypeBoth;
                            break;
                        case 2:
                            authType = SSDKAuthTypeSSO;
                            break;
                        case 3:
                            authType = SSDKAuthTypeWeb;
                            break;
                        default:
                            break;
                    }
                    [appInfo SSDKSetupPocketByConsumerKey:appKey
                                              redirectUri:redirectUri
                                                 authType:authType];
                    break;
                }
                case SSDKPlatformTypeInstagram:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeInstagram]];
                    appId = [confDic objectForKey:@"client_id"];
                    appSecret = [confDic objectForKey:@"client_secret"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    [appInfo SSDKSetupInstagramByClientID:appId
                                             clientSecret:appSecret
                                              redirectUri:redirectUri];
                    break;
                }
                case SSDKPlatformTypeLinkedIn:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeLinkedIn]];
                    appKey = [confDic objectForKey:@"api_key"];
                    appSecret = [confDic objectForKey:@"secret_key"];
                    redirectUri = [confDic objectForKey:@"redirect_uri"];
                    [appInfo SSDKSetupLinkedInByApiKey:appKey
                                             secretKey:appSecret
                                           redirectUrl:redirectUri];
                    break;
                }
                case SSDKPlatformTypeTumblr:
                {
                    confDic = [platformsDic objectForKey:[NSString stringWithFormat:@"%lu",(unsigned long)SSDKPlatformTypeTumblr]];
                    appKey = [confDic objectForKey:@"consumer_key"];
                    appSecret = [confDic objectForKey:@"secret_key"];
                    redirectUri = [confDic objectForKey:@"callback_url"];
                    [appInfo SSDKSetupTumblrByConsumerKey:appKey
                                           consumerSecret:appSecret
                                              callbackUrl:redirectUri];
                    break;
                }
                //待续...
                default:
                    break;
            }
        }];
    }
    
    
    void __iosShareSDKAuthorize (int reqID, int platType, void *observer)
    {
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareSDK authorize:platType settings:nil onStateChanged:^(SSDKResponseState state, SSDKUser *user, NSError *error) {
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            [resultDict setObject:[NSNumber numberWithInteger:1] forKey:@"action"];
            [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
            [resultDict setObject:[NSNumber numberWithInteger:platType] forKey:@"platform"];
            [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];
            
            if (state == SSDKResponseStateFail && error)
            {
                NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                if ([[error userInfo] objectForKey:@"error_message"])
                {
                    if ([[error userInfo] objectForKey:@"error_message"])
                    {
                        [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];

                    }
                }
                else if ([[error userInfo] objectForKey:@"user_data"])
                {
                    NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                    if ([error_data objectForKey:@"error"])
                    {
                        [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                    }
                    if ([error_data objectForKey:@"error_code"])
                    {
                        [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                    }
                }
                
                [resultDict setObject:errorDict forKey:@"res"];

            }
            if (state == SSDKResponseStateSuccess)
            {
                if ([user rawData])
                {
                    [resultDict setObject:[user rawData] forKey:@"res"];
                }
            }
            
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
       
        }];
    }
    
    void __iosShareSDKCancelAuthorize (int platType)
    {
        [ShareSDK cancelAuthorize:platType];
    }
    
    bool __iosShareSDKHasAuthorized (int platType)
    {
        return [ShareSDK hasAuthorized:platType];
    }

    bool __iosShareSDKIsClientInstalled(int platType)
    {
        return [ShareSDK isClientInstalled:platType];
    }
    
    void __iosShareSDKGetUserInfo (int reqID, int platType, void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareSDK getUserInfo:platType
               onStateChanged:^(SSDKResponseState state, SSDKUser *user, NSError *error)
        {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            [resultDict setObject:[NSNumber numberWithInteger:8] forKey:@"action"];
            [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
            [resultDict setObject:[NSNumber numberWithInteger:platType] forKey:@"type"];
            [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];

            if (state == SSDKResponseStateFail && error)
            {
                NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                if ([[error userInfo] objectForKey:@"error_message"])
                {
                    if ([[error userInfo] objectForKey:@"error_message"])
                    {
                        [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];
                        
                    }
                }
                else if ([[error userInfo] objectForKey:@"user_data"])
                {
                    NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                    if ([error_data objectForKey:@"error"])
                    {
                        [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                    }
                    if ([error_data objectForKey:@"error_code"])
                    {
                        [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                    }
                }

                
                [resultDict setObject:errorDict forKey:@"res"];
            }
            if (state == SSDKResponseStateSuccess && user) {
                [resultDict setObject:[user rawData] forKey:@"res"];
            }
            
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
            
        }];
     }
    
    void __iosShareSDKShare (int reqID, int platType, void *content, void *observer)
    {
        NSString *observerStr = nil;
        NSMutableDictionary* shareParams = [NSMutableDictionary dictionary];
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            shareParams = __getShareParamsWithString(contentStr);
        }
        
        [ShareSDK share:SSDKPlatformTypeGooglePlus
             parameters:shareParams
         onStateChanged:^(SSDKResponseState state, NSDictionary *userData, SSDKContentEntity *contentEntity, NSError *error) {
             
             NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
             [resultDict setObject:[NSNumber numberWithInteger:9] forKey:@"action"];
             [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
             [resultDict setObject:[NSNumber numberWithInteger:platType] forKey:@"platform"];
             [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];
             
             if (state == SSDKResponseStateFail && error)
             {
                 NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                 [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                 if ([[error userInfo] objectForKey:@"error_message"])
                 {
                     if ([[error userInfo] objectForKey:@"error_message"])
                     {
                         [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];
                         
                     }
                 }
                 else if ([[error userInfo] objectForKey:@"user_data"])
                 {
                     NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                     if ([error_data objectForKey:@"error"])
                     {
                         [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                     }
                     if ([error_data objectForKey:@"error_code"])
                     {
                         [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                     }
                 }
                 
                 [resultDict setObject:errorDict forKey:@"res"];

             }

             if (state == SSDKResponseStateSuccess)
             {
                 if ([contentEntity rawData])
                 {
                     [resultDict setObject:[contentEntity rawData]  forKey:@"res"];
                 }
             }
             NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
          
             UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
             
             }];
        
    }

    void __iosShareSDKOneKeyShare (int reqID, void *platTypes, void *content, void *observer)
    {
        NSArray *platTypesArr = nil;
        NSString *observerStr = nil;
        NSMutableDictionary* shareParams = [NSMutableDictionary dictionary];

        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (platTypes)
        {
            NSString *platTypesStr = [NSString stringWithCString:platTypes encoding:NSUTF8StringEncoding];
            platTypesArr = [MOBFJson objectFromJSONString:platTypesStr];
        }
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            shareParams = __getShareParamsWithString(contentStr);
        }
        
        [SSEShareHelper oneKeyShare:platTypesArr
                         parameters:shareParams
                     onStateChanged:^(SSDKPlatformType platformType, SSDKResponseState state, NSDictionary *userData, SSDKContentEntity *contentEntity, NSError *error, BOOL end) {
                         
                         NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                         [resultDict setObject:[NSNumber numberWithInteger:9] forKey:@"action"];
                         [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
                         [resultDict setObject:[NSNumber numberWithInteger:platformType] forKey:@"platform"];
                         [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];
                         
                         if (state == SSDKResponseStateFail && error)
                         {
                             NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                             [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                             
                             
                             
                             
                             if ([[error userInfo] objectForKey:@"error_message"])
                             {
                                 if ([[error userInfo] objectForKey:@"error_message"])
                                 {
                                     [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];
                                     
                                 }
                             }
                             else if ([[error userInfo] objectForKey:@"user_data"])
                             {
                                 NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                                 if ([error_data objectForKey:@"error"])
                                 {
                                     [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                                 }
                                 if ([error_data objectForKey:@"error_code"])
                                 {
                                     [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                                 }
                             }
                             
                             [resultDict setObject:errorDict forKey:@"res"];
                         }
                         
                         if (state == SSDKResponseStateSuccess)
                         {
                             if ([contentEntity rawData])
                             {
                                 [resultDict setObject:[contentEntity rawData] forKey:@"res"];
                             }
                         }
                         
                         NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
                         UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
                   
                     }];
    }
    
    void __iosShareSDKShowShareMenu (int reqID, void *platTypes, void *content, int x, int y, void *observer)
    {
        
        NSArray *platTypesArr = nil;
        NSString *observerStr = nil;
        NSMutableDictionary* shareParams = [NSMutableDictionary dictionary];
        
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (platTypes)
        {
            NSString *platTypesStr = [NSString stringWithCString:platTypes encoding:NSUTF8StringEncoding];
            platTypesArr = [MOBFJson objectFromJSONString:platTypesStr];
        }
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            shareParams = __getShareParamsWithString(contentStr);
        }
        
        if ([MOBFDevice isPad])
        {
            if (!_refView)
            {
                _refView = [[UIView alloc] initWithFrame:CGRectMake(x, y, 10, 10)];
            }
            
            [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_refView];
         
        }
        
      
        
        [ShareSDK showShareActionSheet:_refView
                                 items:platTypesArr
                           shareParams:shareParams
                   onShareStateChanged:^(SSDKResponseState state, SSDKPlatformType platformType, NSDictionary *userData, SSDKContentEntity *contentEntity, NSError *error, BOOL end) {
                       
                       NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                       [resultDict setObject:[NSNumber numberWithInteger:9] forKey:@"action"];
                       [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
                       [resultDict setObject:[NSNumber numberWithInteger:platformType] forKey:@"platform"];
                       [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];
                       
                       if (state == SSDKResponseStateFail && error)
                       {
                           
                           NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                           [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                           if ([[error userInfo] objectForKey:@"error_message"])
                           {
                               if ([[error userInfo] objectForKey:@"error_message"])
                               {
                                   [errorDict setObject:[[error userInfo] objectForKey:@"error_message"]
                                                 forKey:@"error_msg"];
                                   
                               }
                           }
                           else if ([[error userInfo] objectForKey:@"user_data"])
                           {
                               NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                               if ([error_data objectForKey:@"error"])
                               {
                                   [errorDict setObject:[error_data objectForKey:@"error"]
                                                 forKey:@"error_msg"];
                               }
                               else if ([error_data objectForKey:@"error_message"])
                               {
                                   [errorDict setObject:[error_data objectForKey:@"error_message"]
                                                 forKey:@"error_msg"];
                               }
                               
                               if ([error_data objectForKey:@"error_code"])
                               {
                                   [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]]
                                                 forKey:@"error_code"];
                               }
                           }
                           
                           [resultDict setObject:errorDict forKey:@"res"];
                       }
                       
                       if (state == SSDKResponseStateSuccess)
                       {
                           if ([contentEntity rawData])
                           {
                               [resultDict setObject:[contentEntity rawData] forKey:@"res"];
                           }
                       }
                       
                       NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
                       UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
                       if (_refView)
                       {
                           //移除视图
                           [_refView removeFromSuperview];
                       }

                   }];
        
    }
    
    void __iosShareSDKShowShareView (int reqID, int platType, void *content, void *observer)
    {
        NSString *observerStr = nil;
        NSMutableDictionary* shareParams = [NSMutableDictionary dictionary];
        
        
        observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        
        if (content)
        {
            NSString *contentStr = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];
            shareParams = __getShareParamsWithString(contentStr);
        }

        
        [ShareSDK showShareEditor:platType
               otherPlatformTypes:nil
                      shareParams:shareParams
              onShareStateChanged:^(SSDKResponseState state, SSDKPlatformType platformType, NSDictionary *userData, SSDKContentEntity *contentEntity, NSError *error, BOOL end) {
                  NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                  [resultDict setObject:[NSNumber numberWithInteger:9] forKey:@"action"];
                  [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
                  [resultDict setObject:[NSNumber numberWithInteger:platformType] forKey:@"platform"];
                  [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];
                  
                  if (state == SSDKResponseStateFail && error)
                  {
                      NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                      [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                      if ([[error userInfo] objectForKey:@"error_message"])
                      {
                          if ([[error userInfo] objectForKey:@"error_message"])
                          {
                              [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];
                              
                          }
                      }
                      else if ([[error userInfo] objectForKey:@"user_data"])
                      {
                          NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                          if ([error_data objectForKey:@"error"])
                          {
                              [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                          }
                          if ([error_data objectForKey:@"error_code"])
                          {
                              [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                          }
                      }
                      
                      [resultDict setObject:errorDict forKey:@"res"];

                  }
                  
                  if (state == SSDKResponseStateSuccess) {
                      
                      if ([contentEntity rawData])
                      {
                          [resultDict setObject:[contentEntity rawData] forKey:@"res"];
                      }
                  }
                  
                  NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
                  UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);

              }];
    }
    
    void __iosShareSDKGetFriendsList (int reqID, int platType, int count , int page, void *observer)
    {
        
        SSDKPlatformType shareType = (SSDKPlatformType)platType;
        NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        NSInteger cursor = page;
        NSInteger size = count;
        
        if (shareType == SSDKPlatformTypeTwitter)
        {
            cursor = -1;
        }

        [ShareSDK getFriends:platType
                      cursor:cursor
                        size:size
              onStateChanged:^(SSDKResponseState state, SSDKFriendsPaging *paging, NSError *error)
         {
             NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
             [resultDict setObject:[NSNumber numberWithInteger:2] forKey:@"action"];
             [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
             [resultDict setObject:[NSNumber numberWithInteger:shareType] forKey:@"platform"];
             [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];

             if (state == SSDKResponseStateFail && error)
             {
                 NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                 [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                 if ([[error userInfo] objectForKey:@"error_message"])
                 {
                     if ([[error userInfo] objectForKey:@"error_message"])
                     {
                         [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];
                         
                     }
                 }
                 else if ([[error userInfo] objectForKey:@"user_data"])
                 {
                     NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                     if ([error_data objectForKey:@"error"])
                     {
                         [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                     }
                     if ([error_data objectForKey:@"error_code"])
                     {
                         [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                     }
                 }
                 
                 [resultDict setObject:errorDict forKey:@"res"];

             }

             if (state == SSDKResponseStateSuccess)
             {
                 if (paging)
                 {
                     NSArray *friends = [NSArray array];
                     friends = paging.users;
                     NSMutableDictionary* resDict = [NSMutableDictionary dictionary];
                     [resDict setObject:friends forKey:@"users"];
                     [resDict setObject:[NSNumber numberWithInteger:paging.prevCursor] forKey:@"prev_cursor"];
                     [resDict setObject:[NSNumber numberWithInteger:paging.nextCursor] forKey:@"next_cursor"];
                     [resDict setObject:[NSNumber numberWithUnsignedInteger:paging.total] forKey:@"total"];
                     [resDict setObject:[NSNumber numberWithBool:paging.hasNext] forKey:@"has_next"];
                     [resultDict setObject:resDict forKey:@"res"];

                 }
             }
             NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
             UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);
             
        }];
    
    }
    char* __StringCopy( const char* string)
    {
        if (string != NULL)
        {
            char* copyStr = (char*)malloc(strlen(string)+1);
            strcpy(copyStr, string);
            return copyStr;
        }
        else
        {
            return NULL;
        }
    }

    extern const char* __iosShareSDKGetCredential (int platType)
    {
        SSDKPlatformType shareType = (SSDKPlatformType)platType;
        SSDKUser* userInfo = [ShareSDK currentUser:shareType];
        SSDKCredential *credential = userInfo.credential;
        NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
        [resultDict setObject:[NSNumber numberWithInteger:shareType] forKey:@"type"];
        
        if ([credential available])
        {
            if ([credential uid])
            {
                [resultDict setObject:[credential uid] forKey:@"uid"];
            }
            if ([credential token])
            {
                [resultDict setObject:[credential token] forKey:@"token"];
            }
            if ([credential secret])
            {
                [resultDict setObject:[credential secret] forKey:@"secret"];
            }
            if ([credential expired])
            {
                [resultDict setObject:@([[credential expired] timeIntervalSince1970]) forKey:@"expired"];
            }
            
            [resultDict setObject:[NSNumber numberWithBool:[credential available]] forKey:@"available"];
      
        }else
        {
            [resultDict setObject:[NSNumber numberWithBool:NO] forKey:@"available"];
            [resultDict setObject:@"Invalid Authorization" forKey:@"error"];
        }
        
        NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
        return __StringCopy([resultStr UTF8String]);
    }

     void __iosShareSDKFollowFriend (int reqID, int platType,void *account, void *observer)
    {
        SSDKPlatformType shareType = (SSDKPlatformType)platType;
        NSString *observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        SSDKUser * user = [[SSDKUser alloc]init];
        user.uid =  [NSString stringWithCString:account encoding:NSUTF8StringEncoding];
        if (shareType == SSDKPlatformTypeTencentWeibo)
        {

            user.uid = nil;
            user.nickname = [NSString stringWithCString:account encoding:NSUTF8StringEncoding];
        }
        
        [ShareSDK addFriend:shareType user:user onStateChanged:^(SSDKResponseState state, SSDKUser *user, NSError *error) {
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            [resultDict setObject:[NSNumber numberWithInteger:6] forKey:@"action"];
            [resultDict setObject:[NSNumber numberWithInteger:state] forKey:@"status"];
            [resultDict setObject:[NSNumber numberWithInteger:shareType] forKey:@"platform"];
            [resultDict setObject:[NSNumber numberWithInteger:reqID] forKey:@"reqID"];
            
            if (state == SSDKResponseStateFail && error)
            {
                NSMutableDictionary* errorDict = [NSMutableDictionary dictionary];
                [errorDict setObject:[NSNumber numberWithInteger:[error code]] forKey:@"error_code"];
                if ([[error userInfo] objectForKey:@"error_message"])
                {
                    if ([[error userInfo] objectForKey:@"error_message"])
                    {
                        [errorDict setObject:[[error userInfo] objectForKey:@"error_message"] forKey:@"error_msg"];
                        
                    }
                }
                else if ([[error userInfo] objectForKey:@"user_data"])
                {
                    NSDictionary* error_data = [[error userInfo] objectForKey:@"user_data"];
                    if ([error_data objectForKey:@"error"])
                    {
                        [errorDict setObject:[error_data objectForKey:@"error"] forKey:@"error_msg"];
                    }
                    if ([error_data objectForKey:@"error_code"])
                    {
                        [errorDict setObject:[NSNumber numberWithInteger:[[error_data objectForKey:@"error_code"] integerValue]] forKey:@"error_code"];
                    }
                }

                
                [resultDict setObject:errorDict forKey:@"res"];

            }
            
            if (state == SSDKResponseStateSuccess)
            {
                NSDictionary *userRawdata = [NSDictionary dictionaryWithDictionary:[user rawData]];
                [resultDict setObject:userRawdata forKey:@"res"];
            }
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_Callback", [resultStr UTF8String]);

        }];
    }
    
    
    
#if defined (__cplusplus)
}
#endif
@implementation ShareSDKUnity3DBridge

@end
