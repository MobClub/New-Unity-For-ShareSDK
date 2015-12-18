New-Unity-For-ShareSDK
=======================================
#This is the new version and new sample of ShareSDK for Unity3D.
- supported original ShareSDK version:
- Android - V2.6.5
- iOS - V3.1.4
- 中文文档请查看[简洁版unity3d快速集成](http://wiki.mob.com/%E7%AE%80%E6%B4%81%E7%89%88unity3d%E5%BF%AB%E9%80%9F%E9%9B%86%E6%88%90%E6%96%87%E6%A1%A3/)


###### The notes for fast integration of Unity3D

###Integration of general part

- Step 1 : Download Unity3D tools of ShareSDK

Open Github and download New-Unity-For-ShareSDK section. Copy ”Unity3dDemo/Assets/Plugins”catalogue to Assets catalogue, or double click “sharesdk-unity3d-plugin.unitypackage” to import relative documents.
Please notice that this operation could cover your original existed documents!

- Step 2 : Add ShareSDK script and set the platforms’ information
Need to add ShareSDK to GameObject(Like Main Camera). Click”Add Component” from the right-hand side bar, and choose ShareSDK to be added.
![github](http://wiki.mob.com/wp-content/uploads/2015/09/step1.jpg “github”)
After that, it will show the social platforms’ information and which one is avaliable for use. You could click here to edit the information based on your needs. Please make sure the compiler environment is Android or iOS, cause they are totally different.

App Key on first line is appkey from ShareSDK. You could get that from our website when you register an account. 
![github](http://wiki.mob.com/wp-content/uploads/2015/09/step2.jpg “github”)

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

- Step 3 : Sharing and Authorization

Please import Name Space first

using cn.sharesdk.unity3d;

private ShareSDK ssdk;

###About Sharing
i.Customize the sharing information :

        Hashtable content = new Hashtable();
        content["content"] = "this is a test string.";
        content["image"] = "https://f1.webshare.mob.com/code/demo/img/1.jpg";
        content["title"] = "test title";
        content["description"] = "test description";
        content["url"] = "http://sharesdk.cn";
        content["type"] = ContentType.News;

ii.Set the sharing callback param :

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

iii. Transfer contents to Sharing interface :

ssdk.ShowShareMenu (reqID, null, content, 100, 100);

###About Authorization

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

###About Get User's information

i. Set the get user's info' call back :

sdk.showUserHandler = GetUserInfoResultHandler;

and defination for callback:
Defination of callback
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

ssdk.GetUserInfo(reqID, PlatformType.SinaWeibo);'







More information About ShareSDK, please visit our website [Mob.com](http://www.mob.com)