package cn.sharesdk.demo.wxapi;

import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

import cn.sharesdk.wechat.utils.WXAppExtendObject;
import cn.sharesdk.wechat.utils.WXMediaMessage;
import cn.sharesdk.wechat.utils.WechatHandlerActivity;

public class WXEntryActivity extends WechatHandlerActivity {
	public void onGetMessageFromWXReq(WXMediaMessage msg) {
		if (msg != null) {
			Intent iLaunchMyself = getPackageManager().getLaunchIntentForPackage(getPackageName());
			startActivity(iLaunchMyself);
		}
	}

	public void onShowMessageFromWXReq(WXMediaMessage msg) {
		if (msg != null && msg.mediaObject != null &&
				msg.mediaObject instanceof WXAppExtendObject) {
			WXAppExtendObject obj = (WXAppExtendObject)msg.mediaObject;
			//Toast.makeText((Context)this, obj.extInfo, 0).show();
		}
	}
}
