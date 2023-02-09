using FoodZombie;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Service;
using Utilities.Service.Ads;
using Utilities.Service.RFirebase;

public class AppManager : Singleton<AppManager>
{
    public static bool IsIAPInited = false;
    [SerializeField] private UIShopController _shopController;
    private void Awake()
    {
        RFirebaseManager.Init(OnFirebaseInitDone);
    }

    private void OnFirebaseInitDone(bool obj)
    {
        //throw new NotImplementedException();
    }

    private void Start()
    {
        List<string> _iap_ids = ShopData.Instance.IAP_ids;
        _iap_ids.Add(GameConstant.GameIAPID.ID_NO_ADS);
        PaymentHelper.Instance.InitProducts(_iap_ids, new List<string>(), OnInitIAPDone);
        //AdsManager.Instance.ShowBannerAd(BannerAdPosition.Bottom);
    }

    private void OnInitIAPDone(bool obj)
    {
        if (obj)
        {
            IsIAPInited = true;
            GameUtils.RaiseMessage(GameEvent.OnIAPInitDoneEvent.Instance);
        }
    }

    public void OnNoAdsBuy()
    {
        Time.timeScale = 0;
        PaymentHelper.Purchase(GameConstant.GameIAPID.ID_NO_ADS, OnNoAdsDone);
    }

    private ItemData _item;
    private void OnNoAdsDone(bool obj)
    {
        Time.timeScale = 1;
        if (obj)
        {
            GameSave.NoAds = true;
            _item = ShopData.Instance.itemData[3];
            if (_item != null)
            {
                _item.isBuy = true;
                if (_item.itemType == ItemData.ItemType.PLAYER)
                {
                    _shopController.OnPlayerItemSelectEvent(_item);
                    GameUtils.RaiseMessage(GameEvent.OnResourceChange.Instance);
                    GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
                }
            }
        }
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
