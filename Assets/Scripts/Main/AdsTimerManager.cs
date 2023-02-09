using FoodZombie;
using UnityEngine;
using Utilities.Service.RFirebase;
using static GameConstant;

public class AdsTimerManager : Singleton<AdsTimerManager>
{
    private float _lastIntern = 0;
    private float _lastReward = 0;
    private float _time;
    // private FirebaseConfigIntern ValueIntern =>
    //     JsonUtility.FromJson<FirebaseConfigIntern>(RFirebaseRemote.Instance.GetStringValue(GameConstant.Ads.INTER_ADS_CONFIG, GameConstant.Ads.INTER_ADS_DEFAULT));
    //
    // private FirebaseConfigReward ValueReward =>
    //     JsonUtility.FromJson<FirebaseConfigReward>(RFirebaseRemote.Instance.GetStringValue(GameConstant.Ads.REWARD_ADS_CONFIG, GameConstant.Ads.REWARD_ADS_DEFAULT));

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 40)
        {
            if (GameSave.CountFistOpen >= 2)
            {
                ViewInternAds("Auto");
                _time = 0;
            }
            else
            {
                GameSave.CountFistOpen++;
                _time = 0;
              
            }
        }
    }

    //Time.realtimeSinceStartup
    public void ViewInternAds(string ev)
    {
        // if (!IsHaveAds)
        // {
        //     return;
        // }
        if (GameSave.NoAds)
        {
            Debug.LogError("No Ads");
            return;
        }

        // if (Time.realtimeSinceStartup - _lastIntern < ValueIntern.InterTime)
        // {
        //     return;
        // }
            

        Debug.LogError("Run Ads");
        GameTracking.LogEvent("show_interstitial_ads_" + ev);
        bool view = AdsHelper.__ShowInterstitialAd(ev);
        if (view)
        {
            _lastIntern = Time.realtimeSinceStartup;
        }
    }
}
