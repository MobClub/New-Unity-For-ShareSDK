using System;
using System.Collections;
using UnityEngine; 

namespace cn.sharesdk.unity3d
{
	#if UNITY_ANDROID
	public class AndroidUtils : ShareSDKUtilsInterface
	{
		private AndroidJavaObject ssdk;

		public AndroidUtils (GameObject go) 
		{
			Debug.Log("AndroidUtils  ===>>>  AndroidUtils" );
			try{
				ssdk = new AndroidJavaObject("cn.sharesdk.unity3d.ShareSDKUtils", go.name, "_Callback");
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
				ssdk.Call("initSDKAndSetPlatfromConfig", appKey, json);
			}
		}

		public override void Authorize (int reqID, PlatformType platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  Authorize" );
			if (ssdk != null) 
			{
				ssdk.Call("authorize", reqID, (int)platform);
			}
		}

		public override void CancelAuthorize (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				ssdk.Call("removeAccount", (int)platform);
			}
		}

		public override bool IsAuthorizedValid (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				return ssdk.Call<bool>("isAuthValid", (int)platform);
			}
			return false;
		}

		public override bool IsClientValid (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				return ssdk.Call<bool>("isClientValid", (int)platform);
			}
			return false;
		}

		public override void GetUserInfo (int reqID, PlatformType platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShowUser" );
			if (ssdk != null) 
			{
				ssdk.Call("showUser", reqID, (int)platform);
			}
		}

		public override void ShareContent (int reqID, PlatformType platform, Hashtable content) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShareContent to one platform" );
			ShareContent (reqID, new PlatformType[]{ platform }, content);
		}

		public override void ShareContent (int reqID, PlatformType[] platforms, Hashtable content) 
		{
			Debug.Log("AndroidUtils  ===>>>  Share" );
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				foreach (PlatformType platform in platforms)
				{
					ssdk.Call("shareContent", reqID, (int)platform, json);
				}
			}
		}

		public override void ShowShareMenu (int reqID, PlatformType[] platforms, Hashtable content, int x, int y) 
		{
			ShowShareView(reqID, 0, content);
		}

		public override void ShowShareView (int reqID, PlatformType platform, Hashtable content) 
		{
			Debug.Log("AndroidUtils  ===>>>  OnekeyShare platform ===" + (int)platform );
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				ssdk.Call("onekeyShare", reqID, (int)platform, json);
			}
		}
		
		public override void GetFriendList (int reqID, PlatformType platform, int count, int page) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetFriendList" );
			if (ssdk != null) 
			{
				ssdk.Call("getFriendList", reqID, (int)platform, count, page);
			}
		}

		public override void AddFriend (int reqID, PlatformType platform, String account)
		{
			Debug.Log("AndroidUtils  ===>>>  FollowFriend" );
			if (ssdk != null) 
			{
				ssdk.Call("followFriend", reqID, (int)platform, account);
			}
		}

		public override Hashtable GetAuthInfo (PlatformType platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetAuthInfo" );
			if (ssdk != null) 
			{
				String result = ssdk.Call<String>("getAuthInfo", (int)platform);
				return (Hashtable) MiniJSON.jsonDecode(result);
			}
			return new Hashtable ();
		}

		public override void CloseSSOWhenAuthorize (Boolean open)
		{
			Debug.Log("AndroidUtils  ===>>>  DisableSSOWhenAuthorize" );
			if (ssdk != null) 
			{
				ssdk.Call("disableSSOWhenAuthorize", open);
			}
		}

	}
	#endif
}
