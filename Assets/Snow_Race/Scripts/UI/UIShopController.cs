using System;
using System.Collections;
using System.Collections.Generic;
using FoodZombie;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.Common;
using Random = UnityEngine.Random;

public class UIShopController : MonoBehaviour
{
    [SerializeField] Text txtCoin, txtSelectPlayer, txtSelectFly, txtSelectBasket;
    // [SerializeField] private Button btnSelectSkin, btnSelectFly, btnSelectBasket;
    // [SerializeField] private GameObject[] objTabs;
    [SerializeField] private int unlockGold;
    [SerializeField] private Button goldUnlockBtn;
    [SerializeField] private Button adsUnlockBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private UIConfirmController uiConfirm;
    [SerializeField] private GameObject uiInternetCheck;

    public enum Type
    {
        SKIN,
        BALL
    }

    [SerializeField] private PlayerSkinLoader playerSkinView;
    [SerializeField] private ShopItemController shopItemController;
    [SerializeField] RectTransform panelPlayerShop;

    private List<ShopItemController> _itemViewsPlayer;
    ItemData[] PlayerShopData => ShopData.Instance.itemData;
    bool Load = false;

    private ItemData _item;

    private void Awake()
    {
        // btnSelectSkin.onClick.AddListener(() => { SelectTab(0); });
        // btnSelectFly.onClick.AddListener(() => { SelectTab(1); });
        // btnSelectBasket.onClick.AddListener(() => { SelectTab(2); });
        if (!Load)
        {
            LoadShop();
        }
        exitBtn.onClick.AddListener(OnClickExitBtn);
        goldUnlockBtn.onClick.AddListener(OnClickGoldUnlockBtn);
        adsUnlockBtn.onClick.AddListener(OnClickAdsUnlockBtn);
        //SelectTab(0);
    }

    private void OnClickGoldUnlockBtn()
    {
        if (GameSave.PlayerCoin >= unlockGold)
        {
            GameSave.PlayerCoin -= unlockGold;
            txtCoin.text = GameSave.PlayerCoin.ToString("0000");
            _item = GetRandomSkinByCoin();
            Debug.Log(_item);
            if (_item != null)
            {
                _item.isBuy = true;
                uiConfirm.SetActive(true);
                uiConfirm.InitConfirm("Info", "Buy success");
                if (_item.itemType == ItemData.ItemType.PLAYER)
                {
                    OnPlayerItemSelectEvent(_item);
                    GameUtils.RaiseMessage(GameEvent.OnResourceChange.Instance);
                    GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
                }
            }
            //MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Claim);
        }
        else
        {
            uiConfirm.SetActive(true);
            uiConfirm.InitConfirm("Warning", "Not enough gold");
        }
    }

