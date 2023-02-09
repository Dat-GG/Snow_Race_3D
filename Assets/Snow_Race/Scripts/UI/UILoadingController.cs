using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UILoadingController : MonoBehaviour
{
    [SerializeField] private Image fill;

    private void Start()
    {
        UnityAction actionComplete =
            () =>
            {
                gameObject.SetActive(false);
            };

        float value = 0;
        DOTween.To(() => value, x => value = x, 1, 2.5f)
            .OnUpdate(() => { fill.fillAmount = value; })
            .OnComplete(() =>
            {
                actionComplete.Invoke();
                fill.fillAmount = 1;
            }).SetUpdate(true);
    }
}
