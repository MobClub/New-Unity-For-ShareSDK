using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace cn.sharesdk.unity3d.sdkporter
{
	[CustomEditor(typeof(ManagePlatforms))]
	[ExecuteInEditMode]
	public class EditorUI : Editor 
	{
		private ChosenPlatforms chosenPlats;

		void Awake()
		{
			Prepare ();
		}
			
		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space ();
			GUIStyle style = new GUIStyle ();
			style.wordWrap = true;
			EditorGUILayout.LabelField ("挂载ManagePlatforms,可用于管理是否导入相应平台的库。例如不需要新浪微博,那么可以不勾选新浪微博,在自动打包的Xcode项目中会自动去除掉新浪微博相关的库,以减少包体积及不必要的文件",style);
			EditorGUILayout.Space ();

			Boolean isSinaWeiboChosen = (Boolean)this.chosenPlats.SinaWeibo["chosen"];
			isSinaWeiboChosen = EditorGUILayout.Toggle ("SinaWeibo", isSinaWeiboChosen);
			this.chosenPlats.SinaWeibo["chosen"] = isSinaWeiboChosen;

			Boolean isTecnentChosen = (Boolean)this.chosenPlats.TencentWeibo["chosen"];
			isTecnentChosen = EditorGUILayout.Toggle ("TencentWeibo", isTecnentChosen);
			this.chosenPlats.TencentWeibo["chosen"] = isTecnentChosen;

			Boolean isDouBanChosen = (Boolean)this.chosenPlats.DouBan["chosen"];
			isDouBanChosen = EditorGUILayout.Toggle ("DouBan", isDouBanChosen);
			this.chosenPlats.DouBan["chosen"] = isDouBanChosen;

			Boolean isQQChosen = (Boolean)this.chosenPlats.QQ["chosen"];
			isQQChosen = EditorGUILayout.Toggle ("QQ", isQQChosen);
			this.chosenPlats.QQ["chosen"] = isQQChosen;

			Boolean isWeChatChosen = (Boolean)this.chosenPlats.WeChat["chosen"];
			isWeChatChosen = EditorGUILayout.Toggle ("WeChat", isWeChatChosen);
			this.chosenPlats.WeChat["chosen"] = isWeChatChosen;

			Boolean isRenrenChosen = (Boolean)this.chosenPlats.Renren["chosen"];
			isRenrenChosen = EditorGUILayout.Toggle ("Renren", isRenrenChosen);
			this.chosenPlats.Renren["chosen"] = isRenrenChosen;

			Boolean isKaixinChosen = (Boolean)this.chosenPlats.Kaixin["chosen"];
			isKaixinChosen = EditorGUILayout.Toggle ("Kaixin", isKaixinChosen);
			this.chosenPlats.Kaixin["chosen"] = isKaixinChosen;

			Boolean isFacebookChosen = (Boolean)this.chosenPlats.Facebook["chosen"];
			isFacebookChosen = EditorGUILayout.Toggle ("Facebook", isFacebookChosen);
			this.chosenPlats.Facebook["chosen"] = isFacebookChosen;

			Boolean isTwitterChosen = (Boolean)this.chosenPlats.Twitter["chosen"];
			isTwitterChosen = EditorGUILayout.Toggle ("Twitter", isTwitterChosen);
			this.chosenPlats.Twitter["chosen"] = isTwitterChosen;

			Boolean isEvernoteChosen = (Boolean)this.chosenPlats.Evernote["chosen"];
			isEvernoteChosen = EditorGUILayout.Toggle ("Evernote", isEvernoteChosen);
			this.chosenPlats.Evernote["chosen"] = isEvernoteChosen;

			Boolean isGooglePlusChosen = (Boolean)this.chosenPlats.GooglePlus["chosen"];
			isGooglePlusChosen = EditorGUILayout.Toggle ("GooglePlus", isGooglePlusChosen);
			this.chosenPlats.GooglePlus["chosen"] = isGooglePlusChosen;

			Boolean isInstagramChosen = (Boolean)this.chosenPlats.Instagram["chosen"];
			isInstagramChosen = EditorGUILayout.Toggle ("Instagram", isInstagramChosen);
			this.chosenPlats.Instagram["chosen"] = isInstagramChosen;

			Boolean isLinkedInChosen = (Boolean)this.chosenPlats.LinkedIn["chosen"];
			isLinkedInChosen = EditorGUILayout.Toggle ("LinkedIn", isLinkedInChosen);
			this.chosenPlats.LinkedIn["chosen"] = isLinkedInChosen;

			Boolean isTumblrChosen = (Boolean)this.chosenPlats.Tumblr["chosen"];
			isTumblrChosen = EditorGUILayout.Toggle ("Tumblr", isTumblrChosen);
			this.chosenPlats.Tumblr["chosen"] = isTumblrChosen;

			Boolean isMailChosen = (Boolean)this.chosenPlats.Mail["chosen"];
			isMailChosen = EditorGUILayout.Toggle ("Mail", isMailChosen);
			this.chosenPlats.Mail["chosen"] = isMailChosen;

			Boolean isSMSChosen = (Boolean)this.chosenPlats.SMS["chosen"];
			isSMSChosen = EditorGUILayout.Toggle ("SMS", isSMSChosen);
			this.chosenPlats.SMS["chosen"] = isSMSChosen;

			Boolean isPrintChosen = (Boolean)this.chosenPlats.Print["chosen"];
			isPrintChosen = EditorGUILayout.Toggle ("Print", isPrintChosen);
			this.chosenPlats.Print["chosen"] = isPrintChosen;

			Boolean isCopyChosen = (Boolean)this.chosenPlats.Copy["chosen"];
			isCopyChosen = EditorGUILayout.Toggle ("Copy", isCopyChosen);
			this.chosenPlats.Copy["chosen"] = isCopyChosen;

			Boolean isInstapaperChosen = (Boolean)this.chosenPlats.Instapaper["chosen"];
			isInstapaperChosen = EditorGUILayout.Toggle ("Instapaper", isInstapaperChosen);
			this.chosenPlats.Instapaper["chosen"] = isInstapaperChosen;

			Boolean isPocketChosen = (Boolean)this.chosenPlats.Pocket["chosen"];
			isPocketChosen = EditorGUILayout.Toggle ("Pocket", isPocketChosen);
			this.chosenPlats.Pocket["chosen"] = isPocketChosen;

			Boolean isYouDaoNoteChosen = (Boolean)this.chosenPlats.YouDaoNote["chosen"];
			isYouDaoNoteChosen = EditorGUILayout.Toggle ("YouDaoNote", isYouDaoNoteChosen);
			this.chosenPlats.YouDaoNote["chosen"] = isYouDaoNoteChosen;

			Boolean isPinterestChosen = (Boolean)this.chosenPlats.Pinterest["chosen"];
			isPinterestChosen = EditorGUILayout.Toggle ("Pinterest", isPinterestChosen);
			this.chosenPlats.Pinterest["chosen"] = isPinterestChosen;

			Boolean isFlickrChosen = (Boolean)this.chosenPlats.Flickr["chosen"];
			isFlickrChosen = EditorGUILayout.Toggle ("Flickr", isFlickrChosen);
			this.chosenPlats.Flickr["chosen"] = isFlickrChosen;

			Boolean isDropboxChosen = (Boolean)this.chosenPlats.Dropbox["chosen"];
			isDropboxChosen = EditorGUILayout.Toggle ("Dropbox", isDropboxChosen);
			this.chosenPlats.Dropbox["chosen"] = isDropboxChosen;

			Boolean isVKontakteChosen = (Boolean)this.chosenPlats.VKontakte["chosen"];
			isVKontakteChosen = EditorGUILayout.Toggle ("VKontakte", isVKontakteChosen);
			this.chosenPlats.VKontakte["chosen"] = isVKontakteChosen;

			Boolean isYiXinChosen = (Boolean)this.chosenPlats.YiXin["chosen"];
			isYiXinChosen = EditorGUILayout.Toggle ("YiXin", isYiXinChosen);
			this.chosenPlats.YiXin["chosen"] = isYiXinChosen;

			Boolean isMingDaoChosen = (Boolean)this.chosenPlats.MingDao["chosen"];
			isMingDaoChosen = EditorGUILayout.Toggle ("MingDao", isMingDaoChosen);
			this.chosenPlats.MingDao["chosen"] = isMingDaoChosen;

			Boolean isLineChosen = (Boolean)this.chosenPlats.Line["chosen"];
			isLineChosen = EditorGUILayout.Toggle ("Line", isLineChosen);
			this.chosenPlats.Line["chosen"] = isLineChosen;

			Boolean isWhatsAppChosen = (Boolean)this.chosenPlats.WhatsApp["chosen"];
			isWhatsAppChosen = EditorGUILayout.Toggle ("WhatsApp", isWhatsAppChosen);
			this.chosenPlats.WhatsApp["chosen"] = isWhatsAppChosen;

			Boolean isKakaoChosen = (Boolean)this.chosenPlats.Kakao["chosen"];
			isKakaoChosen = EditorGUILayout.Toggle ("Kakao", isKakaoChosen);
			this.chosenPlats.Kakao["chosen"] = isKakaoChosen;

			Boolean isFacebookMessengerChosen = (Boolean)this.chosenPlats.FacebookMessenger["chosen"];
			isFacebookMessengerChosen = EditorGUILayout.Toggle ("FacebookMessenger", isFacebookMessengerChosen);
			this.chosenPlats.FacebookMessenger["chosen"] = isFacebookMessengerChosen;

			Boolean isAlipayChosen = (Boolean)this.chosenPlats.Alipay["chosen"];
			isAlipayChosen = EditorGUILayout.Toggle ("Alipay", isAlipayChosen);
			this.chosenPlats.Alipay["chosen"] = isAlipayChosen;

			Boolean isDingTalkChosen = (Boolean)this.chosenPlats.DingTalk["chosen"];
			isDingTalkChosen = EditorGUILayout.Toggle ("DingTalk", isDingTalkChosen);
			this.chosenPlats.DingTalk["chosen"] = isDingTalkChosen;

			Boolean isYoutubeChosen = (Boolean)this.chosenPlats.Youtube["chosen"];
			isYoutubeChosen = EditorGUILayout.Toggle ("Youtube", isYoutubeChosen);
			this.chosenPlats.Youtube["chosen"] = isYoutubeChosen;

			Boolean isMeiPaiChosen = (Boolean)this.chosenPlats.MeiPai["chosen"];
			isMeiPaiChosen = EditorGUILayout.Toggle ("MeiPai", isMeiPaiChosen);
			this.chosenPlats.MeiPai["chosen"] = isMeiPaiChosen;


			Save ();
		}

		private void Prepare()
		{
			string filePath = Application.dataPath + "/ShareSDKiOSAutoPackage/Editor/SDKPorter/ManagePlatforms/ChosenPlatforms.bin";
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				Stream destream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				ChosenPlatforms chosenPlats = (ChosenPlatforms)formatter.Deserialize(destream);
				destream.Flush();
				destream.Close();
				this.chosenPlats = chosenPlats;
			}
			catch(Exception)
			{
				this.chosenPlats = new ChosenPlatforms ();
			}
		}

		private void Save()
		{
			try
			{
				string filePath = Application.dataPath + "/ShareSDKiOSAutoPackage/Editor/SDKPorter/ManagePlatforms/ChosenPlatforms.bin";
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				formatter.Serialize(stream, this.chosenPlats);
				stream.Flush();
				stream.Close();
			}
			catch (Exception e) 
			{
				Debug.Log ("save error:" + e);
			}
		}
	}
}