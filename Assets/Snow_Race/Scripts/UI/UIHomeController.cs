using System;
using System.Collections;
using FoodZombie;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHomeController : Singleton<UIHomeController>
{
    public Button restart;
    public GameObject canvas;
    [SerializeField] private GameObject uiInternetCheck;
    [SerializeField] private Button tapToPlayBtn;
    [SerializeField] private Image bg;
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject coinInfo;
    [SerializeField] private GameObject editNameBox;
    [SerializeField] private Text levelText;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button adsBtn;
    [SerializeField] private Button noAds;
    
    [SerializeField] private GameObject uiSetting;
    [SerializeField] private GameObject uiShop;
    [SerializeField] private GameObject uiNoAds;
    
    [Header("Hack")]
    [SerializeField] private GameObject uiTest;
    [SerializeField] private InputField idTestLevel;
    [SerializeField] private Button applyBtn;
    [SerializeField] private Button hideBtn;
    private PlayerController _playerController;

    private void Start()
    {
        noAds.gameObject.SetActive(!GameSave.NoAds);
        tapToPlayBtn.onClick.AddListener(OnClickTapToPlayBtn);
        restart.onClick.AddListener(OnClickRestartBtn);
        _playerController = FindObjectOfType<PlayerController>();
        settingBtn.onClick.AddListener(OnClickSettingBtn);
        shopBtn.onClick.AddListener(OnClickShopBtn);
        applyBtn.onClick.AddListener(OnClickApplyBtn);
        hideBtn.onClick.AddListener(OnClickHideBtn);
        adsBtn.onClick.AddListener(OnClickAdsBtn);
        noAds.onClick.AddListener(OnClickNoAdsBtn);
    }

    private void OnEnable()
    {
        levelText.text = "Level " + GameSave.PlayerLevel;
        coinText.text = GameSave.PlayerCoin.ToString("0000");
        MusicManager.Instance.PlayMusic(MusicDB.Instance.Music_BG);
    }

    private void Update()
    {
        bool Incr = Input.GetKeyDown(KeyCode.Space);
        if (Incr)
        {
            OnClickHack();
        }
    }

    private void OnClickNoAdsBtn()
    {
        uiNoAds.SetActive(true);
    }

    private void OnClickTapToPlayBtn()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                GameManager.Instance.ChangeState(GameManager.GameStates.Play);
                tapToPlayBtn.gameObject.SetActive(false);
                adsBtn.gameObject.SetActive(false);
                shopBtn.gameObject.SetActive(false);
                levelText.gameObject.SetActive(false);
                coinInfo.SetActive(false);
                editNameBox.SetActive(false);
                bg.gameObject.SetActive(false);
                canvas.SetActive(true);
                noAds.gameObject.SetActive(false);
            }
            else
            {
                uiInternetCheck.SetActive(true);
            }
        }));
    }

    private void OnClickSettingBtn()
    {
        uiSetting.SetActive(true);
    }

    private void OnClickShopBtn()
    {
        gameObject.SetActive(false);
        uiShop.SetActive(true);
    }

    private void OnClickAdsBtn()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                AdsHelper.__ShowVideoRewardedAd("AdsBonusSizeBall", OnViewRewardDone);
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
            if (_playerController == null)
            {
                return;
            }
            _playerController.BonusBall();
            OnClickTapToPlayBtn();
        }
    }

    public void OnClickRestartBtn()
    {
        restart.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private void OnClickHack()
    {
        if (_playerController.Ball.currentTimeScale < _playerController.Ball.maxTimeScale)
        {
            _playerController.Ball.transform.localScale *= 1.1f;
            _playerController.Ball.currentTimeScale++;
        }
    }

    private void OnClickApplyBtn()
    {
        GameSave.PlayerLevel = int.Parse(idTestLevel.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnClickHideBtn()
    {
        uiTest.SetActive(false);
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
}
