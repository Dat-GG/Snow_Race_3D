using UnityBase.Base.Controller;
using UnityEngine;
using UnityEngine.EventSystems;
namespace UnityBase.Base.UI.Effect
{
    [DisallowMultipleComponent]
	public class EffectBtnScale : BehaviourController, IPointerUpHandler, IPointerDownHandler,IEffect
	{
		public float toScale = 1.08f;
		public float fromScale = 1f;
		public void SetFromScale(float value)
        {
			fromScale = value;
		}
		public void OnPointerUp(PointerEventData eventData)
		{
			ScaleUpdate(Vector3.one* fromScale);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			ScaleUpdate(Vector3.one * toScale);
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
