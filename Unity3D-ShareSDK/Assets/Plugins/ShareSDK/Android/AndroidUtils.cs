using System;
using System.Collections;
using UnityEngine; 

namespace cn.sharesdk.unity3d
{
	public class AndroidUtils : ShareSDKUtilsInterface
	{
		private AndroidJavaClass ssdk;

		public AndroidUtils(GameObject go) 
		{
			Debug.Log("AndroidUtils  ===>>>  AndroidUtils" );
			try{
				ssdk = new AndroidJavaClass("cn.sharesdk.unity3d.ShareSDKUtils");
				ssdk.CallStatic("prepare", go.name, "_Callback");
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}

		public override void InitSDK() 
		{
			InitSDK(null);
		}

		public override void InitSDK(string appKey) 
		{
			Debug.Log("AndroidUtils  ===>>>  InitSDK" + appKey);
			if (ssdk != null) {
				ssdk.CallStatic("initSDK", appKey);
			}
		}

		public override void SetPlatformConfig(int platform, Hashtable configs) 
		{
			String json = MiniJSON.jsonEncode(configs);
			Debug.Log("AndroidUtils  ===>>>  SetPlatformConfig === " + json);
			if (ssdk != null) 
			{			
				ssdk.CallStatic("setPlatformConfig", platform, json);
			}
		}

		public override void Authorize(int platform, AuthResultEvent resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  Authorize" );
			authHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("authorize", platform);
			}
		}

		public override void RemoveAccount(int platform) 
		{
			if (ssdk != null) 
			{
				ssdk.CallStatic("removeAccount", platform);
			}
		}

		public override bool IsValid(int platform) 
		{
			if (ssdk != null) 
			{
				return ssdk.CallStatic<bool>("isValid", platform);
			}
			return false;
		}

		public override void ShowUser(int platform, GetUserInfoResultEvent resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShowUser" );
			showUserHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("showUser", platform);
			}
		}

		public override void Share(int platform, Hashtable content, ShareResultEvent resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  Share" );
			shareHandler = resultHandler;
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				ssdk.CallStatic("share", platform, json);
			}
		}

		public override void OnekeyShare(Hashtable content, ShareResultEvent resultHandler) 
		{
			OnekeyShare(0, content, resultHandler);
		}

		public override void OnekeyShare(int platform, Hashtable content, ShareResultEvent resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  OnekeyShare" );
			shareHandler = resultHandler;
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				ssdk.CallStatic("onekeyShare", platform, json);
			}
		}
		
		public override void GetFriendList(int platform, int count, int page, GetFriendsResultEvent resultHandler) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetFriendList" );
			getFriendsHandler = resultHandler;
			if (ssdk != null) 
			{
				ssdk.CallStatic("getFriendList", platform, count, page);
			}
		}

		public override Hashtable GetAuthInfo(int platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetAuthInfo" );
			if (ssdk != null) 
			{
				String result = ssdk.CallStatic<String>("getAuthInfo", platform);
				return (Hashtable) MiniJSON.jsonDecode(result);
			}
			return new Hashtable ();
		}

		public override void DisableSSOWhenAuthorize(Boolean open)
		{
			Debug.Log("AndroidUtils  ===>>>  DisableSSOWhenAuthorize" );
			if (ssdk != null) 
			{
				ssdk.CallStatic("disableSSOWhenAuthorize", open);
			}
		}

		public override void OnActionCallback(string message) 
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
			int platform = Convert.ToInt32(res["platform"]);
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

		public override void OnError(int platform, int action, Hashtable throwable) 
		{
			switch (action) 
			{
				case 1: 
				{ // 1 == Platform.ACTION_AUTHORIZING
					if (authHandler != null) 
					{
						authHandler(ResponseState.Fail, (PlatformType) platform, throwable);
					}
					break;
				} 
				case 2:
				{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
					if (getFriendsHandler != null) 
					{
						getFriendsHandler(ResponseState.Fail, (PlatformType) platform, throwable);
					}
					break;
				}
				case 8: 
				{ // 8 == Platform.ACTION_USER_INFOR
					if (showUserHandler != null) 
					{
						showUserHandler(ResponseState.Fail, (PlatformType) platform, throwable);
					}
					break;
				} 
				case 9: 
				{ // 9 == Platform.ACTION_SHARE
					if (shareHandler != null) 
					{
						shareHandler(ResponseState.Fail, (PlatformType) platform, throwable);
					}
					break;
				} 
			}
		}

		public override void OnComplete(int platform, int action, Hashtable res) 
		{
			switch (action) 
			{
				case 1: 
				{ // 1 == Platform.ACTION_AUTHORIZING
					if (authHandler != null) 
					{
						authHandler(ResponseState.Success, (PlatformType) platform, null);
					}
					break;
				} 
				case 2:
				{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
					if (getFriendsHandler != null) 
					{
						getFriendsHandler(ResponseState.Success, (PlatformType) platform, res);
					}
					break;
				}
				case 8: 
				{ // 8 == Platform.ACTION_USER_INFOR
					if (showUserHandler != null) 
					{
						showUserHandler(ResponseState.Success, (PlatformType) platform, res);
					}
					break;
				} 
				case 9: 
				{ // 9 == Platform.ACTION_SHARE
					if (shareHandler != null) 
					{
						shareHandler(ResponseState.Success, (PlatformType) platform, res);
					}
					break;
				}
			}
		}

		public override void OnCancel(int platform, int action) 
		{
			switch (action) 
			{
				case 1: 
				{ // 1 == Platform.ACTION_AUTHORIZING
					if (authHandler != null) 
					{
						authHandler(ResponseState.Cancel, (PlatformType) platform, null);
					}
					break;
				} 
				case 2:
				{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
					if (getFriendsHandler != null) 
					{
						getFriendsHandler(ResponseState.Cancel, (PlatformType) platform, null);
					}
					break;
				}
			    case 8: 
				{ // 8 == Platform.ACTION_USER_INFOR
					if (showUserHandler != null) 
					{
						showUserHandler(ResponseState.Cancel, (PlatformType) platform, null);
					}
					break;
				} 
				case 9: 
				{ // 9 == Platform.ACTION_SHARE
					if (shareHandler != null) 
					{
						shareHandler(ResponseState.Cancel, (PlatformType) platform, null);
					}
					break;
				}
			}
		}

	}

}
