using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityBase.Base;

namespace UnityBase.Base.UI
{
	[DisallowMultipleComponent]
	public class ButtonController : MonoBehaviour, IPointerClickHandler
	{

		protected Action<ButtonController> callBackClick;

        public void OnClick(Action<ButtonController> _callBackClick)
		{
			if (_callBackClick != null)
				callBackClick = _callBackClick;
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			PlayButtonSound();

			if (callBackClick != null)
				callBackClick(this);
		}

		void PlayButtonSound()
        {
			//MySound.Instance.PlaySound(SoundController.Sound.clickBtnSound);
		}
	}
}
