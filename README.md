# New-Unity-For-ShareSDK
### This is the new version and new sample of [ShareSDK](https://github.com/MobClub/ShareSDK-for-iOS) for Unity3D.
**supported original ShareSDK version:**

- Android - V3.1.4
- iOS - V4.1.1

**中文集成文档**

- [iOS&Android](https://www.mob.com/wiki/detailed/?wiki=ShareSDK_Unity_title_andios&id=14)


- - - - - - - - - - - -

##The notes for fast integration of Unity3D##

### *Integration of general part*

### Step 1 : Download Unity3D tools of ShareSDK

Double click or import ShareSDK.unitypackage Import the related file. Note that this operation may overwrite the files you already exist.

### Step 2 : Add ShareSDK script and set the platforms’ information

Need to add ShareSDK to GameObject(Like Main Camera). Click”Add Component” from the right-hand side bar, and choose ShareSDK to be added.
![](http://download.sdk.mob.com/2020/11/12/16/1605169895469114.44.png)
After that, it will show the social platforms’ information and which one is avaliable for use. You could click here to edit the information based on your needs. Please make sure the compiler environment is Android or iOS, cause they are totally different.

App Key on first line is appkey from ShareSDK. You could get that from our website when you register an account. 

##### Step 2-1 .Android gradle Integrated Compilation configuration
![]()

**==Key documents: baseProjectTemplate.gradle , launcherTemplate.gradle and gradleTemplate.properties==**
For unity2019.3 and above, there are three switches under build Settings > player settings. When a new project is created, you can open these three switches to generate three corresponding files in plugins > Android. Since these three files are directly provided by sharesdk, you just need to import. Unitypackage. Unity detects that these three files already exist and will be automatically updated to check status;

**Note:** for unity2019.3 and below, there are only two key files compiled by gradle: mainTemplate.gradle And Proguard- user.txt , so you need to check it manually. The switch position is also under build Settings > player settings baseProjectTemplate.gradle and launcherTemplate.gradle Copy the integration code in the file to the newly generated mainTemplate.gradle In the file
The baseProjectTemplate.gradle File, one of the core files compiled by gradle. Use the editor to open this file. The main points are as follows:

*Here is the distinction between unity5.6 and unity2017 For gradle plug-in version, you can use which version to use during development. If you use other unity versions, please choose one at will, and then build. An error will be reported during compilation. When the console information reports an error, it will prompt the plug-in version. It is good to modify the version according to the prompt (only modify the following numbers, such as 2.3.0 or 2.1.0)*

```
 buildscript {
        repositories {
            jcenter()
        }
        dependencies {
            classpath 'com.android.tools.build:gradle:2.3.0'//Unity2017
            //classpath 'com.android.tools.build:gradle:2.1.0'//Unity5.6
            // 注册MobSDK
            classpath 'com.mob.sdk:MobSDK:+'
        }
    }
```

* The baseProjectTemplate.gradle File, which is one of the core files of the integration, can be opened with an editor. The main points are as follows:：

**Here is to configure the alias and password of signature file and signature file (signature file required for official release of APK). You can write absolute path or relative path. Relative path uses ". \ \" to jump out of one level of directory, and jump out of multiple layers to concatenate continuously**
    

```xml
    signingConfigs {
            release {
                keyAlias 'demokey.keystore'
                keyPassword '123456'
                storeFile file('F:\\Unitydemo(CJY)\\MobPushForUnity\\Assets\\Plugins\\Android\\demokey.keystore')
                storePassword '123456'
            }
    }
```

**Here is the configuration of obpush file, which is Proguard provided by mobpush- user.txt File, the content of this file does not need to be changed, just follow the provided,
If you need to add additional obfuscation logic to your code, you can add obfuscation rules. If it is unity2017 or below, please change the annotated code;
(minifyenabled property is whether code obfuscation is enabled: true means code obfuscation is on, false is off)**
    

```xml
    buildTypes {
            release {
                minifyEnabled true// Is it confusing
                //shrinkResources false// Do you want to remove invalid resource files
                proguardFiles getDefaultProguardFile('proguard-android.txt'), 'proguard-user.txt' //Unity2017及以上
    	    //proguardFiles getDefaultProguardFile('proguard-android.txt'), 'proguard-unity.txt'  //Unity2017以下
                signingConfig signingConfigs.release
            }
    
            debug {
                minifyEnabled false
                signingConfig signingConfigs.release
            }
    }
```

**Configure third party key information**

The sharesdk provides a MobSDK.gradle In the file, you can directly change the mob key to your own, delete unnecessary platforms, or modify it to your own third-party key information baseProjectTemplate.gradle The file depends on this MobSDK.gradle "Apply from: '"/ MobSDK.gradle ' " , You can also use the baseProjectTemplate.gradle Add directly to the file MobSDK.gradle Code in file；
    
![image](https://download.sdk.mob.com/2019/06/02/17/1559467959887/690_400_30.44.png)
    
**The gradle configuration is as follows**

```xml
    apply plugin: 'com.mob.sdk'
    
    MobSDK {
        appKey "moba6b6c6d6"
        appSecret "b89d2427a3bc7ad1aea1e1e8c1d36bf3"
    
        ShareSDK {
    		version "3.3.0"
    		
            //Platform configuration information
            devInfo {
                SinaWeibo {
                    id 1
                    sortId 1
                    appKey "568898243"
                    appSecret "38a4f8204cc784f81f9f0daaf31e02e3"
                    callbackUri "http://www.sharesdk.cn"
                    shareByAppClient true
                    enable true
                }
    
                Wechat {
                    id 4
                    sortId 4
                    appId "wx4868b35061f87885"
                    appSecret "64020361b8ec4c99936c0e3999a9f249"
                    userName "gh_afb25ac019c9"
                    path "pages/index/index.html?id=1"
                    withShareTicket true
                    miniprogramType 0
                    bypassApproval false
                    enable true
                }
    
                WechatMoments {
                    id 5
                    sortId 5
                    appId "wx4868b35061f87885"
                    appSecret "64020361b8ec4c99936c0e3999a9f249"
                    bypassApproval false
                    enable true
                }
    
    
                QQ {
                    id 7
                    sortId 7
                    appId "100371282"
                    appKey "aed9b0303e3ed1e27bae87c33761161d"
                    shareByAppClient true
                    bypassApproval false
                    enable true
                }
    
                Facebook {
                    id 8
                    sortId 8
                    appKey "1412473428822331"
                    appSecret "a42f4f3f867dc947b9ed6020c2e93558"
                    callbackUri "https://mob.com"
                    shareByAppClient true
                    enable true
                }
    
            }
        }
    }
```

##### Step 2-2.Android offline Integrated Compilation configuration

* After importing the sharesdk offline package, the package name and account information in the manifests file t in the sharesdk folder are the demo information by default, and you need to manually configure your own

  ![image-20200909123259999](https://s1.ax1x.com/2020/09/09/w1D5tO.png)

* When using wechat to log in or share, you need to put the DemoCallback.jar Change it to your own package name**[Click to view the specific process](http://bbs.mob.com/thread-23519-1-1.html)**

* Set the social platform and mob account information in the mounted sharesdk file. You can also“ ShareSDKDevInfo.cs ”The effect is the same if you directly set the information of social platform in。

  ![image-20200909124428652](https://s1.ax1x.com/2020/09/11/wtUJ0I.png)

* When building, there is a build system option. Remember to select the internal option instead（**important**）

  ![image-20200909122501610](https://s1.ax1x.com/2020/09/09/w1B2o8.png)

##### Step 2-3.IOS compilation configuration

Initialization and social platform information configuration

modify ShareSDKDevInfo.cs File, configure the required platform information

**Configure the appkey of your own sharesdk (obtained from the first step)**

```c#
    public class AppKey 
    {
    	//Configure sharesdk appkey
    	public string appKey = "a5d9150e8348";
    }
```

**Select the platform you want, and you can comment or delete what you don't want**

```c#
    public class DevInfoSet
     
    {
    	public SinaWeiboDevInfo sinaweibo;
    	public TencentWeiboDevInfo tencentweibo;
    	public QQ qq;
    	public QZone qzone;
    }
```

**Configure the information of the corresponding platform (it is recommended to modify the string value directly)**

```c#
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
    	public string auth_type = "both";	//can pass "both","sso",or "web"
    	#endif
    }
```

### Step 3、Interface call

#### Mobtech privacy service process access guidance

According to the requirements of national laws and regulations, when using the SDK products provided by mobtech, developers need to show the privacy service agreement of mobtech to the end users and obtain the authorization of users。

Mobtech provides a privacy service interface for developers to use。

**<font color='red'>Note: all developers must access the mobtech privacy service process according to this document, otherwise, the related services provided by mobtech SDKs may be unavailable.</font>**


##### Step 3-1、 Privacy agreement authorization


##### Show mobtech privacy agreement: developers need to show the mobtech privacy terms and return the agreement results to mobtech. The following methods are recommended：

##### **Step 1: embed the URL of mobtech privacy protocol into the app's own privacy protocol description (recommended)**

**Recommend adding privacy policy text**：In order to realize the sharing and authorization functions, we use mobtech's sharesdk product. For the privacy policy of this product, please refer to：https://www.mob.com/about/policy/en

##### **Step 2：Return user authorization results**

When the end user chooses the pop-up box of privacy agreement, whether agree or reject, the developer should send the authorization result back to SDK in time。

Call example

The first parameter passed in is of boolean type. True represents consent to authorization, and false represents disapproval of authorization

```
	mobsdk.submitPolicyGrantResult(true);
```

**<font color='red'>Note: this interface must be connected, otherwise the related services provided by mobtech SDKs may not be available.</font>**


##### Step 3-2、 Sharesdk share authorization interface


First introduce the namespace:

```c#
using cn.sharesdk.unity3d;
private ShareSDK ssdk;
```

### share

**Custom sharing information**

```c#
    ShareContent content = new ShareContent();
    content.SetText("this is a test string.");
    content.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/1.jpg");
    content.SetTitle("test title");
    content.SetTitleUrl("http://www.mob.com");
    content.SetSite("Mob-ShareSDK");
    content.SetSiteUrl("http://www.mob.com");
    content.SetUrl("http://www.mob.com");
    content.SetComment("test description");
    content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
    content.SetShareType(ContentType.Webpage);
```

Shared parameters can be referred to：[Platform parameter description document](http://www.mob.com/wiki/detailed?wiki=ShareSDK_Unity_bbb&id=14)


**Set sharing callback**   

```c
    ssdk.shareHandler = ShareResultHandler;
    //The following is the definition of callback:
    void ShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
    	if (state == ResponseState.Success)
    	{
    		print ("share result :");
    		print (MiniJSON.jsonEncode(result));
    	}
    		else if (state == ResponseState.Fail)
    	{
    		print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
    	}
    	else if (state == ResponseState.Cancel) 
    	{
    			print ("cancel !");
    	}
    }
```

**Share**

```c#
    //Share through the share menu
    ssdk.ShowPlatformList (null, content, 100, 100);
     
    //Share directly through the editing interface
    ssdk.ShowShareContentEditor (PlatformType.SinaWeibo, content);
     
    //Direct sharing
    ssdk.ShareContent (PlatformType.SinaWeibo, content);
```

### Authorization (jump to a third party platform for authorization each time)

**Set authorization callback**

```c
ssdk.authHandler = AuthResultHandler;
//The following is the definition of callback:
void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
{
	if (state == ResponseState.Success)
	{
		print ("authorize success !");
	}
	else if (state == ResponseState.Fail)
	{
        print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
	}
	else if (state == ResponseState.Cancel) 
	{
		print ("cancel !");
	}
}
```

**Authorization**

```
ssdk.Authorize(PlatformType.SinaWeibo);
```

### Access to user information (only when you jump to a third-party platform for authorization)

**Specifies the callback to get user information**

```c#
sdk.showUserHandler = GetUserInfoResultHandler;
//The following is the definition of callback:
void GetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
{
	if (state == ResponseState.Success)
	{
		print ("get user info result :");
		print (MiniJSON.jsonEncode(result));
	}
	else if (state == ResponseState.Fail)
	{
		print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
    }
	else if (state == ResponseState.Cancel) 
	{
		print ("cancel !");
	}
}
```

**Get user information**

```c#
ssdk.GetUserInfo(PlatformType.SinaWeibo);
```

### Closed loop sharing

First of all, open closed-loop sharing in the background and fill in the corresponding information. Secondly, we need to add our JS code in our own front-end sharing interface. Please refer to this for details **[Document description ](http://www.mob.com/wiki/detailed?wiki=ios_sharesdk_closed_share&id=14)Then share the link and add the following method,
```
ShareSDKRestoreScene.setRestoreSceneListener(OnRestoreScene);
```

```
public static void OnRestoreScene(RestoreSceneInfo scene)
    {
        Hashtable customParams = scene.customParams; 
        if (customParams != null)
        {
            Debug.Log("[sharesdk-unity-Demo]OnRestoreScen(). path:" + scene.path.ToString() + ", params:" + scene.customParams.toJson());
        }
        else
        {
            Debug.Log("[sharesdk-unity-Demo]OnRestoreScen(). path:" + scene.path.ToString() + ", params:null");
        }

        //According to the scene, the developer handles the scene transformation himself
        //SceneManager.LoadScene("SceneA");
    }
```


- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

**Finally, if you have any other questions, please do not be hesitate to contact us:**

- Customer Service QQ : 4006852216

- or Skype:amber

More information About ShareSDK, please visit our website [Mob.com](http://www.mob.com)


