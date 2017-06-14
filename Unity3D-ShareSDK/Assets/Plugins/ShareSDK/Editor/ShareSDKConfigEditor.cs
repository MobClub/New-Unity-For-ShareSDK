using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;


namespace cn.sharesdk.unity3d
{
[CustomEditor(typeof(ShareSDK))]
[ExecuteInEditMode]
public class ShareSDKConfigEditor : Editor {

		private ShareSDKConfig config;

		void Awake()
		{
			this.config = new ShareSDKConfig ();
		}
				
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var obj = target as ShareSDK;
			this.config.appKey = obj.appKey;
			this.config.appSecret = obj.appSecret;
//			this.config.SinaWeibo ["chosen"] = obj.devInfo.sinaweibo.Enable;
//			this.config.TencentWeibo ["chosen"] = obj.devInfo.tencentweibo.Enable;
//			this.config.DouBan ["chosen"] = obj.devInfo.douban.Enable;
//			this.config.QQ["chosen"] = obj.devInfo.qq.Enable;
//			this.config.WeChat["chosen"] = obj.devInfo.wechat.Enable;
//			this.config.Renren["chosen"] = obj.devInfo.renren.Enable;
//			this.config.Kaixin["chosen"] = obj.devInfo.kaiXin.Enable;
//			this.config.Facebook["chosen"] = obj.devInfo.facebook.Enable;
//			this.config.Evernote["chosen"] = obj.devInfo.evernote.Enable;
//			this.config.GooglePlus["chosen"] = obj.devInfo.googlePlus.Enable;
//			this.config.Instagram["chosen"] = obj.devInfo.instagram.Enable;
//			this.config.LinkedIn["chosen"] = obj.devInfo.linkedIn.Enable;
//			this.config.Tumblr["chosen"] = obj.devInfo.tumblr.Enable;
//			this.config.Mail["chosen"] = obj.devInfo.email.Enable;
//			this.config.SMS["chosen"] = obj.devInfo.shortMessage.Enable;
//			this.config.Print["chosen"] = obj.devInfo.shortMessage.Enable;
			Save ();
		}
		private void Save()
		{
			try
			{
					string filePath = Application.dataPath + "/Plugins/ShareSDK/ShareSDKConfig.bin";
					BinaryFormatter formatter = new BinaryFormatter();
					Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
					formatter.Serialize(stream, this.config);
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