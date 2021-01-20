using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class DevInfoSet
	{
		public SinaWeiboDevInfo sinaweibo;
		public TencentWeiboDevInfo tencentweibo;
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
		public Dropbox dropbox;
		public VKontakte vkontakte;
		public Pinterest pinterest;
		public Mingdao mingdao;
		public KakaoTalk kakaoTalk;
		public KakaoStory kakaoStory;
		public QQ qq;
		public QZone qzone;
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		public WeChatFavorites wechatFavorites;
		public Yixin yixin;
		public YixinMoments yixinMoments;
		public FacebookMessenger facebookMessenger;
		public Instapaper instapaper;
		public AliSocial aliSocial;
		public AliSocialMoments aliSocialMoments;
		public Dingding dingTalk;
		public Youtube youtube;
		public MeiPai meiPai;
        public CMCC cmcc;
		public Reddit reddit;
        public Telegram telegram;
        public ESurfing eSurfing; //中国电信
        public FacebookAccount facebookAccount;//iOS端无需配置
        public Douyin douyin; //抖音
		public WeWork wework; //企业微信
		public Oasis oasis; //绿洲
		public TikTok tiktok; //TikTok
		public KuaiShou kuaishou;
#if UNITY_ANDROID
		public FourSquare fourSquare;
		//安卓配置印象笔记国内与国际版直接在Evernote中配置
#elif UNITY_IPHONE
		public Copy copy;
		public YixinFavorites yixinFavorites;					//易信收藏，仅iOS端支持							[仅支持iOS端]
		public YixinSeries yixinSeries;							//iOS端易信系列, 可直接配置易信三个子平台			[仅支持iOS端]
		public WechatSeries wechatSeries;						//iOS端微信系列, 可直接配置微信三个子平台 		[仅支持iOS端]
		public QQSeries qqSeries;								//iOS端QQ系列,  可直接配置QQ系列两个子平台		[仅支持iOS端]
		public KakaoSeries kakaoSeries;                         //iOS端Kakao系列, 可直接配置Kakao系列两个子平台	[仅支持iOS端]
        public SnapChat snapChat;
        public EvernoteInternational evernoteInternational;		//iOS配置印象笔记国内版在Evernote中配置;国际版在EvernoteInternational中配置
		public Apple apple;//苹果
		public WatermelonVideo watermelonVideo;
		
#endif

    }

    [Serializable]
    public class DevInfo 
	{	
		public bool Enable = true;
	}

    [Serializable]
    public class SinaWeiboDevInfo : DevInfo 
	{
#if UNITY_ANDROID
		public const int type = (int) PlatformType.SinaWeibo;
		public string SortId = "4";
		public string AppKey = "568898243";
		public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string RedirectUrl = "http://www.sharesdk.cn";
		public bool ShareByAppClient = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SinaWeibo;
		public string app_key = "568898243";
		public string app_secret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string redirect_uri = "http://www.sharesdk.cn";
		public string app_universalLink = "https://bj2ks.share2dlink.com/";
//		public string auth_type = "both";	//can pass "both","sso",or "web"  
#endif
	}

	[Serializable]
    public class TencentWeiboDevInfo : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.TencentWeibo;
		public string SortId = "3";
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
		public string SortId = "2";
		public string AppId = "1110451818";
		public string AppKey = "aed9b0303e3ed1e27bae87c33761161d";
		public bool ShareByAppClient = true;

        //========================================================
        //when you test QQ miniprogram, you should use this params
        //At the same time, the package name and signature should 
        //correspond to the package name signature of the specific 
        //QQ sharing small program applied in the background of tencent
        //========================================================
        //public const int type = (int) PlatformType.QQ;
		//public string SortId = "2";
		//public string AppId = "222222";
		//public string AppKey = "aed9b0303e3ed1e27bae87c33761161d";
		//public bool ShareByAppClient = true;
        //========================================================
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "1110451818";
		public string app_key = "aed9b0303e3ed1e27bae87c33761161d";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
#endif
	}

	[Serializable]
	public class QZone : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "1";
		public const int type = (int) PlatformType.QZone;
		public string AppId = "1110451818";
		public string AppKey = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public bool ShareByAppClient = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QZone;
		public string app_id = "1110451818";
		public string app_key = "aed9b0303e3ed1e27bae87c33761161d";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#endif
	}
	

	
	[Serializable]
	public class WeChat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public string UserName = "gh_afb25ac019c9@app";
		public string Path = "/page/API/pages/share/share";
		public bool BypassApproval = false;
		public bool WithShareTicket = true;
		public string MiniprogramType = "0";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx617c77c82218ea2c";
        public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
        public string app_universalLink = "https://bj2ks.share2dlink.com/";
        #endif
    }

	[Serializable]
	public class WeChatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public bool BypassApproval = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
        public string app_universalLink = "https://bj2ks.share2dlink.com/";
        #endif
    }

	[Serializable]
	public class WeChatFavorites : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int) PlatformType.WeChatFavorites;
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFavorites;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
        public string app_universalLink = "https://bj2ks.share2dlink.com/";
