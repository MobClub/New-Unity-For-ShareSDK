using System;
using System.Collections;
using UnityEngine;


namespace cn.sharesdk.unity3d.sdkporter
{
	[Serializable]
	public class ChosenPlatforms
	{
		public Hashtable SinaWeibo 		{set;get;}	
		public Hashtable TencentWeibo	{set;get;}	    
		public Hashtable DouBan			{set;get;}		   
		public Hashtable QQ				{set;get;}	     
		public Hashtable Renren			{set;get;}			
		public Hashtable Kaixin			{set;get;}				               
		public Hashtable Facebook		{set;get;}			     
		public Hashtable Twitter		{set;get;}			
		public Hashtable Evernote		{set;get;}			   
		public Hashtable GooglePlus		{set;get;}		
		public Hashtable Instagram		{set;get;}			
		public Hashtable LinkedIn		{set;get;}			
		public Hashtable Tumblr			{set;get;}			      
		public Hashtable Mail			{set;get;} 				    
		public Hashtable SMS			{set;get;}				
		public Hashtable Print			{set;get;} 			
		public Hashtable Copy			{set;get;}				
		public Hashtable WeChat			{set;get;}		  
		public Hashtable Instapaper		{set;get;}		
		public Hashtable Pocket			{set;get;}			
		public Hashtable YouDaoNote		{set;get;} 		 
		public Hashtable Pinterest		{set;get;} 		
		public Hashtable Flickr			{set;get;}			      
		public Hashtable Dropbox		{set;get;}			 
		public Hashtable VKontakte		{set;get;}			      
		public Hashtable YiXin			{set;get;} 		
		public Hashtable MingDao		{set;get;}          	
		public Hashtable Line			{set;get;}             	
		public Hashtable WhatsApp		{set;get;}         	
		public Hashtable Kakao			{set;get;}        
		public Hashtable FacebookMessenger{set;get;} 
		public Hashtable Alipay			{set; get;}            
		public Hashtable DingTalk		{set;get;}			
		public Hashtable Youtube		{set;get;}
		public Hashtable MeiPai			{set;get;}

