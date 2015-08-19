using System;
using System.Collections;
using UnityEngine; 

namespace cn.sharesdk.unity3d
{
	public class AndroidUtils : ShareSDKUtilsInterface
	{
		private AndroidJavaClass ssdk;

		public AndroidUtils (GameObject go) 
		{
			Debug.Log("AndroidUtils  ===>>>  AndroidUtils" );
			try{
				ssdk = new AndroidJavaClass("cn.sharesdk.unity3d.ShareSDKUtils");
				ssdk.CallStatic("prepare", go.name, "_Callback");
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}

		public override void RigisterAppAndSetPlatformConfig (String appKey, Hashtable configs) 
		{
			String json = MiniJSON.jsonEncode(configs);
			Debug.Log("AndroidUtils  ===>>>  SetPlatformConfig === " + json);
			if (ssdk != null) 
			{			
				ssdk.CallStatic("initSDKAndSetPlatfromConfig", appKey, json);
			}
		}

		public override void Authorize (PlatformType platform, EventResultListener resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  Authorize" );
			authHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("authorize", (int)platform);
			}
		}

		public override void CancelAuthorize (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				ssdk.CallStatic("removeAccount", (int)platform);
			}
		}

		public override bool IsAuthorizedValid (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				return ssdk.CallStatic<bool>("isAuthValid", (int)platform);
			}
			return false;
		}

		public override bool isClientValid (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				return ssdk.CallStatic<bool>("isClientValid", (int)platform);
			}
			return false;
		}

		public override void GetUserInfo (PlatformType platform, EventResultListener resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShowUser" );
			showUserHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("showUser", (int)platform);
			}
		}

		public override void ShareContentWithAPI (PlatformType platform, Hashtable content, EventResultListener resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShareContent to one platform" );
			ShareContentWithAPI (new PlatformType[]{ platform }, content, resultHandler);
		}

		public override void ShareContentWithAPI (PlatformType[] platforms, Hashtable content, EventResultListener resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  Share" );
			shareHandler = resultHandler;
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				foreach (PlatformType platform in platforms)
				{
					ssdk.CallStatic("shareContent", (int)platform, json);
				}
			}
		}

		public override void ShowShareMenu (PlatformType[] platforms, Hashtable content, int x, int y, MenuArrowDirection direction, EventResultListener resultHandler) 
		{
			ShowShareView(0, content, resultHandler);
		}

		public override void ShowShareView (PlatformType platform, Hashtable content, EventResultListener resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  OnekeyShare platform ===" + (int)platform );
			shareHandler = resultHandler;
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				ssdk.CallStatic("onekeyShare", (int)platform, json);
			}
		}
		
		public override void GetFriendList (PlatformType platform, int count, int page, EventResultListener resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetFriendList" );
			getFriendsHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("getFriendList", (int)platform, count, page);
			}
		}

		public override void FollowFriend (PlatformType platform, String account, EventResultListener resultHandler)
		{
			Debug.Log("AndroidUtils  ===>>>  FollowFriend" );
			followFriendHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("followFriend", (int)platform, account);
			}
		}

		public override Hashtable GetAuthInfo (PlatformType platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetAuthInfo" );
			if (ssdk != null) 
			{
				String result = ssdk.CallStatic<String>("getAuthInfo", (int)platform);
				return (Hashtable) MiniJSON.jsonDecode(result);
			}
			return new Hashtable ();
		}

		public override void CloseSSOWhenAuthorize (Boolean open)
		{
			Debug.Log("AndroidUtils  ===>>>  DisableSSOWhenAuthorize" );
			if (ssdk != null) 
			{
				ssdk.CallStatic("disableSSOWhenAuthorize", open);
			}
		}

		public override void OnActionCallback (string message) 
		{
			if (message == null) 
			{
				return;
			}

			Hashtable res = (Hashtable) MiniJSON.jsonDecode(message);
			if (res == null || res.Count <= 0) 
			{
				return;
			}

			int status = Convert.ToInt32(res["status"]);
			PlatformType platform = (PlatformType)Convert.ToInt32(res["platform"]);
			int action = Convert.ToInt32(res["action"]);
			// Success = 1, Fail = 2, Cancel = 3
			switch(status) 
			{
				case 1: 
				{
					Console.WriteLine(message);
					Hashtable resp = (Hashtable) res["res"];
					OnComplete(platform, action, resp);
					break;
				} 
				case 2: 
				{
					Console.WriteLine(message);
					Hashtable throwable = (Hashtable) res["res"];
					OnError(platform, action, throwable);
					break;
				} 
				case 3: 
				{
					OnCancel(platform, action);
				    break;
				} 
			}
		}

		public override void OnError (PlatformType platform, int action, Hashtable throwable) 
		{
			switch (action) 
			{
				case 1: 
				{ // 1 == Platform.ACTION_AUTHORIZING
					if (authHandler != null) 
					{
						authHandler(ResponseState.Fail, platform, throwable);
					}
					break;
				} 
				case 2:
				{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
					if (getFriendsHandler != null) 
					{
						getFriendsHandler(ResponseState.Fail, platform, throwable);
					}
					break;
				}
				case 8: 
				{ // 8 == Platform.ACTION_USER_INFOR
					if (showUserHandler != null) 
					{
						showUserHandler(ResponseState.Fail, platform, throwable);
					}
					break;
				} 
				case 9: 
				{ // 9 == Platform.ACTION_SHARE
					if (shareHandler != null) 
					{
						shareHandler(ResponseState.Fail, platform, throwable);
					}
					break;
				} 
			}
		}

		public override void OnComplete (PlatformType platform, int action, Hashtable res) 
		{
			switch (action) 
			{
				case 1: 
				{ // 1 == Platform.ACTION_AUTHORIZING
					if (authHandler != null) 
					{
						authHandler(ResponseState.Success, platform, null);
					}
					break;
				} 
				case 2:
				{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
					if (getFriendsHandler != null) 
					{
						getFriendsHandler(ResponseState.Success, platform, res);
					}
					break;
				}
				case 8: 
				{ // 8 == Platform.ACTION_USER_INFOR
					if (showUserHandler != null) 
					{
						showUserHandler(ResponseState.Success, platform, res);
					}
					break;
				} 
				case 9: 
				{ // 9 == Platform.ACTION_SHARE
					if (shareHandler != null) 
					{
						shareHandler(ResponseState.Success, platform, res);
					}
					break;
				}
			}
		}

		public override void OnCancel (PlatformType platform, int action) 
		{
			switch (action) 
			{
				case 1: 
				{ // 1 == Platform.ACTION_AUTHORIZING
					if (authHandler != null) 
					{
						authHandler(ResponseState.Cancel, platform, null);
					}
					break;
				} 
				case 2:
				{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
					if (getFriendsHandler != null) 
					{
						getFriendsHandler(ResponseState.Cancel, platform, null);
					}
					break;
				}
			    case 8: 
				{ // 8 == Platform.ACTION_USER_INFOR
					if (showUserHandler != null) 
					{
						showUserHandler(ResponseState.Cancel, platform, null);
					}
					break;
				} 
				case 9: 
				{ // 9 == Platform.ACTION_SHARE
					if (shareHandler != null) 
					{
						shareHandler(ResponseState.Cancel, platform, null);
					}
					break;
				}
			}
		}

	}
}
