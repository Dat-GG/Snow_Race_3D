using UnityEngine;
using UnityEngine.UI;

public class UINoAdsController : MonoBehaviour
{
    [SerializeField] private Button priceBtn;
    [SerializeField] private Button exitBtn;

    private void Awake()
    {
        priceBtn.onClick.AddListener(OnClickPriceBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    private void OnClickPriceBtn()
    {
        AppManager.Instance.OnNoAdsBuy();
    }

    private void OnClickExitBtn()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
}
