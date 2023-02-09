using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase.Base.Controller;
namespace UnityBase.Base.UI.Effect
{
    public enum TypeEffect
    {
        FromLeft,
        FromRight,
        FromBottom,
        FromTop
    }

    public class EffectSlide : BehaviourController,IEffect
    {
        [SerializeField]
        TypeEffect typeEffect;
        [SerializeField]
        Vector3 originPos;
        [SerializeField]
        Vector3 moveTo;

        protected System.Action _HildeEndEvent;
        protected System.Action _ShowEndEvent;

        void Awake()
        {
            originPos = new Vector3(transform.position.x, 0, 0);
            // switch(typeEffect)
            // {
            //     case TypeEffect.FromBottom:
            //         hidePos = transform.position + Vector3.down * Screen.height;
            //         break;
            //     case TypeEffect.FromLeft:
            //         hidePos = transform.position + Vector3.left * Screen.width;
            //         break;
            //     case TypeEffect.FromRight:
            //         hidePos = transform.position + Vector3.right * Screen.width;
            //         break;
            //     case TypeEffect.FromTop:
            //         hidePos = transform.position + Vector3.up * Screen.height;
            //         break;
            // }
            
        }

        public void HideEffect()
        {
            gameObject.SetActive(false);
            MoveUpdate(moveTo);
        }

        public void ImHide()
        {
            transform.position = moveTo;
        }

        public void ShowEffect()
        {
            gameObject.SetActive(true);
            //MoveUpdate(hidePos);
            MoveUpdate(moveTo, ()=> 
            {
                if (_ShowEndEvent != null) _ShowEndEvent();

                _ShowEndEvent = null;
            });
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

