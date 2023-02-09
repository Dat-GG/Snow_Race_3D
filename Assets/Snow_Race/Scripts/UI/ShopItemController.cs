using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Service;

public class ShopItemController : MonoBehaviour
{
    [SerializeField] private Text txtName, txtPrizeCoin, txtPrizeMoney, txtUnlock;
    [SerializeField] private Text txtAdsValue;
    [SerializeField] private Image imgStatus, imgPreview;
    [SerializeField] private Sprite imgUnloced, imgSelect;
    [SerializeField] private GameObject objSelect;
    [SerializeField] private GameObject objCoinView;
    //[SerializeField] private Button btnBuyCoin, btnBuyVideo, btnBuyMoney;
    [SerializeField] private Button btnUnlocked;
    UIShopController.Type typeShop;
    
    ItemData Data;
    System.Action<ItemData> OnClick;

    private void Awake()
    {
        // btnBuyCoin.onClick.AddListener(OnBuyClick);
        // btnBuyVideo.onClick.AddListener(OnBuyClick);
        // btnBuyMoney.onClick.AddListener(OnBuyClick);
    }

    private void OnDestroy()
    {
        GameUtils.RemoveHandler((GameEvent.OnSkinChange act) => OnSkinChangeEvent());
        // GameUtils.RemoveHandler((GameEvent.OnBallChange act) => OnSkinChangeEvent());
        // GameUtils.AddHandler<GameEvent.OnIAPInitDoneEvent>(OnIAPInintDoneEvent);
    }
    
    private void Start()
    {
        GameUtils.AddHandler((GameEvent.OnSkinChange act) => OnSkinChangeEvent());
        // GameUtils.AddHandler((GameEvent.OnBallChange act) => OnSkinChangeEvent());
        // GameUtils.AddHandler<GameEvent.OnIAPInitDoneEvent>(OnIAPInintDoneEvent);
        //OnSkinChangeEvent();
    }
    
    // private void OnIAPInintDoneEvent(GameEvent.OnIAPInitDoneEvent obj)
    // {
    //     if (this.Data != null && this.Data.unlockType == ItemData.UnlockType.MONEY)
    //         txtPrizeMoney.text = PaymentHelper.Instance.GetLocalizedPriceString(this.Data.IAPID);
    // }

    private void OnSkinChangeEvent()
    {
        InitData(Data, OnClick, typeShop);
    }

    private void OnBuyClick()
    {
        // throw new NotImplementedException();
        if (Data == null)
            return;
        if (Data.isBuy)
        {
            txtUnlock.text = "Unlocked";
        }
        else
        {
            switch (Data.unlockType)
            {
                case ItemData.UnlockType.COIN:
                    if (GameSave.PlayerCoin >= Data.priceCoin)
                    {
                        GameSave.PlayerCoin -= Data.priceCoin;
                        Data.isBuy = true;
                        Data.Select();
                        //UIConfirmController.Instance.InitConfirm("Info", "Buy success");
                        GameUtils.RaiseMessage(GameEvent.OnResourceChange.Instance);
                        GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
                        //MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Claim);
                    }
                    else
                    {
                        //UIConfirmController.Instance.InitConfirm("Warning", "Not enought coins");
                    }
                    break;
                case ItemData.UnlockType.MONEY:
                    //PaymentHelper.Purchase(Data.IAPID, OnBuyDoneEvent);
                    break;
                case ItemData.UnlockType.VIDEO:
                    //AdsHelper.__ShowVideoRewardedAd("BuyItem_" + typeShop.ToString() + "_" + Data.ID, OnWatchedVideoDoneEvent);
                    break;
                default:
                    break;
            }

            InitData(Data, OnClick, typeShop);
        }
    }

    private void OnBuyDoneEvent(bool obj)
    {
        if (obj)
        {
            Data.isBuy = true;
            Data.Select();
            InitData(Data, OnClick, typeShop);
            GameTracking.LogEvent("IAP_" + typeShop.ToString() + "_" + Data.ID);
            if (typeShop == UIShopController.Type.SKIN)
            {
                GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
            }
            else if (typeShop == UIShopController.Type.BALL)
            {
                //GameUtils.RaiseMessage(GameEvent.OnBallChange.Instance);
            }
        }
    }

    private void OnWatchedVideoDoneEvent(bool obj)
    {
        if (obj)
        {
            this.Data.AdsWatched();
            if (this.Data.AdsWatched_Get >= this.Data.priceCoin)
                this.Data.isBuy = true;
            Data.Select();
            if (typeShop == UIShopController.Type.SKIN)
            {
                GameTracking.LogEvent("Buy_skin_" + Data.ID);
                GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
            }
            else if (typeShop == UIShopController.Type.BALL)
            {
                GameTracking.LogEvent("Buy_ball_" + Data.ID);
                //GameUtils.RaiseMessage(GameEvent.OnBallChange.Instance);
            }
            InitData(Data, OnClick, typeShop);
        }
    }

    public void InitData(ItemData data, Action<ItemData> onClick, UIShopController.Type type)
    {
        OnClick = onClick;
        typeShop = type;
        Data = data;
        txtName.text = data.name;
        if (data.isBuy)
        {
            // btnBuyCoin.gameObject.SetActive(false);
            // btnBuyMoney.gameObject.SetActive(false);
            // btnBuyVideo.gameObject.SetActive(false);
            btnUnlocked.gameObject.SetActive(true);
            if (!data.isSelect())
            {
                txtUnlock.text = "Claimed";
                btnUnlocked.image.sprite = imgUnloced;
            }
            else
            {
                txtUnlock.text = "Selected";
                btnUnlocked.image.sprite = imgSelect;
            }
        }
        else
        {
            btnUnlocked.gameObject.SetActive(false);
            // btnBuyCoin.gameObject.SetActive(data.unlockType == ItemData.UnlockType.COIN);
            // btnBuyMoney.gameObject.SetActive(data.unlockType == ItemData.UnlockType.MONEY);
            // btnBuyVideo.gameObject.SetActive(data.unlockType == ItemData.UnlockType.VIDEO);

            switch (data.unlockType)
            {
                case ItemData.UnlockType.COIN:
                    txtPrizeCoin.text = data.priceCoin.ToString("0000");
                    break;
                case ItemData.UnlockType.MONEY:
                    txtPrizeMoney.text = data.IAPPrice + "$";
                    break;
                case ItemData.UnlockType.VIDEO:
                    txtAdsValue.text = data.AdsWatched_Get + "/" + data.priceCoin;
                    break;
                default:
                    break;
            }
            
            try
            {
                if (PaymentHelper.Instance != null && AppManager.IsIAPInited)
                    if (Data != null && Data.unlockType == ItemData.UnlockType.MONEY)
                        txtPrizeMoney.text = PaymentHelper.Instance.GetLocalizedPriceString(Data.IAPID);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        imgPreview.sprite = data.sprView;
    }
    public void OnClickEvent()
    {
        OnClick?.Invoke(Data);
    }

    public void CheckSelect(ItemData Data)
    {
        objSelect.SetActive(this.Data.Equals(Data));
        InitData(this.Data, this.OnClick, typeShop);
    }

    public void CheckSelect(string ID)
    {
        objSelect.SetActive(this.Data.ID.Equals(ID));
        InitData(this.Data, this.OnClick, typeShop);
    }
}
