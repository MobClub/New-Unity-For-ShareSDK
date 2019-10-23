using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using cn.mob.unity3d.sdkporter;
using System.Reflection;

namespace cn.sharesdk.unity3d
{
	#if UNITY_IOS	
	[CustomEditor(typeof(ShareSDK))]
    [ExecuteInEditMode]
	public class ShareSDKConfigEditor : Editor 
	{
		string appKey = "";
		string appSecret = "";
		Hashtable platformConfList;

        List<string> associatedDomains = new List<string>();

        public ShareSDKConfigEditor()
		{
			SetPlatformConfList ();
		}

		void Awake()
		{
			Prepare ();
		}

		//导出时会自动触发一次此方法
		public void OnDisable() 
		{
			var obj = target as ShareSDK;
            if (obj != null)
            {
                if (obj.customAssociatedDomains != null && obj.customAssociatedDomains.Count > 0)
                {
                    associatedDomains.AddRange(obj.customAssociatedDomains);
                }

                appKey = obj.appKey;
                appSecret = obj.appSecret;
                Save();
                checkPlatforms(obj.devInfo);
            }

            Debug.LogWarning ("ShareSDK OnDisable");
		}

		private void SetPlatformConfList()
		{
			platformConfList = new Hashtable();
			platformConfList.Add ((int)PlatformType.AliSocial,"app_id");
			platformConfList.Add ((int)PlatformType.Tumblr,"consumer_key");
			platformConfList.Add ((int)PlatformType.VKontakte,"application_id");
			platformConfList.Add ((int)PlatformType.Pinterest,"client_id");
			platformConfList.Add ((int)PlatformType.KakaoTalk,"app_key");
			platformConfList.Add ((int)PlatformType.KakaoStory,"app_key");
			platformConfList.Add ((int)PlatformType.Facebook,"api_key");
			platformConfList.Add ((int)PlatformType.FacebookMessenger,"api_key");
			platformConfList.Add ((int)PlatformType.QZone,"app_id");
			platformConfList.Add ((int)PlatformType.QQ,"app_id");
			platformConfList.Add ((int)PlatformType.QQPlatform,"app_id");
			platformConfList.Add ((int)PlatformType.MeiPai,"app_key");
			platformConfList.Add ((int)PlatformType.WeChat,"app_id");
			platformConfList.Add ((int)PlatformType.WeChatMoments,"app_id");
			platformConfList.Add ((int)PlatformType.WeChatFavorites,"app_id");
			platformConfList.Add ((int)PlatformType.YiXinSession,"app_id");
			platformConfList.Add ((int)PlatformType.YiXinTimeline,"app_id");
			platformConfList.Add ((int)PlatformType.YiXinFav,"app_id");
			platformConfList.Add ((int)PlatformType.YixinPlatform,"app_id");
			platformConfList.Add ((int)PlatformType.Dingding,"app_id");
			platformConfList.Add ((int)PlatformType.SinaWeibo,"app_key");
			platformConfList.Add ((int)PlatformType.CMCC,"app_id");
			platformConfList.Add ((int)PlatformType.Twitter,"consumer_key");
            platformConfList.Add ((int)PlatformType.Line,"channel_id");
            platformConfList.Add((int)PlatformType.FacebookAccount, "app_id");
            platformConfList.Add((int)PlatformType.Douyin, "app_key");
            platformConfList.Add((int)PlatformType.WeWork, "app_key");

		}

		private void Prepare()
		{
			try
			{
				var files = System.IO.Directory.GetFiles(Application.dataPath , "MOB.keypds", System.IO.SearchOption.AllDirectories);
				string filePath = files [0];
				FileInfo projectFileInfo = new FileInfo( filePath );
				if(projectFileInfo.Exists)
				{
					StreamReader sReader = projectFileInfo.OpenText();
					string contents = sReader.ReadToEnd();
					sReader.Close();
					sReader.Dispose();
					Hashtable datastore = (Hashtable)MiniJSON.jsonDecode( contents );
					appKey = (string)datastore["MobAppKey"];
					appSecret = (string)datastore["MobAppSecret"];
				}
				else
				{
					Debug.LogWarning ("MOB.keypds no find");
				}
			}
			catch(Exception e) 
			{
				if(appKey.Length == 0)
				{
					appKey = "moba6b6c6d6";
					appSecret = "b89d2427a3bc7ad1aea1e1e8c1d36bf3";
				}
				Debug.LogException (e);
			}
		}
			
		private void Save()
		{
			try
			{
				var files = System.IO.Directory.GetFiles(Application.dataPath , "MOB.keypds", System.IO.SearchOption.AllDirectories);
				string filePath = files [0];
				if(File.Exists(filePath))
				{
					Hashtable datastore = new Hashtable();
					datastore["MobAppKey"] = appKey;
					datastore["MobAppSecret"] = appSecret;
					var json = MiniJSON.jsonEncode(datastore);
					StreamWriter sWriter = new StreamWriter(filePath);
					sWriter.WriteLine(json);
					sWriter.Close();
					sWriter.Dispose();
				}
				else
				{
					Debug.LogWarning ("MOB.keypds no find");
				}
			}
			catch(Exception e) 
			{
				Debug.LogWarning ("error");
				Debug.LogException (e);
			}
		}