#endif
	}

	[Serializable]
	public class Facebook : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "8";
		public const int type = (int) PlatformType.Facebook;
		public string ConsumerKey = "1412473428822331";
		public string ConsumerSecret = "a42f4f3f867dc947b9ed6020c2e93558";
		public string RedirectUrl = "https://www.baidu.com/";
		public string ShareByAppClient = "true";
				
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Facebook;
		public string api_key = "1412473428822331";
		public string app_secret = "38053202e1a5fe26c80c753071f0b573";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
		public string display_name = "ShareSDK";//如果需要使用客户端分享，必填且需与FB 后台配置一样
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
		public string consumer_key = "viOnkeLpHBKs6KXV7MPpeGyzE";
		public string consumer_secret = "NJEglQUy2rqZ9Io9FcAU9p17omFqbORknUpRrCDOK46aAbIiey";
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
		public string secret_key = "f29df781abdd4f49beca5a2194676ca4";
//		public string auth_type =  "both";  //can pass "both","sso",or "web" 
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
		public string secret_key = "da32179d859c016169f66d90b6db2a23";
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
		public string secret = "9f1e7b4f71304f2f";
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
		public string consumer_secret = "d98217b4020e7f1874263795f44838fe";
		public string oauth_callback = "http://www.sharesdk.cn/";
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

	//在以下的配置里，安卓请选择Evernote配置。
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
		public bool ShareByAppClient = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Evernote;
		public string consumer_key = "sharesdk-7807";
		public string consumer_secret = "d05bf86993836004";
		public int isSandBox = 1; //"0" mean NO with SandBox, !0 mean YES with SandBox
		#endif
	}

	[Serializable]
	public class EvernoteInternational : DevInfo
	{
		#if UNITY_ANDROID
		//ANDROID do not support this platform
		#elif UNITY_IPHONE
		public const int type = (int)PlatformType.EvernoteInternational;  
		public string consumer_key = "sharesdk-7807";
		public string consumer_secret = "d05bf86993836004";
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
		public string redirect_url = "http://sharesdk.cn";

		#endif
	}
    
    [Serializable]
	public class GooglePlus : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "19";
		public const int type = (int) PlatformType.GooglePlus;
		public string ClientID = "236300675100-am5pm8km7md1memjevq8rl9pg5c4s4b8.apps.googleusercontent.com";
		public string RedirectUrl = "http://localhost";
		public bool	ShareByAppClient = false;
		public string OfficialVersion = "default";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.GooglePlus;
		public string client_id = "232554794995.apps.googleusercontent.com";
		public string client_secret = "PEdFgtrMw97aCvf0joQj7EMk";
		public string redirect_uri = "http://localhost";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
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
		public string client_id = "4797078908495202393";
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
		public string consumer_secret = "3Rt0sPFj7u2g39mEVB3IBpOzKnM3JnTtxX2bao2JKk4VV1gtNo";
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
		public string app_key = "i5vw2mex1zcgjcj";
		public string app_secret = "3i9xifsgb4omr0s";
		public string oauth_callback = "https://www.sharesdk.cn";
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
		public string application_id = "3921561";
		public string secret_key = "6Qf883ukLDyz4OBepYF1";
//		public string auth_type = "both";
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
		public bool BypassApproval = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YiXinSession;
		public string app_id = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string app_secret = "1a5bd421ae089c3";
		public string redirect_uri = "https://open.yixin.im/resource/oauth2_callback.html";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
		#endif
	}

	[Serializable]
	public class YixinMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "28";
		public const int type = (int) PlatformType.YiXinTimeline;
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public bool BypassApproval = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YiXinTimeline;
		public string app_id = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string app_secret = "1a5bd421ae089c3";
		public string redirect_uri = "https://open.yixin.im/resource/oauth2_callback.html";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
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
		public string app_key = "EEEE9578D1D431D3215D8C21BF5357E3";
		public string app_secret = "5EDE59F37B3EFA8F65EEFB9976A4E933";
		public string redirect_uri = "http://sharesdk.cn";
		#endif
	}

	[Serializable]
	public class Line : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "30";
		public const int type = (int) PlatformType.Line;
		public string ChannelID = "1477692153";
		public string ChannelSecret = "f30c036370f2e04ade71c52eef73a9af";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Line;
        public string channel_id = "1639219273";
        public string app_universalLink = "https://ybpre.share2dlink.com/line-auth/";
        #endif
    }

	[Serializable]
	public class KakaoTalk : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "31";
		public const int type = (int) PlatformType.KakaoTalk;
		public string AppKey = "48d3f524e4a636b08d81b3ceb50f1003";
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.KakaoTalk;
		public string app_key = "9c17eb03317e0e627ec95a400f5785fb";
		public string rest_api_key = "802e551a5048c3172fc1dedaaf40fcf1";
		public string redirect_uri = "http://www.mob.com/oauth";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
