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
		#if UNITY_ANDROID
		public DevInfoSet devInfo;
		#elif UNITY_IPHONE
		#endif
		private ShareSDKUtilsInterface shareSDKUtils;

		public EventResultListener authHandler;
		public EventResultListener shareHandler;
		public EventResultListener showUserHandler;
		public EventResultListener getFriendsHandler;
		public EventResultListener followFriendHandler;

		void Awake()
		{			
			#if UNITY_IPHONE			
			#elif UNITY_ANDROID
			shareSDKUtils = new AndroidUtils(gameObject);
			shareSDKUtils.InitSDK(appkey);

			print("ShareSDK Awake");
			Type type = devInfo.GetType();
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo field in fields) 
			{	
				DevInfo info = (DevInfo) field.GetValue(devInfo);
				SetPlatfromDevInfo(info);
			}
			#endif
		}

		private void SetPlatfromDevInfo(DevInfo info)
		{
			int platformId = (int) info.GetType().GetField("type").GetValue(info);
			FieldInfo[] fields = info.GetType().GetFields();
			Hashtable table = new Hashtable();
			foreach (FieldInfo field in fields) 
			{
				table.Add(field.Name, field.GetValue(info));
			}
			shareSDKUtils.SetPlatformConfig(platformId, table);
		}
		
		/// <summary>
		/// callback the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		private void _Callback (string data)
		{
			shareSDKUtils.OnActionCallback(data);
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
		public void SetPlatformConfig (PlatformType type, Hashtable configInfo)
		{
			// if you don't add ShareSDK.xml in your assets folder, use the following line
			shareSDKUtils.SetPlatformConfig((int)type, configInfo);
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
		public void Authorize (PlatformType type)
		{
			print("authorize ===>>>" + (shareSDKUtils == null));
			shareSDKUtils.Authorize((int) type, authHandler);
		}
		
		/// <summary>
		/// Cancel authorized
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		public void CancelAuthorie (PlatformType type)
		{
			shareSDKUtils.RemoveAccount((int) type);
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
		public bool HasAuthorized (PlatformType type)
		{
			return shareSDKUtils.IsValid((int) type);
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
		public void GetUserInfo (PlatformType type)
		{
			shareSDKUtils.ShowUser((int) type, showUserHandler);
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
		public void ShowShareMenu (Hashtable content)
		{
			shareSDKUtils.OnekeyShare(content, shareHandler);
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
		public void ShowShareView (PlatformType type, Hashtable content)
		{			
			Debug.Log("ShareSDK  ===>>>  ShowShareView" );
			shareSDKUtils.OnekeyShare((int) type, content, shareHandler);
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
		public void ShareContent (PlatformType type, Hashtable content)
		{
			shareSDKUtils.Share((int) type, content, shareHandler);
		}

		/// <summary>
		/// Gets the friends.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="count">Count.</param>
		/// <param name="page">Page.</param>
		public void GetFriendList (PlatformType type, int count, int page)
		{
			shareSDKUtils.GetFriendList ((int)type, count, page, getFriendsHandler);
		}

		/// <summary>
		/// Follows the friend.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="account">Account.</param>
		public void FollowFriend (PlatformType type, String account)
		{
			shareSDKUtils.FollowFriend ((int)type, account, followFriendHandler);
		}

		/// <summary>
		/// Gets the auth info.
		/// </summary>
		/// <param name="type">Type.</param>
		public Hashtable GetAuthInfo (PlatformType type)
		{
			return shareSDKUtils.GetAuthInfo ((int)type);
		}

		/// <summary>
		/// Disables the SSO when authorize.
		/// </summary>
		/// <param name="open">If set to <c>true</c> open.</param>
		public void DisableSSOWhenAuthorize(Boolean open){
			shareSDKUtils.DisableSSOWhenAuthorize (open);
		}

	}
}