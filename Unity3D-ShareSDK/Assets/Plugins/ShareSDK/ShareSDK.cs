using UnityEngine;
using System.Collections;
using System;  
using System.Collections.Generic;  
using System.IO;  
using System.Text;
using System.Reflection;

namespace cn.sharesdk.unity3d
{
	/// <summary>
	/// ShareSDK.
	/// </summary>
	public class ShareSDK : MonoBehaviour 
	{
		public AppKey appkey;
		public DevInfoSet devInfo;
		private ShareSDKUtilsInterface shareSDKUtils;

		public EventResultListener authHandler;
		public EventResultListener shareHandler;
		public EventResultListener showUserHandler;
		public EventResultListener getFriendsHandler;
		public EventResultListener followFriendHandler;

		void Awake()
		{				
			print("ShareSDK Awake");
			Type type = devInfo.GetType();
			Hashtable platformConfigs = new Hashtable();
			FieldInfo[] devInfoFields = type.GetFields();
			foreach (FieldInfo devInfoField in devInfoFields) 
			{	
				DevInfo info = (DevInfo) devInfoField.GetValue(devInfo);
				int platformId = (int) info.GetType().GetField("type").GetValue(info);
				FieldInfo[] fields = info.GetType().GetFields();
				Hashtable table = new Hashtable();
				foreach (FieldInfo field in fields) 
				{
					table.Add(field.Name, field.GetValue(info));
				}
				platformConfigs.Add(platformId, table);
			}
			#if UNITY_ANDROID
			shareSDKUtils = new AndroidUtils(gameObject);
			#elif UNITY_IPHONE
			#endif
			appkey = new AppKey ();
			shareSDKUtils.RigisterAppAndSetPlatformConfig(appkey.appKey, platformConfigs);
		}
		
		/// <summary>
		/// callback the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		private void _Callback (string data)
		{
			if (data == null) 
			{
				return;
			}
			
			Hashtable res = (Hashtable) MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) 
			{
				return;
			}
			
			int status = Convert.ToInt32(res["status"]);
			int reqID = Convert.ToInt32(res["reqID"]);
			PlatformType platform = (PlatformType)Convert.ToInt32(res["platform"]);
			int action = Convert.ToInt32(res["action"]);
			// Success = 1, Fail = 2, Cancel = 3
			switch(status) 
			{
				case 1: 
				{
					Console.WriteLine(data);
					Hashtable resp = (Hashtable) res["res"];
					OnComplete(reqID, platform, action, resp);
					break;
				} 
				case 2: 
				{
					Console.WriteLine(data);
					Hashtable throwable = (Hashtable) res["res"];
					OnError(reqID, platform, action, throwable);
					break;
				} 
				case 3: 
				{
					OnCancel(reqID, platform, action);
					break;
				} 
			}
		}

		/// <summary>
		/// Raises the error event.
		/// </summary>
		/// <param name="platform">Platform.</param>
		/// <param name="action">Action.</param>
		/// <param name="throwable">Throwable.</param>
		public void OnError (int reqID, PlatformType platform, int action, Hashtable throwable) 
		{
			switch (action) 
			{
			case 1: 
			{ // 1 == Platform.ACTION_AUTHORIZING
				if (authHandler != null) 
				{
					authHandler(reqID, ResponseState.Fail, platform, throwable);
				}
				break;
			} 
			case 2:
			{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
				if (getFriendsHandler != null) 
				{
					getFriendsHandler(reqID, ResponseState.Fail, platform, throwable);
				}
				break;
			}
			case 6:
			{ //6 == Platform.ACTION_FOLLOWING_USER
				if (followFriendHandler != null) 
				{
					followFriendHandler(reqID, ResponseState.Fail, platform, throwable);
				}
				break;
			}
			case 8: 
			{ // 8 == Platform.ACTION_USER_INFOR
				if (showUserHandler != null) 
				{
					showUserHandler(reqID, ResponseState.Fail, platform, throwable);
				}
				break;
			} 
			case 9: 
			{ // 9 == Platform.ACTION_SHARE
				if (shareHandler != null) 
				{
					shareHandler(reqID, ResponseState.Fail, platform, throwable);
				}
				break;
			} 
			}
		}

		/// <summary>
		/// Raises the success event.
		/// </summary>
		/// <param name="platform">Platform.</param>
		/// <param name="action">Action.</param>
		/// <param name="res">Res.</param>
		public void OnComplete (int reqID, PlatformType platform, int action, Hashtable res) 
		{
			switch (action) 
			{
			case 1: 
			{ // 1 == Platform.ACTION_AUTHORIZING
				if (authHandler != null) 
				{
					authHandler(reqID, ResponseState.Success, platform, null);
				}
				break;
			} 
			case 2:
			{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
				if (getFriendsHandler != null) 
				{
					getFriendsHandler(reqID, ResponseState.Success, platform, res);
				}
				break;
			}
			case 6:
			{ //6 == Platform.ACTION_FOLLOWING_USER
				if (followFriendHandler != null) 
				{
					followFriendHandler(reqID, ResponseState.Success, platform, res);
				}
				break;
			}
			case 8: 
			{ // 8 == Platform.ACTION_USER_INFOR
				if (showUserHandler != null) 
				{
					showUserHandler(reqID, ResponseState.Success, platform, res);
				}
				break;
			} 
			case 9: 
			{ // 9 == Platform.ACTION_SHARE
				if (shareHandler != null) 
				{
					shareHandler(reqID, ResponseState.Success, platform, res);
				}
				break;
			}
			}
		}

