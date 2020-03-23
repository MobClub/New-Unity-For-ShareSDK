using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cn.sharesdk.unity3d
{
#if UNITY_ANDROID
	public class AndroidMobSDKImpl : MobSDKImpl {
		private AndroidJavaObject javaObj;

		public AndroidMobSDKImpl (GameObject go) {
			Debug.Log("AndroidMobSDKImpl  ===>>>  AndroidMobSDKImpl" );
			try{
				javaObj = new AndroidJavaObject("cn.sharesdk.unity3d.MobSDKUtils", go.name, "_PolicyGrantResultCallback");
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}

		public override string getPrivacyPolicy (bool url) {
			Debug.Log("AndroidMobSDKImpl  ===>>>  getPrivacyPolicy === ");
			if(javaObj != null){
				String str = javaObj.Call<string> ("getPrivacyPolicy", url);
				return str;
			}
			return "No get privacypolicy content";
		}

		public override void submitPolicyGrantResult (bool granted) {
			Debug.Log("AndroidMobSDKImpl  ===>>>  submitPolicyGrantResult === ");
			if(javaObj != null){
				javaObj.Call ("submitPolicyGrantResult", granted);
			}
		}

		public override void setAllowDialog (bool allowDialog) {
			Debug.Log("AndroidMobSDKImpl  ===>>>  setAllowDialog === ");
			if(javaObj != null){
				javaObj.Call ("setAllowDialog", allowDialog);
			}
		}

		public override void setPolicyUi (String backgroundColorRes, String positiveBtnColorRes, String negativeBtnColorRes) {
			Debug.Log("AndroidMobSDKImpl  ===>>>  setPolicyUi === ");
			if(javaObj != null){
				javaObj.Call ("setPolicyUi", backgroundColorRes, positiveBtnColorRes, negativeBtnColorRes);
			}
		}
	}
#endif
}