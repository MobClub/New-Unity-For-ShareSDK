package cn.sharesdk.unity3d;

import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;

import com.mob.MobSDK;
import com.mob.commons.MobProduct;
import com.mob.commons.SHARESDK;
import com.mob.tools.utils.Hashon;
import com.mob.tools.utils.UIHandler;
import com.unity3d.player.UnityPlayer;

import java.util.HashMap;
import java.util.Map;

import cn.sharesdk.framework.Platform;
import cn.sharesdk.framework.ShareSDK;
import cn.sharesdk.onekeyshare.OnekeyShare;
import cn.sharesdk.onekeyshare.ShareContentCustomizeCallback;


public class ShareSDKUtils implements Handler.Callback {

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

  public ShareSDKUtils(String gameObject, String callback) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.prepare");
    if (context == null)
      context = UnityPlayer.currentActivity.getApplicationContext();
    if (!TextUtils.isEmpty(gameObject))
      u3dGameObject = gameObject;
    if (!TextUtils.isEmpty(callback))
      u3dCallback = callback;
  }

  public void initSDK(String appKey, String screct) {
    if (DEBUG)
      System.out.println("initSDK appkey ==>>" + appKey + "appscrect ==>>" + screct);
    if (!TextUtils.isEmpty(appKey) && !TextUtils.isEmpty(screct)) {
      MobSDK.init(context, appKey, screct);
    } else if (!TextUtils.isEmpty(appKey)) {
      MobSDK.init(context, appKey);
    } else {
      MobSDK.init(context);
    }
  }

  public void prepareLoopShare() {
    Unity3dPlatformLoopShareResultListener un3dListener = new Unity3dPlatformLoopShareResultListener(u3dGameObject, u3dCallback);
    ShareSDK.prepareLoopShare(un3dListener);
    System.out.println("ShareSDKUtils prepareLoopShare set is ok");
  }


  public void setChannelId() {
    try {
      MobSDK.setChannel((MobProduct)new SHARESDK(), 2);
      System.out.println("ShareSDKUtils setChannelId set is ok");
    } catch (Throwable t) {
      System.out.println("ShareSDKUtils setChannelId error: " + t);
    }
  }


  public void setPlatformConfig(String configs) {
    if (DEBUG)
      System.out.println("initSDK configs ==>>" + configs);
    if (!TextUtils.isEmpty(configs)) {
      Message msg = new Message();
      msg.what = 1;
      msg.obj = configs;
      UIHandler.sendMessageDelayed(msg, 1000L, this);
    }
  }

  public void authorize(int reqID, int platform) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.authorize");
    Message msg = new Message();
    msg.what = 2;
    msg.arg1 = platform;
    msg.arg2 = reqID;
    UIHandler.sendMessage(msg, this);
  }

  public void removeAccount(int platform) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.removeAccount");
    String name = ShareSDK.platformIdToName(platform);
    Platform plat = ShareSDK.getPlatform(name);
    plat.removeAccount(true);
  }

  public boolean isAuthValid(int platform) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.isAuthValid");
    String name = ShareSDK.platformIdToName(platform);
    Platform plat = ShareSDK.getPlatform(name);
    return plat.isAuthValid();
  }

  public boolean isClientValid(int platform) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.isClientValid");
    String name = ShareSDK.platformIdToName(platform);
    Platform plat = ShareSDK.getPlatform(name);
    return plat.isClientValid();
  }

  public void showUser(int reqID, int platform) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.showUser");
    Message msg = new Message();
    msg.what = 3;
    msg.arg1 = platform;
    msg.arg2 = reqID;
    UIHandler.sendMessage(msg, this);
  }

  public void shareContent(int reqID, int platform, String content) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.share");
    Message msg = new Message();
    msg.what = 4;
    msg.arg1 = platform;
    msg.obj = content;
    msg.arg2 = reqID;
    UIHandler.sendMessage(msg, this);
  }

  public void onekeyShare(int reqID, int platform, String content) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.OnekeyShare");
    Message msg = new Message();
    msg.what = 5;
    msg.arg1 = platform;
    msg.obj = content;
    msg.arg2 = reqID;
    UIHandler.sendMessage(msg, this);
  }

  public void getFriendList(int reqID, int platform, int count, int page) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.getFriendList");
    Message msg = new Message();
    msg.what = 6;
    msg.arg1 = platform;
    msg.arg2 = reqID;
    Bundle data = new Bundle();
    data.putInt("page", page);
    data.putInt("count", count);
    msg.setData(data);
    UIHandler.sendMessage(msg, this);
  }

  public void followFriend(int reqID, int platform, String account) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.followFriend");
    Message msg = new Message();
    msg.what = 7;
    msg.arg1 = platform;
    msg.obj = account;
    msg.arg2 = reqID;
    UIHandler.sendMessage(msg, this);
  }

  public String getAuthInfo(int platform) {
    if (DEBUG)
      System.out.println("ShareSDKUtils.getAuthInfo");
    String name = ShareSDK.platformIdToName(platform);
    Platform plat = ShareSDK.getPlatform(name);
    if (plat == null)
      return "error:platform is invaild";
    Hashon hashon = new Hashon();
    HashMap<String, Object> map = new HashMap<>();
    if (plat.isAuthValid()) {
      map.put("expiresIn", Long.valueOf(plat.getDb().getExpiresIn()));
      map.put("expiresTime", Long.valueOf(plat.getDb().getExpiresTime()));
      map.put("token", plat.getDb().getToken());
      map.put("refresh_token", plat.getDb().get("refresh_token"));
      map.put("tokenSecret", plat.getDb().getTokenSecret());
      map.put("userGender", plat.getDb().getUserGender());
      map.put("userID", plat.getDb().getUserId());
      map.put("openID", plat.getDb().get("openid"));
      map.put("unionID", plat.getDb().get("unionid"));
      map.put("userName", plat.getDb().getUserName());
      map.put("userIcon", plat.getDb().getUserIcon());
    }
    return hashon.fromHashMap(map);
  }

  public void disableSSOWhenAuthorize(boolean open) {
    disableSSO = open;
  }

  public boolean handleMessage(Message msg) {
    String configs;
    int i;
    int platformID;
    int platform;
    Hashon hashon;
    Unity3dPlatformActionListener paListener;
    HashMap<String, Object> devInfo;
    String name;
    String content;
    int page;
    String account;
    Platform plat;
    String pName;
    Hashon hashon1;
    int count;
    String str1;
    Platform platform2;
    HashMap<String, Object> map;
    String str2;
    Platform platform1;
    OnekeyShare oks;
    Platform platform3;
    switch (msg.what) {
      case 1:
        if (DEBUG)
          System.out.println("ShareSDKUtils.setPlatformConfig");
        configs = (String)msg.obj;
        hashon = new Hashon();
        devInfo = hashon.fromJson(configs);
        ShareSDK.getPlatformList();
        for (Map.Entry<String, Object> entry : devInfo.entrySet()) {
          String p = ShareSDK.platformIdToName(Integer.parseInt(entry.getKey()));
          if (p != null) {
            if (DEBUG)
              System.out.println(String.valueOf(p) + " ==>>" + (new Hashon())
                  .fromHashMap((HashMap)entry.getValue()));
            ShareSDK.setPlatformDevInfo(p, (HashMap)entry.getValue());
          }
        }
        break;
      case 2:
        i = msg.arg1;
        paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
        paListener.setReqID(msg.arg2);
        name = ShareSDK.platformIdToName(i);
        plat = ShareSDK.getPlatform(name);
        if (plat != null) {
          plat.setPlatformActionListener(paListener);
          plat.SSOSetting(disableSSO);
          plat.authorize();
        }
        break;
      case 3:
        i = msg.arg1;
        paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
        paListener.setReqID(msg.arg2);
        name = ShareSDK.platformIdToName(i);
        plat = ShareSDK.getPlatform(name);
        if (plat != null) {
          plat.setPlatformActionListener(paListener);
          plat.SSOSetting(disableSSO);
          plat.showUser(null);
        }
        break;
      case 4:
        platformID = msg.arg1;
        paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
        paListener.setReqID(msg.arg2);
        content = (String)msg.obj;
        pName = ShareSDK.platformIdToName(platformID);
        platform2 = ShareSDK.getPlatform(pName);
        if (platform2 != null) {
          platform2.setPlatformActionListener(paListener);
          platform2.SSOSetting(disableSSO);
          try {
            Hashon hashon2 = new Hashon();
            if (DEBUG)
              System.out.println("share content ==>>" + content);
            HashMap<String, Object> data = hashon2.fromJson(content);
            Platform.ShareParams sp = new Platform.ShareParams(data);
            if (data.containsKey("customizeShareParams")) {
              final HashMap<String, String> customizeSP = (HashMap<String, String>)data.get("customizeShareParams");
              if (customizeSP.size() > 0) {
                String pID = String.valueOf(platformID);
                if (customizeSP.containsKey(pID)) {
                  String cSP = customizeSP.get(pID);
                  if (DEBUG)
                    System.out.println("share content ==>>" + cSP);
                  data = hashon2.fromJson(cSP);
                  for (String key : data.keySet())
                    sp.set(key, data.get(key));
                }
              }
            }
            platform2.share(sp);
          } catch (Throwable t) {
            paListener.onError(platform2, 9, t);
          }
        }
        break;
      case 5:
        platform = msg.arg1;
        paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
        paListener.setReqID(msg.arg2);
        content = (String)msg.obj;
        hashon1 = new Hashon();
        if (DEBUG)
          System.out.println("onekeyshare  ==>>" + content);
        map = hashon1.fromJson(content);
        oks = new OnekeyShare();
        if (platform > 0) {
          String str = ShareSDK.platformIdToName(platform);
          if (DEBUG)
            System.out.println("ShareSDKUtils Onekeyshare shareView platform name ==>> " + str);
          if (!TextUtils.isEmpty(str)) {
            oks.setPlatform(str);
            oks.setSilent(false);
          }
        }
        if (map.containsKey("hidePlatformList")) {
          String hidePlatformList = (String)map.get("hidePlatformList");
          String[] stringList = hidePlatformList.split(",");
          if (!TextUtils.isEmpty(hidePlatformList) && stringList != null && stringList.length > 0)
            for (int j = 0; j < stringList.length; j++) {
              int platformId = Integer.parseInt(stringList[j]);
              oks.addHiddenPlatform(ShareSDK.platformIdToName(platformId));
            }
        }
        if (map.containsKey("text"))
          oks.setText((String)map.get("text"));
        if (map.containsKey("imagePath"))
          oks.setImagePath((String)map.get("imagePath"));
        if (map.containsKey("imageUrl"))
          oks.setImageUrl((String)map.get("imageUrl"));
        if (map.containsKey("imageArray")) {
          String imageString = (String)map.get("imageArray");
          String[] imageArray = imageString.split(",");
          oks.setImageArray(imageArray);
        }
        if (map.containsKey("title"))
          oks.setTitle((String)map.get("title"));
        if (map.containsKey("comment"))
          oks.setComment((String)map.get("comment"));
        if (map.containsKey("url"))
          oks.setUrl((String)map.get("url"));
        if (map.containsKey("titleUrl"))
          oks.setTitleUrl((String)map.get("titleUrl"));
        if (map.containsKey("site"))
          oks.setSite((String)map.get("site"));
        if (map.containsKey("siteUrl"))
          oks.setSiteUrl((String)map.get("siteUrl"));
        if (map.containsKey("musicUrl"))
          oks.setMusicUrl((String)map.get("musicUrl"));
        if (map.containsKey("filePath"))
          oks.setFilePath((String)map.get("filePath"));
        if (map.containsKey("shareType") && "6"
            .equals(String.valueOf(map.get("shareType"))) && map
            .containsKey("url"))
          oks.setVideoUrl((String)map.get("url"));
        if (map.containsKey("customizeShareParams")) {
          final HashMap<String, String> customizeSP = (HashMap<String, String>)map.get("customizeShareParams");
          if (customizeSP.size() > 0)
            oks.setShareContentCustomizeCallback(new ShareContentCustomizeCallback() {
              public void onShare(Platform platform, Platform.ShareParams paramsToShare) {
                String platformID = String.valueOf(ShareSDK.platformNameToId(platform.getName()));
                if (customizeSP.containsKey(platformID)) {
                  Hashon hashon = new Hashon();
                  String content = (String)customizeSP.get(platformID);
                  if (ShareSDKUtils.DEBUG)
                    System.out.println("share content ==>>" + content);
                  HashMap<String, Object> data = hashon.fromJson(content);
                  for (String key : data.keySet())
                    paramsToShare.set(key, data.get(key));
                }
              }
            });
        }
        if (disableSSO)
          oks.disableSSOWhenAuthorize();
        oks.setCallback(paListener);
        oks.show(context);
        break;
      case 6:
        platform = msg.arg1;
        paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
        paListener.setReqID(msg.arg2);
        page = msg.getData().getInt("page");
        count = msg.getData().getInt("count");
        str2 = ShareSDK.platformIdToName(platform);
        platform3 = ShareSDK.getPlatform(str2);
        if (platform3 != null) {
          platform3.setPlatformActionListener(paListener);
          platform3.SSOSetting(disableSSO);
          platform3.listFriend(count, page, null);
        }
        break;
      case 7:
        platform = msg.arg1;
        paListener = new Unity3dPlatformActionListener(u3dGameObject, u3dCallback);
        paListener.setReqID(msg.arg2);
        account = (String)msg.obj;
        str1 = ShareSDK.platformIdToName(platform);
        platform1 = ShareSDK.getPlatform(str1);
        if (platform1 != null) {
          platform1.setPlatformActionListener(paListener);
          platform1.SSOSetting(disableSSO);
          platform1.followFriend(account);
        }
        break;
    }
    return false;
  }

}