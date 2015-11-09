using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class AppKey 
	{
		//配置ShareSDK AppKey
		public string appKey = "androidv1101";
	}

	[Serializable]
	public class DevInfoSet

	{
		public SinaWeiboDevInfo sinaweibo;
		public TencentWeiboDevInfo tencentweibo;
		public QQ qq;
		public QZone qzone;
		public Wechat wechat;
		public WechatMoments wechatMoments;
		public WechatFavorite wechatFavorite;
		public Facebook facebook;
		public Twitter twitter;
		public Email email;
		public ShortMessage shortMessage;
		public Douban douban;
		public Renren renren;
		public GooglePlus googlePlus;
		public KaiXin kaiXin;
		public Pocket pocket;
		public Instagram instagram;
		public LinkedIn linkedIn;
		public Tumblr tumblr;
		public YouDao youDao;
		public Flickr flickr;
		public Evernote evernote;
		public WhatsApp whatsApp;
		public Line line;

		#if UNITY_ANDROID
		//more platforms on iOS is coming soon ....
		public SohuSuishenkan sohukan;
		public FourSquare fourSquare;
		public Pinterest pinterest;
		public Dropbox dropbox;
		public VKontakte vkontakte;
		public Yixin yixin;
		public YixinMoments yixinMoments;
		public Mingdao mingdao;
		public KakaoTalk kakaoTalk;
		public KakaoStory kakaoStory;
		public Bluetooth bluetooth;
		public Instapaper instapaper;
		public FacebookMessenger facebookMessenger;
		#elif UNITY_IPHONE		
		public Copy copy;
		public EvernoteInternational evernoteInternational;
		#endif
	}

	public class DevInfo 
	{	
		public bool Enable = true;
	}

	[Serializable]
	public class SinaWeiboDevInfo : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.SinaWeibo;
		public string SortId = "1";
		public string AppKey = "568898243";
		public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string RedirectUrl = "http://www.sharesdk.cn";
		public string ShareByAppClient = "false";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SinaWeibo;
		public string app_key = "568898243";
		public string app_secret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string redirect_uri = "http://www.sharesdk.cn";
		public int auth_type = (int)AuthType.BOTH;
		#endif
	}

	[Serializable]
	public class TencentWeiboDevInfo : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.TencentWeibo;
		public string SortId = "2";
		public string AppKey = "801307650";
		public string AppSecret = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string RedirectUri = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.TencentWeibo;
		public string app_key = "801307650";
		public string app_secret = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string redirect_uri = "http://sharesdk.cn";
		#endif
	}
	
	[Serializable]
	public class QQ : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.QQ;
		public string SortId = "3";
		public string AppId = "100371282";
		public string AppKey = "aed9b0303e3ed1e27bae87c33761161d";
		public string ShareByAppClient = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "100371282";
		public string app_secret = "aed9b0303e3ed1e27bae87c33761161d";
		public int authType = (int)AuthType.SSO;
		#endif
	}

	[Serializable]
	public class QZone : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "4";
		public const int type = (int) PlatformType.QZone;
		public string AppId = "100371282";
		public string AppKey = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string ShareByAppClient = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QZone;
		public string app_id = "100371282";
		public string app_secret = "aed9b0303e3ed1e27bae87c33761161d";
		public int auth_type = (int)AuthType.SSO;
		#endif
	}

	[Serializable]
	public class Wechat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChatSession;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public string BypassApproval = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatSession;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class WechatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatTimeline;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public string BypassApproval = "false";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatTimeline;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class WechatFavorite : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int) PlatformType.WeChatFav;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFav;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

	[Serializable]
	public class Facebook : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "8";
		public const int type = (int) PlatformType.Facebook;
		public string ConsumerKey = "107704292745179";
		public string ConsumerSecret = "38053202e1a5fe26c80c753071f0b573";
		public string RedirectUrl = "http://mob.com/";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Facebook;
		public string app_key = "107704292745179";
		public string app_secret = "38053202e1a5fe26c80c753071f0b573";
		public int auth_type = (int)AuthType.SSO;
		#endif
	}

	[Serializable]
	public class Twitter : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "9";
		public const int type = (int) PlatformType.Twitter;
		public string ConsumerKey = "LRBM0H75rWrU9gNHvlEAA2aOy";
		public string ConsumerSecret = "gbeWsZvA9ELJSdoBzJ5oLKX0TU09UOwrzdGfo9Tg7DjyGuMe8G";
		public string CallbackUrl = "http://mob.com";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Twitter;
		public string app_key = "LRBM0H75rWrU9gNHvlEAA2aOy";
		public string app_secret = "gbeWsZvA9ELJSdoBzJ5oLKX0TU09UOwrzdGfo9Tg7DjyGuMe8G";
		public string redirect_uri = "http://mob.com";
		#endif
	}

	[Serializable]
	public class Renren : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "10";
		public const int type = (int) PlatformType.Renren;
		public string AppId = "226427";
		public string ApiKey = "fc5b8aed373c4c27a05b712acba0f8c3";
		public string SecretKey = "f29df781abdd4f49beca5a2194676ca4";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Renren;
		public string app_id = "226427";
		public string app_key = "fc5b8aed373c4c27a05b712acba0f8c3";
		public string app_secret = "f29df781abdd4f49beca5a2194676ca4";
		public int auth_type = (int) AuthType.BOTH;
		#endif
	}

	[Serializable]
	public class KaiXin : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "11";
		public const int type = (int) PlatformType.Kaixin;
		public string AppKey = "358443394194887cee81ff5890870c7c";
		public string AppSecret = "da32179d859c016169f66d90b6db2a23";
		public string RedirectUri = "http://www.sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Kaixin;
		public string api_key = "358443394194887cee81ff5890870c7c";
		public string app_secret = "da32179d859c016169f66d90b6db2a23";
		public string redirect_uri = "http://www.sharesdk.cn";
		#endif
	}

	[Serializable]
	public class Email : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "12";
		public const int type = (int) PlatformType.Mail;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Mail;
		#endif
	}

	[Serializable]
	public class ShortMessage : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "13";
		public const int type = (int) PlatformType.SMS;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SMS;
		#endif
	}

	[Serializable]
	public class Douban : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "14";
		public const int type = (int) PlatformType.DouBan;
		public string ApiKey = "02e2cbe5ca06de5908a863b15e149b0b";
		public string Secret = "9f1e7b4f71304f2f";
		public string RedirectUri = "http://www.sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.DouBan;
		public string api_key = "02e2cbe5ca06de5908a863b15e149b0b";
		public string app_secret = "9f1e7b4f71304f2f";
		public string redirect_uri = "http://www.sharesdk.cn";
		#endif
	}

	[Serializable]
	public class YouDao : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "15";
		public const int type = (int) PlatformType.YouDaoNote;
		public string HostType = "product";
		public string ConsumerKey = "dcde25dca105bcc36884ed4534dab940";
		public string ConsumerSecret = "d98217b4020e7f1874263795f44838fe";
		public string RedirectUri = "http://www.sharesdk.cn/";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YouDaoNote;
		public string consumer_key = "dcde25dca105bcc36884ed4534dab940";
		public string secret_key = "d98217b4020e7f1874263795f44838fe";
		public string callback_url = "http://www.sharesdk.cn/";
		#endif
	}

	[Serializable]
	public class SohuSuishenkan : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "16";
		public const int type = (int) PlatformType.SohuKan;
		public string AppKey = "e16680a815134504b746c86e08a19db0";
		public string AppSecret = "b8eec53707c3976efc91614dd16ef81c";
		public string RedirectUri = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SohuKan;
		public string AppKey = "e16680a815134504b746c86e08a19db0";
		public string AppSecret = "b8eec53707c3976efc91614dd16ef81c";
		public string RedirectUri = "http://sharesdk.cn";
		#endif
	}

	// 		安卓描述:   
	//		在中国大陆，印象笔记有两个服务器，一个是沙箱（sandbox），一个是生产服务器（china）。
	//		一般你注册应用，它会先让你使用sandbox，当你完成测试以后，可以到
	//		http://dev.yinxiang.com/support/上激活你的ConsumerKey，激活成功后，修改HostType
	//		为china就好了。至于如果您申请的是国际版的印象笔记（Evernote），则其生产服务器类型为
	//		“product”。
	//		如果目标设备上已经安装了印象笔记客户端，ShareSDK允许应用调用本地API来完成分享，但
	//		是需要将应用信息中的“ShareByAppClient”设置为true，此字段默认值为false。
	//      

	//      iOS描述:
	//		配置好consumerKey 和 secret, 如果为沙箱模式，请对参数isSandBox传入非0值，否则传入0.

	//在以下的配置里，安卓请任意选择 Evernote还是 EvernoteInternational都可以配置。
	//iOS则需要区分，国内版为Evernote，国际版EvernoteInternational。

	[Serializable]
	public class Evernote : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "17";
		public const int type = (int) PlatformType.Evernote;
		public enum HostType{sandbox = 1, china = 2, product = 3}
		public string ConsumerKey = "sharesdk-7807";
		public string ConsumerSecret = "d05bf86993836004";
		public string ShareByAppClient = "false";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Evernote;
		public string consumer_key = "sharesdk-7807";
		public string secret_key = "d05bf86993836004";
		public int isSandBox = 1; //"0" mean NO with SandBox, !0 mean YES with SandBox
		#endif
	}

	[Serializable]
	public class EvernoteInternational : DevInfo
	{
		#if UNITY_IPHONE
		public const int type = (int)PlatformType.EvernoteInternational;  
		public string consumer_key = "sharesdk-7807";
		public string secret_key = "d05bf86993836004";
		public int isSandBox = 0; //"0" mean NO with SandBox, !0 mean YES with SandBox
		#endif
	}


	[Serializable]
	public class LinkedIn : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "18";
		public const int type = (int) PlatformType.LinkedIn;
		public string ApiKey = "ejo5ibkye3vo";
		public string SecretKey = "cC7B2jpxITqPLZ5M";
		public string RedirectUrl = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.LinkedIn;
		public string api_key = "ejo5ibkye3vo";
		public string secret_key = "cC7B2jpxITqPLZ5M";
		public string redirect_uri = "http://sharesdk.cn";
		#endif
	}

	[Serializable]
	public class GooglePlus : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "19";
		public const int type = (int) PlatformType.GooglePlus;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.GooglePlus;
		public string client_id = "232554794995.apps.googleusercontent.com";
		public string client_secret = "PEdFgtrMw97aCvf0joQj7EMk";
		public string redirect_uri = "http://localhost";
		public int auth_type = (int)AuthType.BOTH;
		#endif
	}

	[Serializable]
	public class FourSquare : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "20";
		public const int type = (int) PlatformType.Foursquare;
		public string ClientID = "G0ZI20FM30SJAJTX2RIBGD05QV1NE2KVIM2SPXML2XUJNXEU";
		public string ClientSecret = "3XHQNSMMHIFBYOLWEPONNV4DOTCDBQH0AEMVGCBG0MZ32XNU";
		public string RedirectUrl = "http://www.sharesdk.cn";
		#elif UNITY_IPHONE
		//iOS do not support this platform
		#endif
	}

	[Serializable]
	public class Pinterest : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "21";
		public const int type = (int) PlatformType.Pinterest;
		public string ClientId = "1432928";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Pinterest;
		public string ClientId = "1432928";
		#endif
	}

	[Serializable]
	public class Flickr : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "22";
		public const int type = (int) PlatformType.Flickr;
		public string ApiKey = "33d833ee6b6fca49943363282dd313dd";
		public string ApiSecret = "3a2c5b42a8fbb8bb";
		public string RedirectUri = "http://www.sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Flickr;
		public string api_key = "33d833ee6b6fca49943363282dd313dd";
		public string api_secret = "3a2c5b42a8fbb8bb";
		#endif
	}

	[Serializable]
	public class Tumblr : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "23";
		public const int type = (int) PlatformType.Tumblr;
		public string OAuthConsumerKey = "2QUXqO9fcgGdtGG1FcvML6ZunIQzAEL8xY6hIaxdJnDti2DYwM";
		public string SecretKey = "3Rt0sPFj7u2g39mEVB3IBpOzKnM3JnTtxX2bao2JKk4VV1gtNo";
		public string CallbackUrl = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Tumblr;
		public string consumer_key = "2QUXqO9fcgGdtGG1FcvML6ZunIQzAEL8xY6hIaxdJnDti2DYwM";
		public string secret_key = "3Rt0sPFj7u2g39mEVB3IBpOzKnM3JnTtxX2bao2JKk4VV1gtNo";
		public string callback_url = "http://sharesdk.cn";
		#endif
	}

	[Serializable]
	public class Dropbox : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "24";
		public const int type = (int) PlatformType.Dropbox;
		public string AppKey = "7janx53ilz11gbs";
		public string AppSecret = "c1hpx5fz6tzkm32";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Dropbox;
		public string AppKey = "7janx53ilz11gbs";
		public string AppSecret = "c1hpx5fz6tzkm32";
		#endif
	}
	
	[Serializable]
	public class VKontakte : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "25";
		public const int type = (int) PlatformType.VKontakte;
		public string ApplicationId = "3921561";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.VKontakte;
		public string ApplicationId = "3921561";
		#endif
	}

	[Serializable]
	public class Instagram : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "26";
		public const int type = (int) PlatformType.Instagram;
		public string ClientId = "ff68e3216b4f4f989121aa1c2962d058";
		public string ClientSecret = "1b2e82f110264869b3505c3fe34e31a1";
		public string RedirectUri = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Instagram;
		public string client_id = "ff68e3216b4f4f989121aa1c2962d058";
		public string client_secret = "1b2e82f110264869b3505c3fe34e31a1";
		public string redirect_uri = "http://sharesdk.cn";
		#endif
	}

	//Yixin易信和YixinMoments易信朋友圈的appid是一样的；
	//注意：开发者不能用我们这两个平台的appid,否则分享不了
	//易信测试的时候需要先签名打包出apk,
	//sample测试易信，要先签名打包，keystore在sample项目中，密码123456
	//BypassApproval是绕过审核的标记，设置为true后AppId将被忽略，故不经过
	//审核的应用也可以执行分享，但是仅限于分享文字或图片，不能分享其他类型，
	//默认值为false。

	[Serializable]
	public class Yixin : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "27";
		public const int type = (int) PlatformType.YiXinSession;
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string BypassApproval = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YiXinSession;
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string BypassApproval = "true";
		#endif
	}

	[Serializable]
	public class YixinMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "28";
		public const int type = (int) PlatformType.YiXinTimeline;
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string BypassApproval = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YiXinTimeline;
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string BypassApproval = "true";
		#endif
	}

	[Serializable]
	public class Mingdao : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "29";
		public const int type = (int) PlatformType.MingDao;
		public string AppKey = "EEEE9578D1D431D3215D8C21BF5357E3";
		public string AppSecret = "5EDE59F37B3EFA8F65EEFB9976A4E933";
		public string RedirectUri = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.MingDao;
		public string AppKey = "EEEE9578D1D431D3215D8C21BF5357E3";
		public string AppSecret = "5EDE59F37B3EFA8F65EEFB9976A4E933";
		public string RedirectUri = "http://sharesdk.cn";
		#endif
	}

	[Serializable]
	public class Line : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "30";
		public const int type = (int) PlatformType.Line;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Line;
		#endif
	}
	
	[Serializable]
	public class KakaoTalk : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "31";
		public const int type = (int) PlatformType.KakaoTalk;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.KakaoTalk;
		#endif
	}
	
	[Serializable]
	public class KakaoStory : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "32";
		public const int type = (int) PlatformType.KakaoStory;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.KakaoStory;
		#endif
	}
	
	[Serializable]
	public class WhatsApp : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "33";
		public const int type = (int) PlatformType.WhatsApp;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WhatsApp;
		#endif
	}
	
	[Serializable]
	public class Bluetooth : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "34";
		public const int type = (int) PlatformType.Bluetooth;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Bluetooth;
		#endif
	}

	[Serializable]
	public class Pocket : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "35";
		public const int type = (int) PlatformType.Pocket;
		public string ConsumerKey = "32741-389c565043c49947ba7edf05";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Pocket;
		public string consumer_key = "11496-de7c8c5eb25b2c9fcdc2b627";
		public string redirect_uri = "pocketapp1234";
		public int auth_type = (int) AuthType.BOTH;
		#endif
	}

	[Serializable]
	public class Instapaper : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "36";
		public const int type = (int) PlatformType.Instapaper;
		public string ConsumerKey = "4rDJORmcOcSAZL1YpqGHRI605xUvrLbOhkJ07yO0wWrYrc61FA";
		public string ConsumerSecret = "GNr1GespOQbrm8nvd7rlUsyRQsIo3boIbMguAl9gfpdL0aKZWe";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Instapaper;
		public string consumer_key = "4rDJORmcOcSAZL1YpqGHRI605xUvrLbOhkJ07yO0wWrYrc61FA";
		public string ConsumerSecret = "GNr1GespOQbrm8nvd7rlUsyRQsIo3boIbMguAl9gfpdL0aKZWe";
		#endif
	}

	[Serializable]
	public class FacebookMessenger : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "37";
		public const int type = (int) PlatformType.FacebookMessenger;
		public string ConsumerKey = "107704292745179";
		public string ConsumerSecret = "38053202e1a5fe26c80c753071f0b573";
		public string RedirectUrl = "http://mob.com";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.FacebookMessenger;
		public string ConsumerKey = "107704292745179";
		public string ConsumerSecret = "38053202e1a5fe26c80c753071f0b573";
		public string RedirectUrl = "http://mob.com";
		#endif
	}

	[Serializable]
	public class Copy : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.Copy;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Copy;
		#endif
	}

}
