using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class Demo : MonoBehaviour {

	public GUISkin demoSkin;
	public ShareSDK ssdk;
	// Use this for initialization
	void Start ()
	{	
		ssdk = gameObject.GetComponent<ShareSDK>();
		ssdk.authHandler = OnAuthResultHandler;
		ssdk.shareHandler = OnShareResultHandler;
		ssdk.showUserHandler = OnGetUserInfoResultHandler;
		ssdk.getFriendsHandler = OnGetFriendsResultHandler;
		ssdk.followFriendHandler = OnFollowFriendResultHandler;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
	
	void OnGUI ()
	{

		GUI.skin = demoSkin;
		
		float scale = 1.0f;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			scale = Screen.width / 320;
		}
		
		float btnWidth = 200 * scale;
		float btnHeight = 45 * scale;
		float btnTop = 20 * scale;
		GUI.skin.button.fontSize = Convert.ToInt32(16 * scale);

		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Authorize"))
		{
			print(ssdk == null);

			ssdk.Authorize(PlatformType.SinaWeibo);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Get User Info"))
		{
			ssdk.GetUserInfo(PlatformType.SinaWeibo);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Show Share Menu"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/1.jpg");
			content.SetTitle("test title");
			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);

			//不同平台分享不同内容
			ShareContent customizeShareParams = new ShareContent();
			customizeShareParams.SetText("Sina share content");
			customizeShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
			customizeShareParams.SetShareType(ContentType.Image);
			customizeShareParams.SetObjectID("SinaID");
			content.SetShareContentCustomize(PlatformType.SinaWeibo, customizeShareParams);
		
			ssdk.ShowPlatformList (null, content, 100, 100);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Show Share View"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/1.jpg");
			content.SetTitle("test title");
			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);
			ssdk.ShowShareContentEditor (PlatformType.TencentWeibo, content);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Share Content"))
		{
			ShareContent content = new ShareContent();
			content.SetText("this is a test string.");
			content.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/1.jpg");
			content.SetTitle("test title");
			content.SetTitleUrl("http://www.mob.com");
			content.SetSite("Mob-ShareSDK");
			content.SetSiteUrl("http://www.mob.com");
			content.SetUrl("http://www.mob.com");
			content.SetComment("test description");
			content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
			content.SetShareType(ContentType.Image);
			ssdk.ShareContent (PlatformType.SinaWeibo, content);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Get Friends SinaWeibo "))
		{
			//获取新浪微博好友，第一页，每页15条数据
			print ("Click Btn Of Get Friends SinaWeibo");
			ssdk.GetFriendList (PlatformType.SinaWeibo, 15, 0);
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Get Token SinaWeibo "))
		{
			Hashtable authInfo = ssdk.GetAuthInfo (PlatformType.SinaWeibo);			
			print ("share result :");
			print (MiniJSON.jsonEncode(authInfo));
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Close SSO Auth"))
		{
			ssdk.DisableSSO (true);			
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Remove Authorize "))
		{
			ssdk.CancelAuthorize (PlatformType.SinaWeibo);			
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Add Friend "))
		{
			//关注新浪微博
			ssdk.AddFriend (PlatformType.SinaWeibo, "3189087725");			
		}

	}
	
	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("authorize success !" + "Platform :" + type);
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
	
	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("get user info result :");
			print (MiniJSON.jsonEncode(result));
			print ("Get userInfo success !Platform :" + type );
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
	
	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("share successfully - share result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			print ("get friend list result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
}
