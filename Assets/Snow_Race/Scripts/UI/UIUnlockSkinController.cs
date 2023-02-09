using DG.Tweening;
using FoodZombie;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIUnlockSkinController : MonoBehaviour
{
    [SerializeField] private Button adsBtn;
    [SerializeField] private Button noTksBtn;
    [SerializeField] private UIShopController _shopController;

    private void Awake()
    {
        adsBtn.onClick.AddListener(OnClickAdsBtn);
        noTksBtn.onClick.AddListener(OnClickNoTksBtn);
    }

    private void OnClickAdsBtn()
    {
        AdsHelper.__ShowVideoRewardedAd("AdsUIUnlockSkin", OnViewRewardDone);
    }

    private ItemData _item;
    
    private void OnViewRewardDone(bool obj)
    {
        if (obj)
        {
            if (GameSave.PlayerLevel == 2)
            {
                _item = ShopData.Instance.itemData[1];
            }
            else if (GameSave.PlayerLevel == 5)
            {
                _item = ShopData.Instance.itemData[7];
            }
            
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
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    private void OnClickNoTksBtn()
    {
        noTksBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        DOVirtual.DelayedCall(3f, delegate
        {
            noTksBtn.gameObject.SetActive(true);
        });
    }
}
