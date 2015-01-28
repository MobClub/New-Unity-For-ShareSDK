package cn.sharesdk.unity3d;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map.Entry;

import m.framework.utils.Hashon;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler.Callback;
import android.os.Message;
import android.text.TextUtils;
import cn.sharesdk.framework.Platform;
import cn.sharesdk.framework.Platform.ShareParams;
import cn.sharesdk.framework.PlatformActionListener;
import cn.sharesdk.framework.ShareSDK;
import cn.sharesdk.framework.utils.UIHandler;
import cn.sharesdk.onekeyshare.OnekeyShare;

import com.unity3d.player.UnityPlayer;

public class ShareSDKUtils {
	private static boolean DEBUG = true;
	private static boolean disableSSO = false; 
	
	private static final int MSG_INITSDK = 1;
	private static final int MSG_AUTHORIZE = 2;
	private static final int MSG_SHOW_USER = 3;
	private static final int MSG_SHARE = 4;
	private static final int MSG_ONEKEY_SAHRE = 5;
	private static final int MSG_GET_FRIENDLIST = 6;
	
	private static Context context;
	private static Callback uiCallback;
	private static PlatformActionListener paListener;
	
	public static void prepare(final String gameObject,final String callback) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.prepare");
		}
		if (context == null) {
			context = UnityPlayer.currentActivity.getApplicationContext();
		}
		if (uiCallback == null) {
			uiCallback = new Callback() {
				public boolean handleMessage(Message msg) {
					return ShareSDKUtils.handleMessage(msg);
				}
			};
		}
		if (paListener == null) {
			paListener = new PlatformActionListener() {
				public void onError(Platform platform, int action, Throwable t) {
					String resp = javaActionResToCS(platform, action, t);
					UnityPlayer.UnitySendMessage(gameObject, callback, resp);
				}
				
				public void onComplete(Platform platform, int action,
						HashMap<String, Object> res) {
					String resp = javaActionResToCS(platform, action, res);
					UnityPlayer.UnitySendMessage(gameObject, callback, resp);
				}
				
				public void onCancel(Platform platform, int action) {
					String resp = javaActionResToCS(platform, action);
					UnityPlayer.UnitySendMessage(gameObject, callback, resp);
				}
			};
		}
	}
	
	public static void initSDK(String appKey) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.initSDK");
		}
		
		Message msg = new Message();
		msg.what = MSG_INITSDK;
		msg.obj = appKey;
		UIHandler.sendMessage(msg, uiCallback);
	}
	
	public static void setPlatformConfig(int platform, String configs) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.setPlatformConfig");
		}
		Hashon hashon = new Hashon();
		HashMap<String, Object> devInfo = hashon.fromJson(configs);
		String p = ShareSDK.platformIdToName(platform);
		ShareSDK.setPlatformDevInfo(p, devInfo);
	}
	
	public static void authorize(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.authorize");
		}
		Message msg = new Message();
		msg.what = MSG_AUTHORIZE;
		msg.arg1 = platform;
		UIHandler.sendMessage(msg, uiCallback);
	}
	
	public static void removeAccount(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.removeAccount");
		}
		String name = ShareSDK.platformIdToName(platform);
		Platform plat = ShareSDK.getPlatform(context, name);
		plat.removeAccount(true);
	}
	
	public static boolean isValid(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.isValid");
		}
		String name = ShareSDK.platformIdToName(platform);
		Platform plat = ShareSDK.getPlatform(context, name);
		return plat.isValid();
	}
	
	public static void showUser(int platform) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.showUser");
		}
		Message msg = new Message();
		msg.what = MSG_SHOW_USER;
		msg.arg1 = platform;
		UIHandler.sendMessage(msg, uiCallback);
	}
	
	public static void share(int platform, String content) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.share");
		}
		Message msg = new Message();
		msg.what = MSG_SHARE;
		msg.arg1 = platform;
		msg.obj = content;
		UIHandler.sendMessage(msg, uiCallback);
	}
	
	public static void onekeyShare(int platform, String content) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.OnekeyShare");
		}
		Message msg = new Message();
		msg.what = MSG_ONEKEY_SAHRE;
		msg.arg1 = platform;
		msg.obj = content;
		UIHandler.sendMessage(msg, uiCallback);
	}

	public static void getFriendList(int platform, int count, int page) {
		if (DEBUG) {
			System.out.println("ShareSDKUtils.getFriendList");
		}
		Message msg = new Message();
		msg.what = MSG_GET_FRIENDLIST;
		msg.arg1 = platform;
		Bundle data = new Bundle();
		data.putInt("page", page);
		data.putInt("count", count);
		msg.setData(data);
		UIHandler.sendMessage(msg, uiCallback);
	}

	public static String getAuthInfo(int platform) {
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
			map.put("gender", plat.getDb().getUserGender());
			map.put("userIcon", plat.getDb().getUserIcon());
			map.put("userID", plat.getDb().getUserId());
			map.put("userName", plat.getDb().getUserName());
		}
		return hashon.fromHashMap(map);
	}
	
	public static void disableSSOWhenAuthorize(boolean open){
		disableSSO = open;
	}
	
	public static boolean handleMessage(Message msg) {
		switch (msg.what) {
			case MSG_INITSDK: {
				if (DEBUG) {
					System.out.println("handleMessage MSG_INITSDK appkey ==>>" + (String) msg.obj);
				}
				if (msg.obj != null) {
					String appKey = (String) msg.obj;
					ShareSDK.initSDK(context, appKey);
				} else {
					ShareSDK.initSDK(context);
				}
			}
			break;
			case MSG_AUTHORIZE: {
				int platform = msg.arg1;
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.authorize();
			}
			break;
			case MSG_SHOW_USER: {
				int platform = msg.arg1;
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.showUser(null);
			}
			break;
			case MSG_SHARE: {
				int platform = msg.arg1;
				String content = (String) msg.obj;
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				try {
					Hashon hashon = new Hashon();
					ShareParams sp = hashmapToShareParams(plat, hashon.fromJson(content));
					plat.share(sp);
				} catch (Throwable t) {
					paListener.onError(plat, Platform.ACTION_SHARE, t);
				}
			}
			break;
			case MSG_ONEKEY_SAHRE: {
				int platform = msg.arg1;
				String content = (String) msg.obj;
				Hashon hashon = new Hashon();
				HashMap<String, Object> map = CSMapToJavaMap(hashon.fromJson(content));
				OnekeyShare oks = new OnekeyShare();
				oks.disableSSOWhenAuthorize();
				if (platform > 0) {
					String name = ShareSDK.platformIdToName(platform);
					if(!TextUtils.isEmpty(name)){
						oks.setPlatform(name);
					}
				}
				if (map.containsKey("text")) {
					oks.setText(String.valueOf(map.get("text")));
				}
				if (map.containsKey("imagePath")) {
					oks.setImagePath(String.valueOf(map.get("imagePath")));
				}
				if (map.containsKey("imageUrl")) {
					oks.setImageUrl(String.valueOf(map.get("imageUrl")));
				}
				if (map.containsKey("title")) {
					oks.setTitle(String.valueOf(map.get("title")));
				}
				if (map.containsKey("comment")) {
					oks.setComment(String.valueOf(map.get("comment")));
				}
				if (map.containsKey("url")) {
					oks.setUrl(String.valueOf(map.get("url")));
					oks.setTitleUrl(String.valueOf(map.get("url")));
				}
				if (map.containsKey("site")) {
					oks.setSite(String.valueOf(map.get("site")));
				}
				if (map.containsKey("siteUrl")) {
					oks.setSiteUrl(String.valueOf(map.get("siteUrl")));
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
				int page = msg.getData().getInt("page");
				int count = msg.getData().getInt("count");
				String name = ShareSDK.platformIdToName(platform);
				Platform plat = ShareSDK.getPlatform(context, name);
				plat.setPlatformActionListener(paListener);
				plat.SSOSetting(disableSSO);
				plat.listFriend(count, page, null);
			}
			break;
		}
		return false;
	}
	
	// ==================== java tools =====================
	
	private static String javaActionResToCS(Platform platform, int action, Throwable t) {
		int platformId = ShareSDK.platformNameToId(platform.getName());
		HashMap<String, Object> map = new HashMap<String, Object>();
		map.put("platform", platformId);
		map.put("action", action);
		map.put("status", 2); // Success = 1, Fail = 2, Cancel = 3
		map.put("res", throwableToMap(t));
		Hashon hashon = new Hashon();
		return hashon.fromHashMap(map);
	}
	
	private static String javaActionResToCS(Platform platform, int action, HashMap<String, Object> res) {
		int platformId = ShareSDK.platformNameToId(platform.getName());
		HashMap<String, Object> map = new HashMap<String, Object>();
		map.put("platform", platformId);
		map.put("action", action);
		map.put("status", 1); // Success = 1, Fail = 2, Cancel = 3
		map.put("res", res);
		Hashon hashon = new Hashon();
		return hashon.fromHashMap(map);
	}
	
	private static String javaActionResToCS(Platform platform, int action) {
		int platformId = ShareSDK.platformNameToId(platform.getName());
		HashMap<String, Object> map = new HashMap<String, Object>();
		map.put("platform", platformId);
		map.put("action", action);
		map.put("status", 3); // Success = 1, Fail = 2, Cancel = 3
		Hashon hashon = new Hashon();
		return hashon.fromHashMap(map);
	}
	
	private static HashMap<String, Object> throwableToMap(Throwable t) {
		HashMap<String, Object> map = new HashMap<String, Object>();
		map.put("msg", t.getMessage());
		ArrayList<HashMap<String, Object>> traces = new ArrayList<HashMap<String, Object>>();
		for (StackTraceElement trace : t.getStackTrace()) {
			HashMap<String, Object> element = new HashMap<String, Object>();
			element.put("cls", trace.getClassName());
			element.put("method", trace.getMethodName());
			element.put("file", trace.getFileName());
			element.put("line", trace.getLineNumber());
			traces.add(element);
		}
		map.put("stack", traces);
		Throwable cause = t.getCause();
		if (cause != null) {
			map.put("cause", throwableToMap(cause));
		}
		return map;
	}
	
	private static ShareParams hashmapToShareParams(Platform plat, 
			HashMap<String, Object> content) throws Throwable {
		String className = plat.getClass().getName() + "$ShareParams";
		Class<?> cls = Class.forName(className);
		if (cls == null) {
			return null;
		}
		
		Object sp = cls.newInstance();
		if (sp == null) {
			return null;
		}
		
		HashMap<String, Object> data = CSMapToJavaMap(content);
		for (Entry<String, Object> ent : data.entrySet()) {
			try {
				Field fld = cls.getField(ent.getKey());
				if (fld != null) {
					fld.setAccessible(true);
					fld.set(sp, ent.getValue());
				}
			} catch(Throwable t) {}
		}
		
		return (Platform.ShareParams) sp;
	}
	
	private static HashMap<String, Object> CSMapToJavaMap(HashMap<String, Object> content) {
		HashMap<String, Object> map = new HashMap<String, Object>();
		map.put("text", content.get("content"));
		String image = String.valueOf(content.get("image"));
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
		if (type != null) {
			int shareType = iosTypeToAndroidType(Integer.parseInt(type));
			map.put("shareType", shareType);
		}
		map.put("filePath", content.get("file"));
		map.put("siteUrl", content.get("siteUrl"));
		map.put("site", content.get("site"));
		map.put("musicUrl", content.get("musicUrl"));
		map.put("extInfo", content.get("extInfo"));
		return map;
	}
	
	private static int iosTypeToAndroidType(int type) {
		switch (type) {
			case 1: return Platform.SHARE_IMAGE;
			case 2: return Platform.SHARE_WEBPAGE;
			case 3: return Platform.SHARE_MUSIC;
			case 4: return Platform.SHARE_VIDEO;
			case 5: return Platform.SHARE_APPS;
			case 6: 
			case 7: return Platform.SHARE_EMOJI;
			case 8: return Platform.SHARE_FILE;
		}
        return Platform.SHARE_TEXT;
	}
	
}