		/// <summary>
		/// Raises the cancel event.
		/// </summary>
		/// <param name="platform">Platform.</param>
		/// <param name="action">Action.</param>
		public void OnCancel (int reqID, PlatformType platform, int action) 
		{
			switch (action) 
			{
			case 1: 
			{ // 1 == Platform.ACTION_AUTHORIZING
				if (authHandler != null) 
				{
					authHandler(reqID, ResponseState.Cancel, platform, null);
				}
				break;
			} 
			case 2:
			{ //2 == Platform.ACTION_GETTING_FRIEND_LIST
				if (getFriendsHandler != null) 
				{
					getFriendsHandler(reqID, ResponseState.Cancel, platform, null);
				}
				break;
			}
			case 6:
			{ //6 == Platform.ACTION_FOLLOWING_USER
				if (followFriendHandler != null) 
				{
					followFriendHandler(reqID, ResponseState.Cancel, platform, null);
				}
				break;
			}
			case 8: 
			{ // 8 == Platform.ACTION_USER_INFOR
				if (showUserHandler != null) 
				{
					showUserHandler(reqID, ResponseState.Cancel, platform, null);
				}
				break;
			} 
			case 9: 
			{ // 9 == Platform.ACTION_SHARE
				if (shareHandler != null) 
				{
					shareHandler(reqID, ResponseState.Cancel, platform, null);
				}
				break;
			}
			}
		}

		/// <summary>
		/// Sets the platform config.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='configInfo'>
		/// Config info.
		/// </param>
		public void RigisterAppAndSetPlatformConfig (String appKey, Hashtable configInfo)
		{			
			// if you don't add ShareSDK.xml in your assets folder, use the following line
			shareSDKUtils.RigisterAppAndSetPlatformConfig (appKey, configInfo);			
		}
		
		/// <summary>
		/// Authorize the specified type, observer and resultHandler.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='observer'>
		/// Observer.
		/// </param>
		/// <param name='resultHandler'>
		/// Result handler.
		/// </param>
		public void Authorize (int reqID, PlatformType platform)
		{
			shareSDKUtils.Authorize(reqID, platform);			
		}
		
		/// <summary>
		/// Cancel authorized
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		public void CancelAuthorize (PlatformType platform)
		{
			shareSDKUtils.CancelAuthorize(platform);			
		}
		
		/// <summary>
		/// Has authorized
		/// </summary>
		/// <returns>
		/// true has authorized, otherwise not authorized.
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		public bool IsAuthorizedValid (PlatformType platform)
		{
			return shareSDKUtils.IsAuthorizedValid(platform);			
		}

		public bool IsClientValid (PlatformType platform)
		{
			return shareSDKUtils.IsClientValid(platform);			
		}
		
		/// <summary>
		/// Gets the user info.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public void GetUserInfo (int reqID, PlatformType platform)
		{
			shareSDKUtils.GetUserInfo(reqID, platform);			
		}

		/// <summary>
		/// Shares the content.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='resultHandler'>
		/// Callback.
		/// </param>
		public void ShareContent(int reqID, PlatformType platform, Hashtable content)
		{
			shareSDKUtils.ShareContent(reqID, platform, content);			
		}

		/// <summary>
		/// Shares the content.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='resultHandler'>
		/// Callback.
		/// </param>
		public void ShareContent(int reqID, PlatformType[] platforms, Hashtable content)
		{
			shareSDKUtils.ShareContent(reqID, platforms, content);			
		}
				
		/// <summary>
		/// Shows the share menu of using onekeyshare.
		/// </summary>
		/// <param name='types'>
		/// Types.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public void ShowShareMenu (int reqID, PlatformType[] platforms, Hashtable content, int x, int y)
		{
			shareSDKUtils.ShowShareMenu(reqID, platforms, content, x, y);			
		}
		
		/// <summary>
		/// Shows the share view of using onekeyshare.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='content'>
		/// Content.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public void ShowShareView (int reqID, PlatformType platform, Hashtable content)
		{			
			Debug.Log("Demo  ===>>>  ssdk.ShowShareView" );
			shareSDKUtils.ShowShareView(reqID, platform, content);			
		}

		/// <summary>
		/// Gets the friends.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="count">Count.</param>
		/// <param name="page">Page.</param>
		public void GetFriendList (int reqID, PlatformType platform, int count, int page)
		{
			shareSDKUtils.GetFriendList (reqID, platform, count, page);			
		}

		/// <summary>
		/// Follows the friend.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="account">Account.</param>
		public void AddFriend (int reqID, PlatformType platform, String account)
		{
			shareSDKUtils.AddFriend (reqID, platform, account);			
		}

		/// <summary>
		/// Gets the auth info.
		/// </summary>
		/// <param name="type">Type.</param>
		public Hashtable GetAuthInfo (PlatformType platform)
		{
			return shareSDKUtils.GetAuthInfo (platform);			
		}

		/// <summary>
		/// Close the SSO when authorize.
		/// </summary>
		/// <param name="open">If set to <c>true</c> open.</param>
		public void CloseSSOWhenAuthorize(Boolean open){
			shareSDKUtils.CloseSSOWhenAuthorize (open);			
		}

		/// <summary>
		/// Event result listener.
		/// </summary>
		public delegate void EventResultListener (int reqID, ResponseState state, PlatformType type, Hashtable data);

	}
}