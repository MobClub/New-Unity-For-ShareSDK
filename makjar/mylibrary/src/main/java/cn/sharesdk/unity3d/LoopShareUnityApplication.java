package cn.sharesdk.unity3d;

import android.content.Intent;
import android.util.Log;

import com.mob.MobApplication;
import com.mob.tools.utils.Hashon;
import com.unity3d.player.UnityPlayer;

import java.io.PrintWriter;
import java.io.StringWriter;
import java.util.HashMap;

import cn.sharesdk.framework.ShareSDK;
import cn.sharesdk.framework.loopshare.LoopShareResultListener;

public class LoopShareUnityApplication extends MobApplication {

    @Override
    public void onCreate() {
        super.onCreate();
        Log.e("QQQ", "LoopShareUnityApplication: Set prepareLoopShare in "
            + "LoopShareUnityApplication start ");
//        Intent intent = new Intent();
//        intent.setClass(getApplicationContext(), MobUnityPlayerActivity.class);
//        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
//        startActivity(intent);

        Intent intent = new Intent();
        intent.setClass(getApplicationContext(), com.unity3d.player.UnityPlayerActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        startActivity(intent);

        Log.e("QQQ", "LoopShareUnityApplication:  start MobUnityPlayerActivity is ok");


        ShareSDK.prepareLoopShare(new LoopShareResultListener() {
            @Override
            public void onResult(Object var1) {
                //super.onResult(var1);
                try {
                    String resp = (new Hashon()).fromHashMap((HashMap) var1);

                    System.out.println("LoopShareUnityApplication: onResult: " + resp);
                    Log.e("QQQ", "LoopShareUnityApplication onResult: " + resp);

                    UnityPlayer.UnitySendMessage("ShareSDKRestoreScene", "_RestoreCallBack", resp);

                    System.out.println("LoopShareUnityApplication: onResult: UnitySendMessage finish");
                    Log.e("QQQ", "LoopShareUnityApplication onResult:  UnitySendMessage finish");
                } catch (Throwable t) {
                    System.out.println("LoopShareUnityApplication onResult catch " + t);
                    Log.e("QQQ", "LoopShareUnityApplication onResult: Throwable " + t);
                }
            }

            @Override
            public void onError(Throwable var1) {
                //super.onError(var1);
                try {
                    String error = getErrorInfoFromException(var1);
                    System.out.println("LoopShareUnityApplication: onError: " + error);
                    Log.e("QQQ", "LoopShareUnityApplication onError: " + error);

                    UnityPlayer.UnitySendMessage("ShareSDKRestoreScene", "_RestoreCallBack", error);

                    System.out.println("LoopShareUnityApplication: onError: UnitySendMessage finish");
                    Log.e("QQQ", "LoopShareUnityApplication onError:  UnitySendMessage finish");
                } catch (Throwable t) {
                    System.out.println("LoopShareUnityApplication onError catch " + t);
                    Log.e("QQQ", "LoopShareUnityApplication onError: Throwable " + t);
                }
            }
        });

        Log.e("QQQ", "Set prepareLoopShare in LoopShareUnityApplication is ok ");
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


}
