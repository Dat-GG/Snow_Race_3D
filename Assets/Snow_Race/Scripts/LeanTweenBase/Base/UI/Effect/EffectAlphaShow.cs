using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityBase.Base.Controller;
namespace UnityBase.Base.UI.Effect
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EffectAlphaShow : BehaviourController,IEffect
    {
        CanvasGroup alphaGroup;
        protected System.Action _HildeEndEvent;
        protected System.Action _ShowEndEvent;
        [SerializeField]
        float firstAlpha,lastAlpha;
        bool isShow = false, isHide = false;
        public float currentAlpha { get { return alphaGroup.alpha; } }
 
        private void Awake()
        {
            alphaGroup = GetComponent<CanvasGroup>();
            //originColor = graphic.color;
        }

        public void HideEffect()
        {
            UpdateValue(lastAlpha, firstAlpha, UpdateAlpha,()=>
            {
                gameObject.SetActive(false);
                if (_HildeEndEvent != null) _HildeEndEvent();

                _HildeEndEvent = null;
            });
        }
        public void ShowNormal()
        {
            gameObject.SetActive(true);
            alphaGroup.alpha = 1;
        }
        public void ShowEffect()
        {
            gameObject.SetActive(true);
            UpdateValue(firstAlpha, lastAlpha, UpdateAlpha, ()=> 
            {
                if (_ShowEndEvent != null) _ShowEndEvent();

                _ShowEndEvent = null;
            });

        }

        public void ImHideEffect()
        {
            alphaGroup.alpha = firstAlpha;
            gameObject.SetActive(false);
        }

        void UpdateAlpha(float value)
        {
            alphaGroup.alpha = value;
        }
        public virtual void AddHideEndEvent(System.Action hide)
        {
            _HildeEndEvent = hide;
        }
        public virtual void AddShowEndEvent(System.Action show)
        {
            _ShowEndEvent = show;
        }
    }
}
