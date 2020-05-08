package cn.sharesdk.unity3d;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;

public class MobUnityPlayerActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
        Log.e("QQQ", " MobUnityPlayerActivity onCreate ");
    }

    @Override
    protected void onResume() {
        super.onResume();
        Log.e("QQQ", " MobUnityPlayerActivity onResume ");
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        Log.e("QQQ", " MobUnityPlayerActivity onResume ");
    }
}
