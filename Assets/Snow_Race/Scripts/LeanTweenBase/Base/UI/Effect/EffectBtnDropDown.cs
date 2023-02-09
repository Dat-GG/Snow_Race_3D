using UnityBase.Base.Controller;
using UnityEngine;
namespace UnityBase.Base.UI.Effect
{
    public class EffectBtnDropDown : BehaviourController,IEffect
    {
        float openSize = 798f;
        float closeSize = 235f;

        [SerializeField]
        GameObject blurImage;

        RectTransform currentRect;

        //public CreateRoomType roomType = CreateRoomType.PLO;
        private void Awake()
        {
            _LeanTweenType = LeanTweenType.linear;
            timePerforme = 0.5f;
            currentRect = GetComponent<RectTransform>();
            //currentRect.sizeDelta = new Vector2(currentRect.rect.width, closeSize);
        }

        public void HideEffect()
        {
            blurImage.SetActive(true);
            UpdateValue(currentRect.rect.height, closeSize, OnUpdate);
        }

        private void OnUpdate(float value)
        {
            currentRect.sizeDelta = new Vector3(currentRect.rect.width, value);
        }

        public void ShowEffect()
        {
            blurImage.SetActive(false);
            UpdateValue(currentRect.rect.height, openSize, OnUpdate);
        }
    }
}