		public ChosenPlatforms()
		{	
			this.SinaWeibo = new Hashtable ();
			this.SinaWeibo ["chosen"] = true;
			this.SinaWeibo ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/SinaWeiboSDK";
			this.SinaWeibo ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/SinaWeiboConnector.framework";
			this.SinaWeibo ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/SinaWeibo.js";

			this.TencentWeibo = new Hashtable ();
			this.TencentWeibo ["chosen"] = true;
			this.TencentWeibo ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/TencentWeibo.js";

			this.DouBan = new Hashtable ();
			this.DouBan ["chosen"] = true;
			this.DouBan ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/DouBan.js";

			this.QQ = new Hashtable ();
			this.QQ ["chosen"] = true;
			this.QQ ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/QQSDK";
			this.QQ ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/QQConnector.framework";
			this.QQ ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/QQ.js";

			this.Renren = new Hashtable ();
			this.Renren ["chosen"] = true;
			this.Renren ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/RenRenSDK";
			this.Renren ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/RenrenConnector.framework";
			this.Renren ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/RenRen.js";

			this.Kaixin = new Hashtable ();
			this.Kaixin ["chosen"] = true;
			this.Kaixin ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/KaiXin.js";

			this.Facebook = new Hashtable ();
			this.Facebook ["chosen"] = true;
			this.Facebook ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/FacebookConnector.framework";
			this.Facebook ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Facebook.js";

			this.Twitter = new Hashtable ();
			this.Twitter ["chosen"] = true;
			this.Twitter ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Twitter.js";

			this.Evernote = new Hashtable ();
			this.Evernote ["chosen"] = true;
			this.Evernote ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/EvernoteConnector.framework";
			this.Evernote ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Evernote.js";

			this.GooglePlus = new Hashtable ();
			this.GooglePlus ["chosen"] = true;
			this.GooglePlus ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/GooglePlusConnector.framework";
			this.GooglePlus ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/GooglePlus.js";

			this.Instagram = new Hashtable ();
			this.Instagram ["chosen"] = true;
			this.Instagram ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/InstagramConnector.framework";
			this.Instagram ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Instagram.js";

			this.LinkedIn = new Hashtable ();
			this.LinkedIn ["chosen"] = true;
			this.LinkedIn ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/LinkedIn.js";

			this.Tumblr = new Hashtable ();
			this.Tumblr ["chosen"] = true;
			this.Tumblr ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Tumblr.js";

			this.Mail = new Hashtable ();
			this.Mail ["chosen"] = true;
			this.Mail ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/MailConnector.framework";
			this.Mail ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Mail.js";

			this.SMS = new Hashtable ();
			this.SMS ["chosen"] = true;
			this.SMS ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/SMSConnector.framework";
			this.SMS ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/SMS.js";

			this.Print = new Hashtable ();
			this.Print ["chosen"] = true;
			this.Print ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/PrintConnector.framework";
			this.Print ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Print.js";

			this.Copy = new Hashtable ();
			this.Copy ["chosen"] = true;
			this.Copy ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/CopyConnector.framework";
			this.Copy ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Copy.js";

			this.WeChat = new Hashtable ();
			this.WeChat ["chosen"] = true;
			this.WeChat ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/WeChatSDK";
			this.WeChat ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/WechatConnector.framework";
			this.WeChat ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/WeChat.js";

			this.Instapaper = new Hashtable ();
			this.Instapaper ["chosen"] = true;
			this.Instapaper ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/InstapaperConnector.framework";
			this.Instapaper ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Instapaper.js";

			this.Pocket = new Hashtable ();
			this.Pocket ["chosen"] = true;
			this.Pocket ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Pocket.js";

			this.YouDaoNote = new Hashtable ();
			this.YouDaoNote ["chosen"] = true;
			this.YouDaoNote ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/YouDaoNote.js";

			this.Pinterest = new Hashtable ();
			this.Pinterest ["chosen"] = true;
			this.Pinterest ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Pinterest.js";

			this.Flickr = new Hashtable ();
			this.Flickr ["chosen"] = true;
			this.Flickr ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Flickr.js";

			this.Dropbox = new Hashtable ();
			this.Dropbox ["chosen"] = true;
			this.Dropbox ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Dropbox.js";

			this.VKontakte = new Hashtable ();
			this.VKontakte ["chosen"] = true;
			this.VKontakte ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/VKontakte.js";

			this.YiXin = new Hashtable ();
			this.YiXin ["chosen"] = true;
			this.YiXin ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/YiXinSDK";
			this.YiXin ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/YiXinConnector.framework";
			this.YiXin ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/YiXin.js";

			this.MingDao = new Hashtable ();
			this.MingDao ["chosen"] = true;
			this.MingDao ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/MingDao.js";

			this.Line = new Hashtable ();
			this.Line ["chosen"] = true;
			this.Line ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/LineConnector.framework";
			this.Line ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/Line.js";

			this.WhatsApp = new Hashtable ();
			this.WhatsApp ["chosen"] = true;
			this.WhatsApp ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/WhatsAppConnector.framework";
			this.WhatsApp ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/WhatsApp.js";

			this.Kakao = new Hashtable ();
			this.Kakao ["chosen"] = true;
			this.Kakao ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/KaKaoSDK";
			this.Kakao ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/KakaoConnector.framework";
			this.Kakao ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/KaKao.js";

			this.FacebookMessenger = new Hashtable ();
			this.FacebookMessenger ["chosen"] = true;
			this.FacebookMessenger ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/FacebookMessengerSDK";
			this.FacebookMessenger ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/FacebookConnector.framework";
			this.FacebookMessenger ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/FacebookMessenger.js";

			this.Alipay = new Hashtable ();
			this.Alipay ["chosen"] = true;
			this.Alipay ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/APSocialSDK";
			this.Alipay ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/AliPayConnector.framework";
			this.Alipay ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/AliPaySocial.js";

			this.DingTalk = new Hashtable ();
			this.DingTalk ["chosen"] = true;
			this.DingTalk ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/DingTalkSDK";
			this.DingTalk ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/DingTalkConnector.framework";
			this.DingTalk ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/DingTalk.js";

			this.Youtube = new Hashtable ();
			this.Youtube ["chosen"] = true;
			this.Youtube ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/YouTubeConnector.framework";
			this.Youtube ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/YouTube.js";

			this.MeiPai = new Hashtable ();
			this.MeiPai ["chosen"] = true;
			this.MeiPai ["sdkPath"] = "/ShareSDK/Support/PlatformSDK/MPShareSDK";
			this.MeiPai ["connectorPath"] = "/ShareSDK/Support/PlatformConnector/MeiPaiConnector.framework";
			this.MeiPai ["jsPath"] = "/ShareSDK/Support/Required/ShareSDK.bundle/ScriptCore/platforms/MeiPai.js";

		}
	}

}
	
