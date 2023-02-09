using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Service.Ads;

public class MAXClient : AdClient
{
    private const string NO_SDK_MESSAGE = "SDK missing. Please import the MAX plugin.";
#if ACTIVE_MAX
    protected MAXSettings mAdSettings;
    protected bool mIsBannerAdLoaded = false;
    //  protected MAXBAn mCurrentBannerAdSize = IronSourceBannerSize.SMART;
    //  protected IronSourceBannerPosition mCurrentBannerAdPos = IronSourceBannerPosition.BOTTOM;

    protected bool mRewardedVideoIsCompleted = false;
#endif

    #region Singleton

    private static MAXClient mInstance;

    public static MAXClient CreateClient()
    {
        if (mInstance == null)
            mInstance = new MAXClient();
        return mInstance;
    }

    #endregion


    //===========AdClient Override
    #region AdClient
    public override bool IsSdkAvail
    {
        get
        {
#if ACTIVE_MAX
            return true;
#else
            return false;
#endif
        }
    }

    public override AdNetwork Network { get { return AdNetwork.MAX; } }

    public override bool IsBannerAdSupported { get { return true; } }

    public override bool IsInterstitialAdSupported { get { return true; } }

    public override bool IsRewardedAdSupported { get { return true; } }

    protected override string NoSdkMessage => NO_SDK_MESSAGE;


    public override void Init(AdSettings pSettings)
    {
#if ACTIVE_MAX
        mIsInitialized = true;
        mAdSettings = pSettings as MAXSettings;

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
{

    Debug.LogError("MaxSdkCallbacks init Done");
    // AppLovin SDK is initialized, start loading ads
#if ACTIVE_MAX_TEST
    MaxSdk.ShowMediationDebugger();
#endif
    // Attach callback
    MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (s, o) => { OnInterstitialAdReady(); };
    MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (s, o) => { OnInterstitialAdLoadFailed(null); };
    MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (s, o) => { OnInterstitialAdOpened(); };
    MaxSdkCallbacks.Interstitial.OnAdClickedEvent += (s, o) => { OnInterstitialAdClicked(); };
    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (s, o) => { OnInterstitialAdClosed(); OnInterstitialAdShowSucceeded(); };

    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (s, o, p) => { OnInterstitialAdShowFailed(null); };


    //  MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
    // MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;


    MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (o, p) =>
{

    Debug.LogError("Rewarded.OnAdLoadFailedEvent");
    // StartCoroutine(LoadRewardWithDelay(InterstitialLoadDelayTime));

};
    MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (o, p) =>
    {
        Debug.LogError("Rewarded.OnAdDisplayedEvent");
        OnRewardedVideoAdOpened();
    };
    MaxSdkCallbacks.Rewarded.OnAdClickedEvent += (o, p) =>
    {

        Debug.LogError("Rewarded.OnAdClickedEvent");
        OnRewardedVideoAdClicked(null);

    };
    MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (o, p) =>
    {

        Debug.LogError("Rewarded.OnAdRevenuePaidEvent");
        RewardedOnAdRevenuePaidEvent(o, p);

    };

    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (o, p) =>
    {

        Debug.LogError("Rewarded.OnAdHiddenEvent");
        OnRewardedVideoAdEnded();
    };
    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (o, p, a) =>
    {

        Debug.LogError("Rewarded.OnAdDisplayFailedEvent");
        OnRewardedVideoAdShowFailed(null);

    };

    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdRecivedRewardEvent;

    LoadInterstitialAd();
    LoadRewardedAd();
};
        MaxSdk.SetSdkKey(this.mAdSettings.appId.Id);
        //MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();


#endif
    }



    public override bool IsValid(AdType type)
    {
#if ACTIVE_MAX
        return true;
#else
        return false;
#endif
    }

    protected override void InternalDestroyBannerAd()
    {
        MaxSdk.DestroyBanner(mAdSettings.bannerId.Id);
    }

    protected override void InternalHideBannerAd()
    {
        MaxSdk.HideBanner(mAdSettings.bannerId.Id);
    }

    protected override bool InternalIsInterstitialAdReady()
    {
        return MaxSdk.IsInterstitialReady(mAdSettings.internId.Id);
    }

    protected override bool InternalIsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(mAdSettings.rewardId.Id);
    }

    protected override void InternalLoadInterstitialAd()
    {
        Debug.LogError("InternalLoadInterstitialAd");
        MaxSdk.LoadInterstitial(mAdSettings.internId.Id);
    }

    protected override void InternalLoadRewardedAd()
    {
        Debug.LogError("InternalLoadRewardedAd");
        MaxSdk.LoadRewardedAd(mAdSettings.rewardId.Id);
    }

    protected override void InternalShowBannerAd(BannerAdPosition position, BannerAdSize size)
    {
        MaxSdk.CreateBanner(mAdSettings.bannerId.Id, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(mAdSettings.bannerId.Id, Color.white);
        MaxSdk.ShowBanner(mAdSettings.bannerId.Id);
    }

    protected override void InternalShowInterstitialAd(string id)
    {
        Debug.LogError("InternalShowInterstitialAd");
        MaxSdk.ShowInterstitial(mAdSettings.internId.Id);
    }

    protected override void InternalShowRewardedAd(string id)
    {
        Debug.LogError("InternalShowRewardedAd");
        MaxSdk.ShowRewardedAd(mAdSettings.rewardId.Id);
    }

    #endregion
    ///==========AdClient Override
    ///

    //===========Ad Event Handler
    #region AdEvent
    private void OnInterstitialAdShowFailed(object p)
    {
        //   throw new NotImplementedException();
    }

    private void OnInterstitialAdShowSucceeded()
    {
        //     throw new NotImplementedException();
    }

    private void OnInterstitialAdClosed()
    {
        OnInterstitialAdCompleted();
    }

    private void OnInterstitialAdClicked()
    {
        //   throw new NotImplementedException();
    }

    private void OnInterstitialAdOpened()
    {
        //   throw new NotImplementedException();
    }

    private void OnInterstitialAdLoadFailed(object p)
    {
        Debug.Log("Failed to show interstitial ad");
    }

    private void OnInterstitialAdReady()
    {
        //  throw new NotImplementedException();
    }
    
    private void RewardedOnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //AdjustController.Instance.SendPaidToFirebase(adUnitId, adInfo);
        AdjustController.Instance.SendRevAdjust(adUnitId, adInfo);
    }


    private void OnAdRecivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
    {
        mRewardedVideoIsCompleted = true;
    }

    private void OnRewardedVideoAdShowFailed(object p)
    {
        mRewardedVideoIsCompleted = false;
    }

    private void OnRewardedVideoAdEnded()
    {
        if (!mRewardedVideoIsCompleted)
            OnRewardedAdSkipped();
        else
            OnRewardedAdCompleted();
        // GameData.Instance.GameConfigGroup.UnPauseGame();
        mRewardedVideoIsCompleted = false;
    }

    private void OnRewardedVideoAdClicked(object p)
    {
        //   throw new NotImplementedException();
    }

    private void OnRewardedVideoAdOpened()
    {
        mRewardedVideoIsCompleted = false;
    }
    #endregion
    //===========Ad Event Handler
}

[Serializable]
public class MAXSettings : AdSettings
{
    public AdId appId;
    public AdId bannerId;
    public AdId internId;
    public AdId rewardId;
}
