using Frictionless;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameUtils : SingletonClass<GameUtils>
{
    public static bool isInit = false;
    public delegate void VoidEvent();
    public delegate void VoidFloatInputEvent(float input);
    public delegate void VoidStringInputEvent(string input);

    public static void EventHandlerIni()
    {

        ServiceFactory.Instance.Reset();
        ServiceFactory.Instance.RegisterSingleton<MessageRouter>();
    }
    public static void RaiseMessage(object msg)
    {
        if (ServiceFactory.Instance != null)
        {
            ServiceFactory.Instance.Resolve<MessageRouter>().RaiseMessage(msg);
        }
    }



    public static void AddHandler<T>(Action<T> handler)
    {
        if (ServiceFactory.Instance != null)

            ServiceFactory.Instance.Resolve<MessageRouter>().AddHandler<T>(handler);
    }

    public static void RemoveHandler<T>(Action<T> handler)
    {
        if (ServiceFactory.Instance != null && handler != null)
            if (ServiceFactory.Instance.Resolve<MessageRouter>() != null)
                ServiceFactory.Instance.Resolve<MessageRouter>().RemoveHandler<T>(handler);
    }
    public static void EventHandlerReset()
    {

        ServiceFactory.Instance.Reset();
    }


    public static DateTime TimeNow()
    {
        return DateTime.Now;
    }


    public static string DeviceID
    {
        get
        {
            //#if UNITY_EDITOR
            // return "f6971e456d70e9d53e71f25cec5c21ee";
            //#endif
            //return MACAddress;
#if UNITY_EDITOR
            return MACAddress;
#endif
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    public static string MACAddress
    {
        get
        {

            string physicalAddress = "";

            NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adaper in nice)
            {

                Debug.Log(adaper.Description);

                if (adaper.Description == "en0")
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    break;
                }
                else
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();

                    if (physicalAddress != "")
                    {
                        break;
                    };
                }
            }

            return physicalAddress;

        }
    }


    /// <summary>
    /// get AndroidID
    /// </summary>
    /// <returns></returns>
    public static string GetAndroidDeviceID()
    {

        // Get Android ID
        AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");

        string android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

        // Get bytes of Android ID
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(android_id);

        // Encrypt bytes with md5
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        string device_id = hashString.PadLeft(32, '0');

        return device_id;
    }


    public static string StringServerToDate(string input)
    {
        try
        {
            int _start = input.IndexOf(".");
            int _end = input.Length;
            string _input = input.Substring(0, _start);
            _input = _input.Replace("T", " ");
            var newDate = DateTime.ParseExact(_input,
                                              "yyyy-MM-dd HH:mm:ss",
                                               CultureInfo.InvariantCulture);
            var _dif = newDate - DateTime.Now;
            return new DateTime(_dif.Ticks).ToString("HH:mm:ss");
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex.ToString());
            return "";
        }

    }



}