    private void OnClickAdsUnlockBtn()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                AdsHelper.__ShowVideoRewardedAd("AdsUnlockSkin", OnViewRewardDone);
            }
            else
            {
                uiInternetCheck.SetActive(true);
            }
        }));
        
    }
    
    private void OnViewRewardDone(bool obj)
    {
        if (obj)
        {
            if (GameSave.PlayerLevel < 5)
            {
                _item = GetRandomSkinByCoin();
            }
            else
            {
                _item = GetRandomSkinByAds();
            }
            Debug.Log(_item);
            if (_item != null)
            {
                _item.isBuy = true;
                if (_item.itemType == ItemData.ItemType.PLAYER)
                {
                    OnPlayerItemSelectEvent(_item);
                    GameUtils.RaiseMessage(GameEvent.OnResourceChange.Instance);
                    GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
                }
            }
        }
    }

    private void OnClickExitBtn()
    {
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    void LoadShop()
    {
        Load = true;
        _itemViewsPlayer = new List<ShopItemController>();

        foreach (ItemData item in PlayerShopData)
        {
            var _shop = Instantiate(shopItemController, panelPlayerShop);
            _shop.InitData(item, OnPlayerItemSelectEvent, Type.SKIN);
            _itemViewsPlayer.Add(_shop);
        }
    }

    private ItemData GetRandomSkinByCoin()
    {
        List<ItemData> list = new List<ItemData>();
        foreach (ItemData item in PlayerShopData)
        {
            if (!item.isBuy && item.unlockType == ItemData.UnlockType.COIN)
            {
                list.Add(item);
            }
        }
        if (list.Count == 0)
        {
            return null;
        }
        else return list[Random.Range(0, list.Count)];
    }
    
    private ItemData GetRandomSkinByAds()
    {
        List<ItemData> list = new List<ItemData>();
        foreach (ItemData item in PlayerShopData)
        {
            if (!item.isBuy && item.unlockType != ItemData.UnlockType.MONEY)
            {
                list.Add(item);
            }
        }
        if (list.Count == 0)
        {
            return null;
        }
        else return list[Random.Range(0, list.Count)];
    }

    public void OnPlayerItemSelectEvent(ItemData obj)
    {
        //GameplayManager.Instance.ShowUIPlayer(obj);
        // if (obj.mat != null)
        // {
        //     playerSkinView.TargetMat = obj.mat;
        //     playerSkinView.LoadSkin();
        // }
        if (playerSkinView == null)
        {
            return;
        }
        playerSkinView.LoadView(obj.prefab);
        // Debug.LogError("OnPlayerItemSelectEvent");
        if (obj.isBuy)
        {
            //GameSave.SkinPlayer = obj.ID;
            obj.Select();
            GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
        }
        if (!Load)
        {
            LoadShop();
        }
        foreach (var item in _itemViewsPlayer)
        {
            item.CheckSelect(obj);
        }
    }

    private void OnEnable()
    {
        MusicManager.Instance.PlayMusic(MusicDB.Instance.Music_Shop);
        txtCoin.text = GameSave.PlayerCoin.ToString("0000");
        //LoadShop();
        //Check Select:
        foreach (var item in _itemViewsPlayer)
        {
            item.CheckSelect(GameSave.SkinPlayer);
        }
        
        var _skinPlayer = ShopData.Instance.PlayerSkinSelect();
        if (_skinPlayer != null && _skinPlayer.mat != null)
        {
            //GameplayManager.Instance.ShowUIPlayerCurrent();

            // playerSkinView.TargetMat = _skinPlayer.mat;
            // playerSkinView.LoadSkin();
            playerSkinView.LoadView(_skinPlayer.prefab);
        }
        // Time.timeScale = 0;
    }

    private void OnDisable()
    {
        MusicManager.Instance.PlayMusic(MusicDB.Instance.Music_BG);
        var _skinPlayer = ShopData.Instance.PlayerSkinSelect();
        if (_skinPlayer != null && _skinPlayer.mat != null)
        {
            //GameplayManager.Instance.ShowUIPlayerCurrent();

            // playerSkinView.TargetMat = _skinPlayer.mat;
            // playerSkinView.LoadSkin();
            playerSkinView.LoadView(_skinPlayer.prefab);
        }
        //   Time.timeScale = 1;
    }
    
    private IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("https://google.com");
        yield return request.SendWebRequest();
        if (request.error != null) {
            Debug.Log ("Error");
            action (false);
        } else{
            Debug.Log ("Success");
            action (true);
        }
    }
    
    // public void SelectTab(int index)
    // {
    //     Color _blue = Color.blue;
    //     ColorUtility.TryParseHtmlString("#4DA1FF", out _blue);
    //     txtSelectPlayer.color = index == 0 ? Color.white : _blue;
    //     txtSelectFly.color = index == 1 ? Color.white : _blue;
    //     txtSelectBasket.color = index == 2 ? Color.white : _blue;
    //
    //     txtSelectPlayer.text = index == 0 ? "<u>PLAYER</u>" : "PLAYER";
    //     txtSelectFly.text = index == 1 ? "<u>FLY</u>" : "FLY";
    //     txtSelectBasket.text = index == 2 ? "<u>BASKET</u>" : "BASKET";
    //
    //     foreach (var item in objTabs)
    //     {
    //         item.gameObject.SetActive(false);
    //     }
    //     objTabs[index].SetActive(true);
    // }
}
