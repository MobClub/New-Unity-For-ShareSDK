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

		public override void Authorize (PlatformType platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  Authorize" );
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

		public override bool IsClientValid (PlatformType platform) 
		{
			if (ssdk != null) 
			{
				return ssdk.CallStatic<bool>("isClientValid", (int)platform);
			}
			return false;
		}

		public override void GetUserInfo (PlatformType platform) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShowUser" );
			if (ssdk != null) 
			{
				ssdk.CallStatic("showUser", (int)platform);
			}
		}

		public override void ShareContent (PlatformType platform, Hashtable content) 
		{
			Debug.Log("AndroidUtils  ===>>>  ShareContent to one platform" );
			ShareContent (new PlatformType[]{ platform }, content);
		}

		public override void ShareContent (PlatformType[] platforms, Hashtable content) 
		{
			Debug.Log("AndroidUtils  ===>>>  Share" );
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				foreach (PlatformType platform in platforms)
				{
					ssdk.CallStatic("shareContent", (int)platform, json);
				}
			}
		}

		public override void ShowShareMenu (PlatformType[] platforms, Hashtable content, int x, int y) 
		{
			ShowShareView(0, content);
		}

		public override void ShowShareView (PlatformType platform, Hashtable content) 
		{
			Debug.Log("AndroidUtils  ===>>>  OnekeyShare platform ===" + (int)platform );
			String json = MiniJSON.jsonEncode(content);
			if (ssdk != null) 
			{
				ssdk.CallStatic("onekeyShare", (int)platform, json);
			}
		}
		
		public override void GetFriendList (PlatformType platform, int count, int page) 
		{
			Debug.Log("AndroidUtils  ===>>>  GetFriendList" );
			if (ssdk != null) 
			{
				ssdk.CallStatic("getFriendList", (int)platform, count, page);
			}
		}

		public override void AddFriend (PlatformType platform, String account)
		{
			Debug.Log("AndroidUtils  ===>>>  FollowFriend" );
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

	}
}
