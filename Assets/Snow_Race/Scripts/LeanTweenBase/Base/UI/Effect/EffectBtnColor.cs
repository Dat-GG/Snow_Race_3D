using UnityBase.Base.Controller;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UnityBase.Base.UI.Effect
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class EffectBtnColor : BehaviourController,IPointerUpHandler, IPointerDownHandler,IEffect
	{
		[SerializeField]
		Color ToColor;

		Color originColor;

		Image img;

        void Start()
        {
			img = GetComponent<Image>();
			originColor = img.color;
        }

		public void OnPointerUp(PointerEventData eventData)
		{
			UpdateColor(ToColor,originColor, UpdateColorImage);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			UpdateColor(originColor, ToColor, UpdateColorImage);
		}

        void UpdateColorImage(Color color)
        {
			img.color = color;
        }

        public void ShowEffect()
        {
            throw new System.NotImplementedException();
        }

        public void HideEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}

