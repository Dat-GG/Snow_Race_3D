using com.adjust.sdk;
using UnityEngine;

public class AdjustController : MonoBehaviour
{
    public static AdjustController Instance;
    public string appToken = "kc6zdfwx9zpc";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        AdjustEnvironment environment = AdjustEnvironment.Production;
        AdjustConfig adjustConfig = new AdjustConfig(appToken, AdjustEnvironment.Production);
        adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
        adjustConfig.setLogDelegate(msg => Debug.Log(msg));
        adjustConfig.setEventSuccessDelegate(EventSuccessCallback);
        adjustConfig.setEventFailureDelegate(EventFailureCallback);
        adjustConfig.setSessionSuccessDelegate(SessionSuccessCallback);
        adjustConfig.setSessionFailureDelegate(SessionFailureCallback);
        adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
        adjustConfig.setAttributionChangedDelegate(AttributionChangedCallback);
        Adjust.start(adjustConfig);
    }

    private void AttributionChangedCallback(AdjustAttribution attributionData)
    {
        Debug.Log("Attribution changed!");

        if (attributionData.trackerName != null)
        {
            Debug.Log("Tracker name: " + attributionData.trackerName);
        }
        if (attributionData.trackerToken != null)
        {
            Debug.Log("Tracker token: " + attributionData.trackerToken);
        }
        if (attributionData.network != null)
        {
            Debug.Log("Network: " + attributionData.network);
        }
        if (attributionData.campaign != null)
        {
            Debug.Log("Campaign: " + attributionData.campaign);
        }
        if (attributionData.adgroup != null)
        {
            Debug.Log("Adgroup: " + attributionData.adgroup);
        }
        if (attributionData.creative != null)
        {
            Debug.Log("Creative: " + attributionData.creative);
        }
        if (attributionData.clickLabel != null)
        {
            Debug.Log("Click label: " + attributionData.clickLabel);
        }
        if (attributionData.adid != null)
        {
            Debug.Log("ADID: " + attributionData.adid);
        }
    }

    private void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
    {
        Debug.Log("Event tracked successfully!");

        if (eventSuccessData.Message != null)
        {
            Debug.Log("Message: " + eventSuccessData.Message);
        }
        if (eventSuccessData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
        }
        if (eventSuccessData.Adid != null)
        {
            Debug.Log("Adid: " + eventSuccessData.Adid);
        }
        if (eventSuccessData.EventToken != null)
        {
            Debug.Log("EventToken: " + eventSuccessData.EventToken);
        }
        if (eventSuccessData.CallbackId != null)
        {
            Debug.Log("CallbackId: " + eventSuccessData.CallbackId);
        }
        if (eventSuccessData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
        }
    }

    private void EventFailureCallback(AdjustEventFailure eventFailureData)
    {
        Debug.Log("Event tracking failed!");

        if (eventFailureData.Message != null)
        {
            Debug.Log("Message: " + eventFailureData.Message);
        }
        if (eventFailureData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + eventFailureData.Timestamp);
        }
        if (eventFailureData.Adid != null)
        {
            Debug.Log("Adid: " + eventFailureData.Adid);
        }
        if (eventFailureData.EventToken != null)
        {
            Debug.Log("EventToken: " + eventFailureData.EventToken);
        }
        if (eventFailureData.CallbackId != null)
        {
            Debug.Log("CallbackId: " + eventFailureData.CallbackId);
        }
        if (eventFailureData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
        }

        Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
    }

    private void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
    {
        Debug.Log("Session tracked successfully!");

        if (sessionSuccessData.Message != null)
        {
            Debug.Log("Message: " + sessionSuccessData.Message);
        }
        if (sessionSuccessData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
        }
        if (sessionSuccessData.Adid != null)
        {
            Debug.Log("Adid: " + sessionSuccessData.Adid);
        }
        if (sessionSuccessData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
        }
    }

    private void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
    {
        Debug.Log("Session tracking failed!");

        if (sessionFailureData.Message != null)
        {
            Debug.Log("Message: " + sessionFailureData.Message);
        }
        if (sessionFailureData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
        }
        if (sessionFailureData.Adid != null)
        {
            Debug.Log("Adid: " + sessionFailureData.Adid);
        }
        if (sessionFailureData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
        }

        Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
    }

    private void DeferredDeeplinkCallback(string deeplinkURL)
    {
        Debug.Log("Deferred deeplink reported!");

        if (deeplinkURL != null)
        {
            Debug.Log("Deeplink URL: " + deeplinkURL);
        }
        else
        {
            Debug.Log("Deeplink URL is null!");
        }
    }
    

/// Hàm tạm để sử lý push len trong   max các bạn để trong event của, max theo hương dẫn
/// ======== MAx ===========
    public void SendRevAdjust(string adUnitId, MaxSdkBase.AdInfo impressionData )
    {
        double revenue = impressionData.Revenue;
        AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        adRevenue.setRevenue(revenue / 1000000f, "USD");
        Debug.LogError("=> Adjust Value : " + (revenue));
        Debug.LogError("=> Adjust 1000000 : " + (revenue / 1000000f));
        Adjust.trackAdRevenue(adRevenue);
    }

    // public void SendPaidToFirebase(string adUnitId, MaxSdkBase.AdInfo impressionData)
    // {
    //     double revenue = (impressionData.Revenue);
    //     var impressionParameters = new[] {
    //         new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
    //         new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
    //         new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
    //         new Firebase.Analytics.Parameter("ad_format", impressionData.Placement),
    //         new Firebase.Analytics.Parameter("value", revenue),
    //         new Firebase.Analytics.Parameter("currency", "USD"), // All Applovin revenue is sent in USD
    //     };
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    // }
}