		//shareSDK
		private void checkPlatforms(DevInfoSet devInfo)
		{
			Type type = devInfo.GetType();
			FieldInfo[] devInfoFields = type.GetFields();
			Hashtable enablePlatforms = new Hashtable();
			foreach (FieldInfo devInfoField in devInfoFields) 
			{
				DevInfo info = (DevInfo) devInfoField.GetValue(devInfo);
				if(info.Enable)
				{
					int platformId = (int) info.GetType().GetField("type").GetValue(info);
					string appkey = GetAPPKey (info,platformId);
					enablePlatforms.Add (platformId,appkey);

                    if (info.GetType().GetField("app_universalLink") != null)
                    {
                        string app_universalLink = GetValueByName(info, "app_universalLink");
                        if (app_universalLink != null && app_universalLink.Length > 0)
                        {

                            app_universalLink = app_universalLink.Trim().TrimEnd('/');

                            if (app_universalLink.Contains("://"))
                            {
                                string[] links = app_universalLink.Split(new[] { "://" }, StringSplitOptions.None);
                                app_universalLink = "applinks:" + links[1];
                                associatedDomains.Add(app_universalLink);
                            }
                            else
                            {
                                if (app_universalLink.Contains(":"))
                                {
                                    associatedDomains.Add(app_universalLink);
                                }
                                else
                                {
                                    app_universalLink = "applinks:" + app_universalLink;
                                    associatedDomains.Add(app_universalLink);
                                }

                            }

                        }
                    }
                }
			}
			var files = System.IO.Directory.GetFiles(Application.dataPath , "ShareSDK.mobpds", System.IO.SearchOption.AllDirectories);
			string filePath = files [0];
			FileInfo projectFileInfo = new FileInfo( filePath );
			if (projectFileInfo.Exists) 
			{
				StreamReader sReader = projectFileInfo.OpenText();
				string contents = sReader.ReadToEnd();
				sReader.Close();
				sReader.Dispose();
				Hashtable datastore = (Hashtable)MiniJSON.jsonDecode( contents );
				if (datastore.ContainsKey ("ShareSDKPlatforms"))
				{
					datastore["ShareSDKPlatforms"] = enablePlatforms;
				}
				else 
				{
					datastore.Add ("ShareSDKPlatforms",enablePlatforms);
				}

                //Debug.LogWarning("=======================");
                Debug.LogWarning(associatedDomains.ToArray());
                //if (associatedDomains.Count > 0)
                //{
                    var associatedDomains_t = associatedDomains.Distinct();
                   
                    if (datastore.ContainsKey("AssociatedDomains"))
                    {

                        datastore["AssociatedDomains"] = associatedDomains_t.ToArray();
                    }
                    else
                    {
                        datastore.Add("AssociatedDomains", associatedDomains_t.ToArray());
                    }
                //}


                var json = MiniJSON.jsonEncode(datastore);
				StreamWriter sWriter = new StreamWriter(filePath);
				sWriter.WriteLine(json);
				sWriter.Close();
				sWriter.Dispose();
			}
		}

        private string GetValueByName(DevInfo devInfoField,string valueName)
		{
			return (string)devInfoField.GetType ().GetField (valueName).GetValue (devInfoField);
		}

		private string GetAPPKey (DevInfo devInfoField, int platformId)
		{
			string valueName = (string)platformConfList[platformId];
			if(valueName == null)
			{
				return "";
			}
			return GetValueByName (devInfoField, valueName);
		}
	}

    [CustomEditor(typeof(ShareSDKRestoreScene))]
    [ExecuteInEditMode]
    public class ShareSDKRestoreSceneEditor : Editor
    {
        public ShareSDKRestoreSceneEditor()
        {
           
        }

        void Awake()
        {

        }

        //导出时会自动触发一次此方法
        public void OnDisable()
        {
            var restoreSceneObj = target as ShareSDKRestoreScene;
            if (restoreSceneObj != null)
            {
                checkRestoreScene(restoreSceneObj.restoreSceneConfig);

            }
            Debug.LogWarning("ShareSDKRestoreScene OnDisable");
        }

        private void checkRestoreScene(RestoreSceneConfigure restoreSceneConfig)
        {

            Hashtable enableRestoreScene = new Hashtable();
            if (restoreSceneConfig != null && restoreSceneConfig.Enable)
            {
                enableRestoreScene.Add("open", "1");
                if (restoreSceneConfig.capabilititesAssociatedDomain != null)
                {
                    enableRestoreScene.Add("Capabilitites_AssociatedDomain", restoreSceneConfig.capabilititesAssociatedDomain);
                    enableRestoreScene.Add("Capabilitites_EntitlementsPath", restoreSceneConfig.capabilititesEntitlementsPathInXcode);
                }
                else
                {
                    enableRestoreScene.Add("Capabilitites_AssociatedDomain", "");
                    enableRestoreScene.Add("Capabilitites_EntitlementsPath", "");
                }
            }

            var files = System.IO.Directory.GetFiles(Application.dataPath, "ShareSDK.mobpds", System.IO.SearchOption.AllDirectories);
            string filePath = files[0];
            FileInfo projectFileInfo = new FileInfo(filePath);
            if (projectFileInfo.Exists)
            {
                StreamReader sReader = projectFileInfo.OpenText();
                string contents = sReader.ReadToEnd();
                sReader.Close();
                sReader.Dispose();
                Hashtable datastore = (Hashtable)MiniJSON.jsonDecode(contents);
                if (datastore.ContainsKey("ShareSDKRestoreScene"))
                {
                    datastore["ShareSDKRestoreScene"] = enableRestoreScene;
                }
                else
                {
                    datastore.Add("ShareSDKRestoreScene", enableRestoreScene);
                }
                var json = MiniJSON.jsonEncode(datastore);
                StreamWriter sWriter = new StreamWriter(filePath);
                sWriter.WriteLine(json);
                sWriter.Close();
                sWriter.Dispose();
            }
        }
    }
#endif
}