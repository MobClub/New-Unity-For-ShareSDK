 package cn.sharesdk.unity3d;

 import com.mob.tools.utils.Hashon;
 import com.unity3d.player.UnityPlayer;

 import java.util.ArrayList;
 import java.util.HashMap;

 import cn.sharesdk.framework.Platform;
 import cn.sharesdk.framework.PlatformActionListener;
 import cn.sharesdk.framework.ShareSDK;


 public class Unity3dPlatformActionListener implements PlatformActionListener
 {
   private int reqID;

   private String u3dCallback;

   private String u3dGameObject;

   public Unity3dPlatformActionListener(String u3dGameObject, String u3dCallback) {
     this.u3dGameObject = u3dGameObject;
     this.u3dCallback = u3dCallback;
   }

   public void setReqID(int reqID) {
     this.reqID = reqID;
   }

   public void onError(Platform platform, int action, Throwable t) {
     t.printStackTrace();
     String resp = javaActionResToCS(platform, action, t);
     UnityPlayer.UnitySendMessage(this.u3dGameObject, this.u3dCallback, resp);
   }

   public void onComplete(Platform platform, int action, HashMap<String, Object> res) {
     String resp = javaActionResToCS(platform, action, res);
     UnityPlayer.UnitySendMessage(this.u3dGameObject, this.u3dCallback, resp);
   }

   public void onCancel(Platform platform, int action) {
     String resp = javaActionResToCS(platform, action);
     UnityPlayer.UnitySendMessage(this.u3dGameObject, this.u3dCallback, resp);
   }

   private String javaActionResToCS(Platform platform, int action, Throwable t) {
     int platformId = ShareSDK.platformNameToId(platform.getName());
     HashMap<String, Object> map = new HashMap<>();
     map.put("reqID", Integer.valueOf(this.reqID));
     map.put("platform", Integer.valueOf(platformId));
     map.put("action", Integer.valueOf(action));
     map.put("status", Integer.valueOf(2));
     map.put("res", throwableToMap(t));
     Hashon hashon = new Hashon();
     return hashon.fromHashMap(map);
   }

   private String javaActionResToCS(Platform platform, int action, HashMap<String, Object> res) {
     int platformId = ShareSDK.platformNameToId(platform.getName());
     HashMap<String, Object> map = new HashMap<>();
     map.put("reqID", Integer.valueOf(this.reqID));
     map.put("platform", Integer.valueOf(platformId));
     map.put("action", Integer.valueOf(action));
     map.put("status", Integer.valueOf(1));
     map.put("res", res);
     Hashon hashon = new Hashon();
     return hashon.fromHashMap(map);
   }

   private String javaActionResToCS(Platform platform, int action) {
     int platformId = ShareSDK.platformNameToId(platform.getName());
     HashMap<String, Object> map = new HashMap<>();
     map.put("reqID", Integer.valueOf(this.reqID));
     map.put("platform", Integer.valueOf(platformId));
     map.put("action", Integer.valueOf(action));
     map.put("status", Integer.valueOf(3));
     Hashon hashon = new Hashon();
     return hashon.fromHashMap(map);
   }

   private HashMap<String, Object> throwableToMap(Throwable t) {
     HashMap<Object, Object> map = new HashMap<Object, Object>();
     map.put("msg", t.getMessage());
     ArrayList<HashMap<Object, Object>> traces = new ArrayList(); byte b; int i; StackTraceElement[] arrayOfStackTraceElement;
     for (i = (arrayOfStackTraceElement = t.getStackTrace()).length, b = 0; b < i; ) { StackTraceElement trace = arrayOfStackTraceElement[b];
       HashMap<Object, Object> element = new HashMap<Object, Object>();
       element.put("cls", trace.getClassName());
       element.put("method", trace.getMethodName());
       element.put("file", trace.getFileName());
       element.put("line", Integer.valueOf(trace.getLineNumber()));
       traces.add(element); b++; }

     map.put("stack", traces);
     Throwable cause = t.getCause();
     if (cause != null) {
       map.put("cause", throwableToMap(cause));
     }
     return (HashMap)map;
   }
 }