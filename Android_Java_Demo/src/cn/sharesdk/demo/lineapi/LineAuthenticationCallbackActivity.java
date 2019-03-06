package cn.sharesdk.demo.lineapi;

import android.os.Bundle;

import cn.sharesdk.line.LineHandlerActivity;

/**
 * Created by xiangli on 2019/1/16.
 *
 * /**
 * Activity to notify an Intent of authentication result to {LineSSOProcessor}.
 * {@code LineAuthenticationActivity} can not receive the intent directly because it must not be
 * singleInstance or singleTask.
 */

public class LineAuthenticationCallbackActivity extends LineHandlerActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }
}
