package cn.sharesdk.unity3d;

import android.content.Intent;
import android.util.Log;

import com.mob.MobSDK;
import com.mob.tools.utils.Hashon;
import com.unity3d.player.UnityPlayer;

import java.io.PrintWriter;
import java.io.StringWriter;
import java.util.ArrayList;
import java.util.HashMap;

import cn.sharesdk.framework.loopshare.LoopShareResultListener;

public class Unity3dPlatformLoopShareResultListener implements LoopShareResultListener {

    private String u3dCallback;

    private String u3dGameObject;

    public Unity3dPlatformLoopShareResultListener(String u3dGameObject, String u3dCallback) {
        this.u3dGameObject = u3dGameObject;
        this.u3dCallback = u3dCallback;
        System.out.print(" Unity3dPlatformLoopShareResultListener  u3dGameObject: " + u3dGameObject + " u3dCallback: " + u3dCallback);
        Log.e("QQQ", "Unity3dPlatformLoopShareResultListener construction method is ok  u3dGameObject: "
            + u3dGameObject + " u3dCallback:  " + u3dCallback);
    }

    public Unity3dPlatformLoopShareResultListener() {

    }

    public void onResult(Object var1) {
        try {
//            Intent intent = new Intent();
//            intent.setClass(MobSDK.getContext(), MobUnityPlayerActivity.class);
//            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
//            MobSDK.getContext().startActivity(intent);

            Intent intent = new Intent();
            intent.setClass(MobSDK.getContext(), com.unity3d.player.UnityPlayerActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            MobSDK.getContext().startActivity(intent);

            String resp = (new Hashon()).fromHashMap((HashMap) var1);

            System.out.println("Unity3dPlatformLoopShareResultListener: onResult: " + resp);
            Log.e("QQQ", "Unity3dPlatformLoopShareResultListener onResult: " + resp);

            //new add
          //UnityPlayer.UnitySendMessage(this.u3dGameObject, this.u3dCallback, resp);


            UnityPlayer.UnitySendMessage("ShareSDKRestoreScene", "_RestoreCallBack", resp);

            System.out.println("Unity3dPlatformLoopShareResultListener: onResult: UnitySendMessage finish");
            Log.e("QQQ", "Unity3dPlatformLoopShareResultListener onResult:  UnitySendMessage finish");
        } catch (Throwable t) {
            System.out.println("Unity3dPlatformLoopShareResultListener onResult catch " + t);
            Log.e("QQQ", "Unity3dPlatformLoopShareResultListener onResult: Throwable " + t);
        }
    }

    public void onError(Throwable var1) {
        try {
            String error = getErrorInfoFromException(var1);
            System.out.println("Unity3dPlatformLoopShareResultListener: onError: " + error);
            Log.e("QQQ", "Unity3dPlatformLoopShareResultListener onError: " + error);

            UnityPlayer.UnitySendMessage("ShareSDKRestoreScene", "_RestoreCallBack", error);

            System.out.println("Unity3dPlatformLoopShareResultListener: onError: UnitySendMessage finish");
            Log.e("QQQ", "Unity3dPlatformLoopShareResultListener onError:  UnitySendMessage finish");
        } catch (Throwable t) {
            System.out.println("Unity3dPlatformLoopShareResultListener onError catch " + t);
            Log.e("QQQ", "Unity3dPlatformLoopShareResultListener onError: Throwable " + t);
        }
    }

    private static String getErrorInfoFromException(Throwable e) {
        try {
            StringWriter sw = new StringWriter();
            PrintWriter pw = new PrintWriter(sw);
            e.printStackTrace(pw);
            return "\r\n" + sw.toString() + "\r\n";
        } catch (Exception e2) {
            return "bad getErrorInfoFromException";
        }
    }

    private String javaOnCompleteToCS(HashMap<String, Object> res) {
        HashMap<String, Object> map = new HashMap<>();
        map.put("status", Integer.valueOf(1));
        map.put("action", Integer.valueOf(10));
        map.put("res", res);
        Hashon hashon = new Hashon();
        return hashon.fromHashMap(map);
    }

    private String javaOnErrorToCS(Throwable t) {
        HashMap<String, Object> map = new HashMap<>();
        map.put("status", Integer.valueOf(2));
        map.put("action", Integer.valueOf(10));
        map.put("res", throwableToMap(t));
        Hashon hashon = new Hashon();
        return hashon.fromHashMap(map);
    }

    private HashMap<String, Object> throwableToMap(Throwable t) {
        HashMap<Object, Object> map = new HashMap<Object, Object>();
        map.put("msg", t.getMessage());
        ArrayList<HashMap<Object, Object>> traces = new ArrayList();
        byte b;
        int i;
        StackTraceElement[] arrayOfStackTraceElement;
        for (i = (arrayOfStackTraceElement = t.getStackTrace()).length, b = 0; b < i; ) {
            StackTraceElement trace = arrayOfStackTraceElement[b];
            HashMap<Object, Object> element = new HashMap<Object, Object>();
            element.put("cls", trace.getClassName());
            element.put("method", trace.getMethodName());
            element.put("file", trace.getFileName());
            element.put("line", Integer.valueOf(trace.getLineNumber()));
            traces.add(element);
            b++;
        }

        map.put("stack", traces);
        Throwable cause = t.getCause();
        if (cause != null) {
            map.put("cause", throwableToMap(cause));
        }
        return (HashMap) map;
    }
}