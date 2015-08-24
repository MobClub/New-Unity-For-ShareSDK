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

		public string appkey = "androidv1101";
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
			shareSDKUtils.RigisterAppAndSetPlatformConfig(appkey, platformConfigs);			
			#elif UNITY_IPHONE
			#endif
		}
		
		/// <summary>
		/// callback the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		private void _Callback (string data)
		{
			#if UNITY_ANDROID
			shareSDKUtils.OnActionCallback(data);			
			#elif UNITY_IPHONE
			#endif
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
			#if UNITY_ANDROID
			shareSDKUtils.RigisterAppAndSetPlatformConfig (appKey, configInfo);			
			#elif UNITY_IPHONE
			#endif
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
		public void Authorize (PlatformType platform)
		{
			#if UNITY_ANDROID
			shareSDKUtils.Authorize(platform, authHandler);			
			#elif UNITY_IPHONE
			#endif
		}
		
		/// <summary>
		/// Cancel authorized
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		public void CancelAuthorize (PlatformType platform)
		{
			#if UNITY_ANDROID
			shareSDKUtils.CancelAuthorize(platform);			
			#elif UNITY_IPHONE
			#endif
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
			#if UNITY_ANDROID
			return shareSDKUtils.IsAuthorizedValid(platform);			
			#elif UNITY_IPHONE
			#endif
		}

		public bool IsClientValid (PlatformType platform)
		{
			#if UNITY_ANDROID
			return shareSDKUtils.IsClientValid(platform);			
			#elif UNITY_IPHONE
			#endif
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
		public void GetUserInfo (PlatformType platform)
		{
			#if UNITY_ANDROID
			shareSDKUtils.GetUserInfo(platform, showUserHandler);			
			#elif UNITY_IPHONE
			#endif
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
		public void ShareContentWithAPI(PlatformType platform, Hashtable content)
		{
			#if UNITY_ANDROID
			shareSDKUtils.ShareContentWithAPI(platform, content, shareHandler);			
			#elif UNITY_IPHONE
			#endif
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
		public void ShareContentWithAPI(PlatformType[] platforms, Hashtable content)
		{
			#if UNITY_ANDROID
			shareSDKUtils.ShareContentWithAPI(platforms, content, shareHandler);			
			#elif UNITY_IPHONE
			#endif
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
		public void ShowShareMenu (PlatformType[] platforms, Hashtable content, int x, int y)
		{
			#if UNITY_ANDROID
			shareSDKUtils.ShowShareMenu(platforms, content, x, y, shareHandler);			
			#elif UNITY_IPHONE
			#endif
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
		public void ShowShareView (PlatformType platform, Hashtable content)
		{			
			#if UNITY_ANDROID
			Debug.Log("Demo  ===>>>  ssdk.ShowShareView" );
			shareSDKUtils.ShowShareView(platform, content, shareHandler);			
			#elif UNITY_IPHONE
			#endif
		}

		/// <summary>
		/// Gets the friends.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="count">Count.</param>
		/// <param name="page">Page.</param>
		public void GetFriendList (PlatformType platform, int count, int page)
		{
			#if UNITY_ANDROID
			shareSDKUtils.GetFriendList (platform, count, page, getFriendsHandler);			
			#elif UNITY_IPHONE
			#endif
		}

		/// <summary>
		/// Follows the friend.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="account">Account.</param>
		public void FollowFriend (PlatformType platform, String account)
		{
			#if UNITY_ANDROID
			shareSDKUtils.FollowFriend (platform, account, followFriendHandler);			
			#elif UNITY_IPHONE
			#endif
		}

		/// <summary>
		/// Gets the auth info.
		/// </summary>
		/// <param name="type">Type.</param>
		public Hashtable GetAuthInfo (PlatformType platform)
		{
			#if UNITY_ANDROID
			return shareSDKUtils.GetAuthInfo (platform);			
			#elif UNITY_IPHONE
			#endif
		}

		/// <summary>
		/// Close the SSO when authorize.
		/// </summary>
		/// <param name="open">If set to <c>true</c> open.</param>
		public void CloseSSOWhenAuthorize(Boolean open){
			#if UNITY_ANDROID
			shareSDKUtils.CloseSSOWhenAuthorize (open);			
			#elif UNITY_IPHONE
			#endif
		}

	}
}