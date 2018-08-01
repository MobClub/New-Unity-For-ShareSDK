# New-Unity-For-ShareSDK
### This is the new version and new sample of [ShareSDK](https://github.com/MobClub/ShareSDK-for-iOS) for Unity3D.
**supported original ShareSDK version:**

- Android - V3.2.1
- iOS - V4.1.3

**中文集成文档**

- [Android](http://wiki.mob.com/unity3d%E5%BF%AB%E9%80%9F%E9%9B%86%E6%88%90%E6%8C%87%E5%8D%97/)
- [iOS](http://wiki.mob.com/sharesdk-ios-for-untiy3d-%E5%90%AB%E5%BF%AB%E6%8D%B7%E6%89%93%E5%8C%85%E6%8F%92%E4%BB%B6/)


- - - - - - - - - - - -

##The notes for fast integration of Unity3D##

### *Integration of general part*

###### Step 1 : Download Unity3D tools of ShareSDK

Open Github and download New-Unity-For-ShareSDK section. Copy ”Unity3dDemo/Assets/Plugins”catalogue to Assets catalogue, or double click “sharesdk-unity3d-plugin.unitypackage” to import relative documents.
Please notice that this operation could cover your original existed documents!

###### Step 2 : Add ShareSDK script and set the platforms’ information

Need to add ShareSDK to GameObject(Like Main Camera). Click”Add Component” from the right-hand side bar, and choose ShareSDK to be added.
![image](http://wiki.mob.com/wp-content/uploads/2015/09/step1.jpg)
After that, it will show the social platforms’ information and which one is avaliable for use. You could click here to edit the information based on your needs. Please make sure the compiler environment is Android or iOS, cause they are totally different.

App Key on first line is appkey from ShareSDK. You could get that from our website when you register an account. 
![image](http://wiki.mob.com/wp-content/uploads/2015/09/step2.jpg)

You could also set the social platforms’ information in file “ShareSDKDevInfo.cs”. The effect are the same.

i.Set your own ShareSDK Appkey

        public class AppKey 
        {
        //set ShareSDK AppKey
        public string appKey = "a5d9150e8348";
        }

ii.Choose the platforms based on your needs

        public class DevInfoSet
        {
        public SinaWeiboDevInfo sinaweibo;
        public TencentWeiboDevInfo tencentweibo;
        public QQ qq;
        public QZone qzone;
        }

iii.Set the platforms’ information (you could directly edit the string value )

        public class SinaWeiboDevInfo : DevInfo 
        {
        #if UNITY_ANDROID
        public const int type = (int) PlatformType.SinaWeibo;
        public string SortId = "1";
        public string AppKey = "568898243";
        public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
        public string RedirectUrl = "http://www.sharesdk.cn";
        public string ShareByAppClient = "false";
        #elif UNITY_IPHONE
        public const int type = (int) PlatformType.SinaWeibo;
        public string app_key = "568898243";
        public string app_secret ="38a4f8204cc784f81f9f0daaf31e02e3";
        public string redirect_uri = "http://www.sharesdk.cn";
        public string auth_type = "both";
        #endif
        }

###### Step 3 : Sharing and Authorization

Please import Name Space first :

        using cn.sharesdk.unity3d;

        private ShareSDK ssdk;

#### About Sharing
i.Customize the sharing information :

        ShareContent content = new ShareContent();
        content.SetText("this is a test string.");
        content.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/1.jpg");
        content.SetTitle("test title");
        content.SetShareType(ContentType.Image);

ii.If need,you can customize the ShareContent for some detail platform(Please refer the attachment<分享内容参数表>):

        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetText("Sina share content");
        customizeShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
        customizeShareParams.SetShareType(ContentType.Image);
        customizeShareParams.SetObjectID("SinaID");
        content.SetShareContentCustomize(PlatformType.SinaWeibo, customizeShareParams);


iii.Set the sharing callback param :

        ssdk.shareHandler = ShareResultHandler;

and Defination of callback:

        void ShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
        {
            if (state == ResponseState.Success)
            {
            print ("share result :");
            print (MiniJSON.jsonEncode(result));
            }
            else if (state == ResponseState.Fail)
            {
            print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
            }
            else if (state == ResponseState.Cancel) 
            {
            print ("cancel !");
            }
        }

iv. Pass the sharecontent to Sharing interface :

        //Share by the menu
        ssdk.ShowPlatformList (null, content, 100, 100);

        //share by the content editor
        ssdk.ShowShareContentEditor (PlatformType.SinaWeibo, content);

        //share directly
        ssdk.ShareContent (PlatformType.SinaWeibo, content);

#### About Authorization

i. Set the auth call back :

        ssdk.authHandler = AuthResultHandler;

and defination for callback:

        void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
        {
        if (state == ResponseState.Success)
        {
        print ("authorize success !");
        }
        else if (state == ResponseState.Fail)
        {
        print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
        }
        else if (state == ResponseState.Cancel) 
        {
        print ("cancel !");
        }
        }

ii. now you can make an Authorization:

        ssdk.Authorize(reqID, PlatformType.SinaWeibo);

#### About Get User's information

i. Set the get user's info' call back :

        sdk.showUserHandler = GetUserInfoResultHandler;

and defination for callback:

        void GetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
        {
        if (state == ResponseState.Success)
        {
        print ("get user info result :");
        print (MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
        print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
        }
        else if (state == ResponseState.Cancel) 
        {
        print ("cancel !");
        }
        }

ii. now you can get the user's info:

        ssdk.GetUserInfo(reqID, PlatformType.SinaWeibo);

### *Integration for iOS* (if you don't need iOS ,please ignore this)

**Edit document ”ShareSDKUnity3DBridge.m”**

For those who need to use Single sign on (QQ Zone, RenRen, Tencent Weibo) or the platforms have to be shared by client-side (Wechat, Yinxin, Pinterest, Google+, QQ), please make sure whether the macro is define. If you want to integrate wechat, then you need to open_ SHARESDK_WECHAT_. Same as other platforms.
![image](http://wiki.mob.com/wp-content/uploads/2015/09/mm.jpg)

You could export XCode project after editing complete

***Noticed: The next steps need to be done after exporting XCode project***


Import ShareSDK iOS version to the project.
Log in Mob website to download the latest version of ShareSDK.If you have not download, please click here to download.(Notes: You could choose the platforms to use or download the whole offical demo.) If you download the demo, it will apears after you unzip it:
![image](http://wiki.mob.com/wp-content/uploads/2015/09/u3d1.jpg)

Drag file”ShareSDK” to Xcode project from Untiy3D:

![image](http://wiki.mob.com/wp-content/uploads/2015/09/u3d2.jpg)

Choose “Create groups”, check the project.Check “Copy item if needed”, and you could copy the folder to Xcode projects. (If you don’t check “copy if needed”, it means just refer the file but copy), and click Finish, you could add ShareSDK to Xcode project:

![image](http://wiki.mob.com/wp-content/uploads/2015/09/SDK3.jpg)

After import ShareSDK to Xcode Project, please add relevant dependent libraries.

![image](http://wiki.mob.com/wp-content/uploads/2015/09/SDK4.jpg)

Basic:
- libicucore.dylib
- libz.dylib
- libstdc++.dylib
- JavaScriptCore.framework

for Sina SDK(optional,if need Sina):
- ImageIO.framework
- AdSupport.framework
- libsqlite3.dylib

for QQ SDK(optional,if need QQ or QZone):
- libsqlite3.dylib

for Wechat(optional,if need Wechat):
- libsqlite3.dylib

for Google＋SDK(optional,if need Google+):
- CoreMotion.framework
- CoreLocation.framework
- MediaPlayer.framework
- AssetsLibrary.framework
- AddressBook.framework

**Add URL scheme**

For those who need client side sharing platforms and Single sign on authorization platforms, need add an URL Scheme. Client side sharing please refer to the demo of [ShareSDK For iOS(v3.x)](https://github.com/MobClub/ShareSDK).


### *Integration for Android* (if you don't need Android ,please ignore this)


###### Step 1 : 

Download”New-Unity-For-ShareSDK”project, and copy” Unity3D-ShareSDK/Assets/plugins” catalogue to Assets catalogue in your project. Or doule click “ShareSDKForU3D.unitypackage” and import relative documents, Whle you doing that, ShareSDK has been integrated successfully in your project.

![image](http://wiki.mob.com/wp-content/uploads/2015/09/F8EBCCEA-A5F9-42A5-9129-FAFE7FD1324E.png)
![image](http://wiki.mob.com/wp-content/uploads/2014/09/QQ%E6%88%AA%E5%9B%BE20180224115322.jpg)



###### Step 2 : 

Set “AndroidManifest.xml” document and add relative configuration imformation.

![image](http://wiki.mob.com/wp-content/uploads/2015/09/04930835-7D1F-45C6-A898-DA9797175F82.png)


###### Step 3 : 

After above, you can share,auth or get user's' info. You can refer to Unity3D-ShareSDK/Assets/Demo.cs and see how to user it.
Before that ,you must handle cs file Unity3D-ShareSDK/Assets/Plugins/ShareSDK/ShareSDK.cs with you project,and set the platforms configuration.

![image](http://wiki.mob.com/wp-content/uploads/2015/09/CFB71BF8-2371-46F3-A88D-BC0F644C103D.png)

Then you could call the code, like open the sharing interface:



###### Step 4 : 

While runing the Demo, you need to add Demo.cs and ShareSDK.cs together

![image](http://wiki.mob.com/wp-content/uploads/2015/09/5C02A01B-E641-45BE-9BB5-769BC72AE527.png)


- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

**Finally, if you have any other questions, please do not be hesitate to contact us:**

- Customer Service QQ : 4006852216

- or Skype:amber

More information About ShareSDK, please visit our website [Mob.com](http://www.mob.com)