#endif
	}

	[Serializable]
	public class KakaoStory : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "32";
		public const int type = (int) PlatformType.KakaoStory;
		public string AppKey = "48d3f524e4a636b08d81b3ceb50f1003";
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.KakaoStory;
		public string app_key = "9c17eb03317e0e627ec95a400f5785fb";
		public string rest_api_key = "802e551a5048c3172fc1dedaaf40fcf1";
		public string redirect_uri = "http://www.mob.com/oauth";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
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
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
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
		public string consumer_secret = "GNr1GespOQbrm8nvd7rlUsyRQsIo3boIbMguAl9gfpdL0aKZWe";
		#endif
	}

	[Serializable]
	public class FacebookMessenger : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "37";
		public const int type = (int) PlatformType.FacebookMessenger;
		public string AppId = "107704292745179";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.FacebookMessenger;
		public string api_key = "107704292745179";
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

	[Serializable]
	public class YixinSeries : DevInfo 
	{
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "Yixin" or class "YixinMoments" 
		//(Android do not support YixinFavorites)
			//对于安卓端，Yixin或YixinMoments中配置相关信息(安卓端不支持易信收藏YixinFavorites)
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YixinPlatform;
		public string app_id = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string app_secret = "1a5bd421ae089c3";
		public string redirect_uri = "https://open.yixin.im/resource/oauth2_callback.html";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
		#endif
	}

	[Serializable]
	public class YixinFavorites : DevInfo 
	{
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "Yixin" or class "YixinMoments" 
		//(Android do not support YixinFavorites)
		//对于安卓端，Yixin或YixinMoments中配置相关信息(安卓端不支持易信收藏YixinFavorites)
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.YiXinFav;
		public string app_id = "yx0d9a9f9088ea44d78680f3274da1765f";
		public string app_secret = "1a5bd421ae089c3";
		public string redirect_uri = "https://open.yixin.im/resource/oauth2_callback.html";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
		#endif
	}
		
	[Serializable]
	public class AliSocial : DevInfo
	{
		#if UNITY_ANDROID
		public string SortId = "50";
		public const int type = (int) PlatformType.AliSocial;
		public string AppId = "2015072400185895";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.AliSocial;
		public string app_id = "2015072400185895";
		#endif
	}

	[Serializable]
	public class AliSocialMoments : DevInfo
	{
		#if UNITY_ANDROID
		public string SortId = "51";
		public const int type = (int) PlatformType.AliSocialMoments;
		public string AppId = "2015072400185895";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.AliSocialMoments;
		public string app_id = "2015072400185895";
		#endif
	}

	[Serializable]
	public class Dingding : DevInfo
	{
		#if UNITY_ANDROID
		//安卓暂不支持,请留意更新
		public const int type = (int) PlatformType.Dingding;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Dingding;
		public string app_id = "dingoanxyrpiscaovl4qlw";
		#endif
	}

	[Serializable]
	public class WechatSeries : DevInfo 
	{	
		#if UNITY_ANDROID
		//for android,please set the configuraion in class "Wechat" ,class "WechatMoments" or class "WechatFavorite"
		//对于安卓端，请在类Wechat,WechatMoments或WechatFavorite中配置相关信息↑	
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WechatPlatform;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
        public string app_universalLink = "https://bj2ks.share2dlink.com/";
		#endif
	}

	[Serializable]
	public class QQSeries : DevInfo 
	{
#if UNITY_ANDROID
		//for android,please set the configuraion in class "QQ" and  class "QZone"
		//对于安卓端，请在类QQ或QZone中配置相关信息↑	
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQPlatform;
		public string app_id = "1110451818";
		public string app_key = "aed9b0303e3ed1e27bae87c33761161d";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
#endif
	}

	[Serializable]
	public class KakaoSeries : DevInfo 
	{
#if UNITY_ANDROID
		//for android,please set the configuraion in class "KakaoTalk" and  class "KakaoStory"
		//对于安卓端，请在类KakaoTalk或KakaoStory中配置相关信息
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.KakaoPlatform;
		public string app_key = "9c17eb03317e0e627ec95a400f5785fb";
		public string rest_api_key = "802e551a5048c3172fc1dedaaf40fcf1";
		public string redirect_uri = "http://www.mob.com/oauth";
//		public string auth_type = "both";   //can pass "both","sso",or "web" 
#endif
	}

	[Serializable]
	public class Youtube : DevInfo
	{
		#if UNITY_ANDROID
		public string SortId = "53";
		public const int type = (int) PlatformType.Youtube;
		public string ClientID = "370141748022-bicrnsjfiije93bvdt63dh3728m4shas.apps.googleusercontent.com";
		public string AppSecret = "AIzaSyAO06g-0TDpHcsXXO918a7QE3Zdct2bB5E";
		public string RedirectUrl="http://localhost";
		public string ShareByAppClient = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.Youtube;
		public string client_id = "906418427202-jinnbqal1niq4s8isbg2ofsqc5ddkcgr.apps.googleusercontent.com";
		public string client_secret = "";
		public string redirect_uri = "http://localhost";
		#endif
	}

	[Serializable]
	public class MeiPai : DevInfo
	{
		#if UNITY_ANDROID
		public string SortId = "54";
		public const int type = (int) PlatformType.MeiPai;
		public string ClientID = "1089867596";
		public string ShareByAppClient = "true";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.MeiPai;
		public string app_key = "1089867596";
		#endif
	}

    [Serializable]
    public class CMCC : DevInfo
    {
#if UNITY_ANDROID
        public string SortId = "55";
        public const int type = (int)PlatformType.CMCC;
        public string AppId = "300011860247";
        public string AppKey = "2D464D8BFCE73A44B4F9DF95A2FDBE1C";
#elif UNITY_IPHONE
        public string app_id = "300011936870";
        public string app_key = "610FC6F60177B9558C59B45C6FE47B9C";
        public int displayUI = 1; // 1 显示授权界面  0 不显示授权界面 
        public const int type = (int)PlatformType.CMCC;
#endif
	}

	[Serializable]
    public class Reddit : DevInfo
    {
#if UNITY_ANDROID
        public string SortId = "56";
        public const int type = (int)PlatformType.Reddit;
        public string Id = "56";
        public string AppKey = "MExDxPuTCtFiRw";
        public string RedirectUrl = "http://www.sharesdk.cn";
#elif UNITY_IPHONE
        public string app_key = "ObzXn50T7Cg0Xw";
        public string redirect_uri = "https://www.mob.com/reddit_callback";
        public const int type = (int)PlatformType.Reddit;
#endif
    }

    [Serializable]
    public class Telegram : DevInfo
    {
#if UNITY_ANDROID
        public string SortId = "47";
        public string AppKey = "782826033";
        public string RedirectUrl = "http://www.mob.com";
        public const int type = (int) PlatformType.Telegram;
#elif UNITY_IPHONE
        public string bot_token = "600852601:AAElp9J93JiYevLocDIEYPhEYulnMFuB_nQ";
		public string bot_domain = "http://127.0.0.1";
		public const int type = (int) PlatformType.Telegram;
#endif
    }

    [Serializable]
    public class ESurfing : DevInfo
    {
#if UNITY_ANDROID
        public string SortId = "57";
        public string AppKey = "8148612606";
        public string AppSecret = "mCltrhUqwshFa86egDTs0491ibaAulKA";
        public string RedirectUrl = "http://www.sharesdk.cn";
        public const int type = (int)PlatformType.ESurfing;
#elif UNITY_IPHONE
        public string app_key = "8252014408";
        public string app_secret = "bkqJOALOPc2i6V6R5mEjqLyuzrxF8rWD";
        public string app_name = "天天日记";
        public const int type = (int)PlatformType.ESurfing;
#endif
	}


	[Serializable]
    public class FacebookAccount : DevInfo
    {
#if UNITY_ANDROID
        public string SortId = "58";
		public string AppKey = "579465512480462";
		public string AppSecret = "8a6383652dd9f23fb0994f03d350d0ca";
		public string RedirectUrl = "http://www.sharesdk.cn/";
        public const int type = (int)PlatformType.FacebookAccount;
#elif UNITY_IPHONE
        public string app_id = "1412473428822331";
        //iOS平台 请到FacebookAccount.pltpds 设置AccountKitClientToken值 当前默认为测试
        public string client_token = "c30c08723aa8c48fbd5e01d1c3103891";
        public const int type = (int)PlatformType.FacebookAccount;
#endif
    }


    [Serializable]
    public class Douyin : DevInfo
    {
#if UNITY_ANDROID
       public string SortId = "59";
       public const int type = (int)PlatformType.Douyin;
       public string AppKey = "aw9ivykfjvi4hpwo";
       public string AppSecret = "42b4caa6bda60bd49f05f06d0a4956e1";
#elif UNITY_IPHONE
        public string app_key = "awycvl19mldccyso";
        public string app_secret = "8793a4dfdc3636cbda0924a3cfbc8424";
        public const int type = (int)PlatformType.Douyin;
#endif

	}

	[Serializable]	
	public class TikTok : DevInfo
	{
#if UNITY_ANDROID
		public const int type = (int)PlatformType.TikTok;
#elif UNITY_IPHONE
        public string app_key = "aw3vqar8qg1oy91q";
        public string app_secret = "18cf1714c53e9f9c64aec484ca4f2e29";
        public const int type = (int)PlatformType.TikTok;
#endif
	}


	[Serializable]
    public class WeWork : DevInfo
    {
#if UNITY_ANDROID
        public string SortId = "60";
        public const int type = (int)PlatformType.WeWork;
        public string AppKey = "wwa21eaecf93f0e3ba";                                    //对应企业id
        public string AppSecret = "dW7e27P7Hc8NiYdRxnbTeOLgfI1ugR72e-PM8uusq2s";
        public string AgentId = "1000012";                                              //对应企业内部id
        public string Schema = "wwautha21eaecf93f0e3ba000012";                          //对应企业微信SCHEMA
#elif UNITY_IPHONE
        public const int type = (int)PlatformType.WeWork;
        public string corp_id = "wwa21eaecf93f0e3ba";                                   //对应企业id
        public string app_secret = "dW7e27P7Hc8NiYdRxnbTeOLgfI1ugR72e-PM8uusq2s";
        public string agent_id = "1000012";                                             //对应企业内部id
        public string app_key = "wwautha21eaecf93f0e3ba000012";                         //对应企业微信SCHEMA
#endif
    }


	[Serializable]
	public class Oasis : DevInfo
	{
#if UNITY_ANDROID
		public string SortId = "64";
		public const int type = (int)PlatformType.Oasis;
		public string AppKey = "568898243";                                    //对应企业id
		public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string RedirectUrl = "http://www.sharesdk.cn"; 
#elif UNITY_IPHONE
        public string app_key = "568898243";
        public const int type = (int)PlatformType.Oasis;
#endif
	}

	[Serializable]
	public class KuaiShou : DevInfo
	{
#if UNITY_ANDROID
		public const int type = (int)PlatformType.KuaiShou;
#elif UNITY_IPHONE
		public const int type = (int)PlatformType.KuaiShou;
		public string app_id = "ks705657770555308030";
		public string app_secret = "RQ17enXUOioeoDMrwk3j2Q";
		public string app_universalLink = "https://bj2ks.share2dlink.com/";
#endif
	}

	[Serializable]
	public class SnapChat : DevInfo
	{
#if UNITY_ANDROID
		 
#elif UNITY_IPHONE
		public string client_id = "dbe54b15-1939-4bfc-b6a0-c30a4af426a6";
		public string redirect_uri = "ssdkmoba0b0c0d0://mob";
        public const int type = (int)PlatformType.SnapChat;
#endif
	}

#if UNITY_IPHONE
	[Serializable]
    public class Apple : DevInfo
    {
	    public const int type = (int) PlatformType.Apple;
    }
	
	[Serializable]
	public class WatermelonVideo : DevInfo
	{
		public const int type = (int)PlatformType.WatermelonVideo;
	}
#endif
	// 下列为闭环分享相关类
	[Serializable]
    public class RestoreSceneConfigure
    {
        public bool Enable = false;
#if UNITY_ANDROID

#elif UNITY_IPHONE
        public string capabilititesAssociatedDomain = "applinks:ahmn.t4m.cn";
        public string capabilititesEntitlementsPathInXcode = "Unity-iPhone/Base.entitlements";
#endif
    }


    public class RestoreSceneInfo
    {
        public string path;
        public Hashtable customParams;

        public RestoreSceneInfo(string scenePath, Hashtable sceneCustomParams)
        {
            try 
            {
                this.path = scenePath;
                this.customParams = sceneCustomParams;
            } catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e); 
            }
        }
    }


}
