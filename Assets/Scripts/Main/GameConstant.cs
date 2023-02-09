using UnityEngine;
using Utilities.Service.RFirebase;

public class GameConstant
{
    // public static bool IsHaveAds =>
    //     RFirebaseRemote.Instance.GetBoolValue(Ads.IS_HAVE_INTER_ADS_CONFIG, true);
    
    public const float GAME_SCALE = 0.7f;

    public class GameScene
    {
        public const string LOADING = "";
    }


    public class GameLayer
    {
        public const int TARGET = 10;
    }

    public class Ads
    {
        public const string INTER_ADS_CONFIG = "InterstitialConfig";
        public const string INTER_ADS_DEFAULT = "{\"InterTime\":40}";
        public const string REWARD_ADS_CONFIG = "RewardedAdsConfig";
        public const string REWARD_ADS_DEFAULT = "{\"DelayRebirth\":0 }";
        public const string IS_HAVE_INTER_ADS_CONFIG = "IS_HAVE_INTER_ADS_CONFIG";
    }

    [SerializeField]
    public class FirebaseConfigIntern
    {
        public int InterTime;
    }
    [SerializeField]
    public class FirebaseConfigReward
    {

    }

    public class GameIAPID
    {
        public const string ID_NO_ADS = "com.snowrace.noads";
    }
}
