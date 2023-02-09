using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
#if UNITY_IOS
//using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

public class VipEditorSetting : ScriptableObject
{
    public static string RESOURCES_NAME = "BuildEditor_Setting.asset";
    //public static string RESOURCES_PATH;
    static VipEditorSetting _instance;

#if UNITY_EDITOR
    public static VipEditorSetting Instance
    {
        get
        {
            if (_instance == null)
            {
                //   AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(VipEditorSetting));
                string path = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
                Debug.LogError(path);
                int _charPos = 0;
                for (int i = 0; i < path.Length; i++)
                {
                    char p = path[i];
#if UNITY_ANDROID
                    if (p == '\\')
                    {
                        _charPos = i;
                    }
#endif
#if UNITY_IOS
					if (p == '/')
					{
						_charPos = i;
					}
#endif
                }
                int assetIndex = path.IndexOf("Assets");
                path = path.Substring(assetIndex, _charPos - assetIndex + 1);
                path = Path.Combine(path, RESOURCES_NAME);

                //Debug.LogError(path);
                //return null;

                _instance = AssetDatabase.LoadAssetAtPath<VipEditorSetting>(path);
                if (_instance == null)
                {
                    VipEditorSetting asset = CreateInstance<VipEditorSetting>();
                    Directory.CreateDirectory(path);
                    AssetDatabase.CreateAsset(asset, path);
                    _instance = asset;

                }

                Debug.LogError(path);
            }
            return _instance;
        }
    }

    public static void Focus()
    {
        Selection.activeObject = Instance;
    }

    public string ToString()
    {
        string otput = "TEST VALUE= " + this.Test + "\n";
        otput += ("Current Setting: \nIOS_Build_NSCalendarsUsageDescription:" + VipEditorSetting.Instance.IOS_Build_NSCalendarsUsageDescription + "\nIOS_Build_NSCalendarsUsageDescription:"
                 + VipEditorSetting.Instance.IOS_Build_NSCalendarsUsageDescription + "\nIOS_Build_SKAdNetworkItems:"
                 + VipEditorSetting.Instance.IOS_Build_SKAdNetworkItems + "\nBitcode:"
                 + VipEditorSetting.Instance.IOS_Build_enableBitcode);
        otput += "\nCapabilityTypeList:\n";
        foreach (var item in Instance.CapabilityTypeList)
        {
            otput += "<color=green>" + item.ToString() + "</color>\n";
        }

        otput += "<color=green>IDs netword: " + NetIDs.IDs.Length + "</color>\n";

        return otput;
    }

    private void OnValidate()
    {
        this.NetIDNumber = NetIDs.IDs.Length;
    }
#endif


    public string Test;


    [Space]
    [Header("Android Build Setting")]

    public string buildPassword;
    public string fileName;

    [Space]
    public int buildNumber;
    public int Version;
    public int buildVersionCode;

    public int DevBuildVersion;

    [Space]
    [Header("IOS Build Setting")]

    [Header("Plist")]
    [Tooltip("Network IDs")]
    public bool IOS_Build_SKAdNetworkItems = true;
    public int NetIDNumber = 0;
    [Tooltip("Cá nhân hóa quảng cáo")]
    public bool IOS_Build_NSUserTrackingUsageDescriptions = true;
    [Tooltip("Calendars Usage Description")]
    public bool IOS_Build_NSCalendarsUsageDescription = true;

    [Header("Build Property")]
    public bool IOS_Build_enableBitcode = false;
    [SerializeField]
    public CapabilityTypeEnum[] CapabilityTypeList;

    [Header("Build Setting")]
    public string BuildPath;

#if UNITY_EDITOR
    [Button("Open Folder")]
    static void OpenBuildFolder()
    {
        EditorUtility.RevealInFinder(VipEditorSetting.Instance.BuildPath);
    }
    [Button("Change Build Folder")]
    static void ChangeBuildFolder()
    {
        string path = EditorUtility.OpenFolderPanel("Select folder for build", "", "");
        //  Debug.Log(path);
        VipEditorSetting.Instance.BuildPath = path + "/";
    }
#endif

#if UNITY_EDITOR
    //[Header("Build")]
    //string ok;
    [Button("Build APK-DEV")]
    [PropertySpace]
    static void BuildDev()
    {
        VipMenuEditor.AutoBuildAPKDEV();
    }
    [Button("Build APK")]
    static void BuildAPK()
    {
        VipMenuEditor.AutoBuildApk();
    }
    [Button("Build AAB")]
    static void BuildAAB()
    {
        VipMenuEditor.AutoBuildaab();
    }
    [Button("Build All")]
    static void BuildAll()
    {
        VipMenuEditor.AutoBuilAll();
    }
#endif
}

public enum CapabilityTypeEnum
{
    ApplePay, SignInWithApple, AccessWiFiInformation, WirelessAccessoryConfiguration,
    Wallet, Siri, PushNotifications, PersonalVPN, KeychainSharing, InterAppAudio,
    Maps, iCloud, HomeKit, HealthKit, GameCenter, DataProtection, BackgroundModes,
    AssociatedDomains, AppGroups, InAppPurchase
}
