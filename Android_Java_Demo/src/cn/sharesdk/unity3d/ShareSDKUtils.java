package cn.sharesdk.unity3d;

import java.util.HashMap;
import java.util.Map.Entry;

import android.content.Context;
import android.os.Bundle;
import android.os.Handler.Callback;
import android.os.Message;
import android.text.TextUtils;
import cn.sharesdk.framework.Platform;
import cn.sharesdk.framework.Platform.ShareParams;
import cn.sharesdk.framework.ShareSDK;
import cn.sharesdk.onekeyshare.OnekeyShare;

import com.mob.tools.utils.Hashon;
import com.mob.tools.utils.UIHandler;
import com.unity3d.player.UnityPlayer;

public class ShareSDKUtils implements Callback{
	private static boolean DEBUG = true;
	private static boolean disableSSO = false; 
	
	private static final int MSG_INITSDK = 1;
	private static final int MSG_AUTHORIZE = 2;
	private static final int MSG_SHOW_USER = 3;
	private static final int MSG_SHARE = 4;
	private static final int MSG_ONEKEY_SAHRE = 5;
	private static final int MSG_GET_FRIENDLIST = 6;
	private static final int MSG_FOLLOW_FRIEND = 7;
	
	private static Context context;
	private static String u3dGameObject;
	private static String u3dCallback;
		
	public ShareSDKUtils(final String gameObject,final String callback) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.prepare");
		}
		if (context == null) {
			context = UnityPlayer.currentActivity.getApplicationContext();
		}
				
		if(!TextUtils.isEmpty(gameObject)) {
			u3dGameObject = gameObject;
		}
		
		if(!TextUtils.isEmpty(callback)) {
			u3dCallback = callback;
		}	
	}
	
	public void initSDKAndSetPlatfromConfig(String appKey, String configs) {
		if (DEBUG) {
			System.out.println("initSDK appkey ==>>" + appKey);
			System.out.println("initSDK configs ==>>" + configs);
		}
		if (!TextUtils.isEmpty(appKey)) {
			ShareSDK.initSDK(context, appKey);
		} else {
			ShareSDK.initSDK(context);
		}
		
		if (!TextUtils.isEmpty(configs)) {
			Message msg = new Message();
			msg.what = MSG_INITSDK;
			msg.obj = configs;
			UIHandler.sendMessageDelayed(msg, 1000, this);
		}
	}
	
	public void authorize(int reqID, int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.authorize");
		}
		Message msg = new Message();
		msg.what = MSG_AUTHORIZE;
		msg.arg1 = platform;
		msg.arg2 = reqID;
		UIHandler.sendMessage(msg, this);
	}
	
	public void removeAccount(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.removeAccount");
		}
		String name = ShareSDK.platformIdToName(platform);
		Platform plat = ShareSDK.getPlatform(context, name);
		plat.removeAccount(true);
	}
	
	public boolean isAuthValid(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.isAuthValid");
		}
		String name = ShareSDK.platformIdToName(platform);
		Platform plat = ShareSDK.getPlatform(context, name);
		return plat.isAuthValid();
	}
	
	public boolean isClientValid(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.isClientValid");
		}
		String name = ShareSDK.platformIdToName(platform);
		Platform plat = ShareSDK.getPlatform(context, name);
		return plat.isClientValid();
	}
	
	public void showUser(int reqID, int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.showUser");
		}
		Message msg = new Message();
		msg.what = MSG_SHOW_USER;
		msg.arg1 = platform;
		msg.arg2 = reqID;
		UIHandler.sendMessage(msg, this);
	}
	
	public void shareContent(int reqID, int platform, String content) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.share");
		}
		Message msg = new Message();
		msg.what = MSG_SHARE;
		msg.arg1 = platform;
		msg.obj = content;
		msg.arg2 = reqID;
		UIHandler.sendMessage(msg, this);
	}
	
	public void onekeyShare(int reqID, int platform, String content) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.OnekeyShare");
		}
		Message msg = new Message();
		msg.what = MSG_ONEKEY_SAHRE;
		msg.arg1 = platform;
		msg.obj = content;
		msg.arg2 = reqID;
		UIHandler.sendMessage(msg, this);
	}

	public void getFriendList(int reqID, int platform, int count, int page) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.getFriendList");
		}
		Message msg = new Message();
		msg.what = MSG_GET_FRIENDLIST;
		msg.arg1 = platform;
		msg.arg2 = reqID;
		Bundle data = new Bundle();
		data.putInt("page", page);
		data.putInt("count", count);
		msg.setData(data);
		UIHandler.sendMessage(msg, this);
	}
	
	public void followFriend(int reqID, int platform, String account) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.followFriend");
		}
		
		Message msg = new Message();
		msg.what = MSG_FOLLOW_FRIEND;
		msg.arg1 = platform;
		msg.obj = account;
		msg.arg2 = reqID; 
		UIHandler.sendMessage(msg, this);
	}

	public String getAuthInfo(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.getAuthInfo");
		}
		
		String name = ShareSDK.platformIdToName(platform);
		Platform plat = ShareSDK.getPlatform(context, name);
		Hashon hashon = new Hashon();
		HashMap<String, Object> map = new HashMap<String, Object>();
		if(plat.isValid()){
			map.put("expiresIn", plat.getDb().getExpiresIn());
			map.put("expiresTime", plat.getDb().getExpiresTime());
			map.put("token", plat.getDb().getToken());
			map.put("tokenSecret", plat.getDb().getTokenSecret());
			map.put("userGender", plat.getDb().getUserGender());
			map.put("userID", plat.getDb().getUserId());
			map.put("openID", plat.getDb().get("openid"));
			map.put("userName", plat.getDb().getUserName());
			map.put("userIcon", plat.getDb().getUserIcon());
		}
		return hashon.fromHashMap(map);
	}
	
	public void disableSSOWhenAuthorize(boolean open){
		disableSSO = open;
	}
	
	@SuppressWarnings("unchecked")
	public boolean handleMessage(Message msg) {
		switch (msg.what) {			
			case MSG_INITSDK: {
				if (DEBUG) {
					System.out.println("ShareSDKUtils.setPlatformConfig");
				}
				String configs = (String) msg.obj;
				Hashon hashon = new Hashon();
				HashMap<String, Object> devInfo = hashon.fromJson(configs);
				for(Entry<String, Object> entry: devInfo.entrySet()){
					String p = ShareSDK.platformIdToName(Integer.parseInt(entry.getKey()));
					//System.out.println(p + " ==>>" + new Hashon().fromHashMap((HashMap<String, Object>)entry.getValue()));
					ShareSDK.setPlatformDevInfo(p, (HashMap<String, Object>)entry.getValue());
				}
			}
			break;
			case MSG_AUTHORIZE: {
				int platform = msg.arg1;
				Unity3dPlatformActionListener paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
				paListener.setReqID(msg.arg2);
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.authorize();
			}
			break;
			case MSG_SHOW_USER: {
				int platform = msg.arg1;
				Unity3dPlatformActionListener paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
				paListener.setReqID(msg.arg2);
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.showUser(null);
			}
			break;
			case MSG_SHARE: {
				int platform = msg.arg1;
				Unity3dPlatformActionListener paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
				paListener.setReqID(msg.arg2);
				String content = (String) msg.obj;
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				try {
					Hashon hashon = new Hashon();
					if (DEBUG) {
						System.out.println("share content ==>>" + content);
					}
					HashMap<String, Object> data = CSMapToJavaMap(hashon.fromJson(content));
					ShareParams sp = new ShareParams(data);
					plat.share(sp);
				} catch (Throwable t) {
					paListener.onError(plat, Platform.ACTION_SHARE, t);
				}
			}
			break;
			case MSG_ONEKEY_SAHRE: {
				int platform = msg.arg1;
				Unity3dPlatformActionListener paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
				paListener.setReqID(msg.arg2);
				String content = (String) msg.obj;
				Hashon hashon = new Hashon();
				if (DEBUG) {
					System.out.println("onekeyshare  ==>>" + content);
				}
				HashMap<String, Object> map = CSMapToJavaMap(hashon.fromJson(content));
				OnekeyShare oks = new OnekeyShare();
				if (platform > 0) {
					String name = ShareSDK.platformIdToName(platform);
					if (DEBUG) {
						System.out.println("ShareSDKUtils Onekeyshare shareView platform name ==>> " + name);
					}
					if(!TextUtils.isEmpty(name)){
						oks.setPlatform(name);
						oks.setSilent(false);
					}
				}
				if (map.containsKey("text")) {
					oks.setText((String)map.get("text"));
				}
				if (map.containsKey("imagePath")) {
					oks.setImagePath((String)map.get("imagePath"));
				}
				if (map.containsKey("imageUrl")) {
					oks.setImageUrl((String)map.get("imageUrl"));
				}
				if (map.containsKey("title")) {
					oks.setTitle((String)map.get("title"));
				}
				if (map.containsKey("comment")) {
					oks.setComment((String)map.get("comment"));
				}
				if (map.containsKey("url")) {
					oks.setUrl((String)map.get("url"));
					oks.setTitleUrl((String)map.get("url"));
				}
				if (map.containsKey("site")) {
					oks.setSite((String)map.get("site"));
				}
				if (map.containsKey("siteUrl")) {
					oks.setSiteUrl((String)map.get("siteUrl"));
				}
				if (map.containsKey("musicUrl")) {
					oks.setSiteUrl((String)map.get("musicUrl"));
				}
				if (map.containsKey("shareType")) {
					if ("6".equals(String.valueOf(map.get("shareType")))) {
						if (map.containsKey("url")) {
							oks.setVideoUrl((String)map.get("url"));
						}
					}
				}
				
				if(disableSSO){
					oks.disableSSOWhenAuthorize();
				}
				oks.setCallback(paListener);
				oks.show(context);
			}
			break;
			case MSG_GET_FRIENDLIST:{
				int platform = msg.arg1;
				Unity3dPlatformActionListener paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
				paListener.setReqID(msg.arg2);
				int page = msg.getData().getInt("page");
				int count = msg.getData().getInt("count");
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.listFriend(count, page, null);
			}
			break;
			case MSG_FOLLOW_FRIEND:{
				int platform = msg.arg1;
				Unity3dPlatformActionListener paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
				paListener.setReqID(msg.arg2);
				String account = (String) msg.obj;
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.followFriend(account);
			}
			break;
		}
		return false;
	}
	
	private HashMap<String, Object> CSMapToJavaMap(HashMap<String, Object> content) {
		HashMap<String, Object> map = new HashMap<String, Object>();
		map.put("text", content.get("content"));
		String image = (String)content.get("image");
		if (!TextUtils.isEmpty(image) && image.startsWith("/")) {
			map.put("imagePath", image);
		} else if(!TextUtils.isEmpty(image)){
			map.put("imageUrl", image);
		}
		map.put("title", content.get("title"));
		map.put("comment", content.get("description"));
		map.put("url", content.get("url"));
		map.put("titleUrl", content.get("url"));
		String type = (String) content.get("type");
		if (DEBUG) {
			System.out.println("share content type==>>" + type);
		}
		if (type != null) {
			int shareType = Integer.parseInt(type) ;
			shareType = shareType == 0 ? 1 :shareType;//ios的自动类型，改成Text
			map.put("shareType", shareType);
		}
		map.put("shareTheme", content.get("shareTheme"));
		map.put("filePath", content.get("file"));
		map.put("siteUrl", content.get("siteUrl"));
		map.put("site", content.get("site"));
		map.put("musicUrl", content.get("musicUrl"));
		map.put("extInfo", content.get("extInfo"));
		
		return map;
	}
	
}
