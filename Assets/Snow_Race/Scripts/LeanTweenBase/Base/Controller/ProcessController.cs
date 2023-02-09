using System;
using UnityEngine;

namespace UnityBase.Base.Controller
{
    public abstract class ProcessController : BehaviourController
    {
        [SerializeField]
        protected float maxValue;
        protected float currentsValue;
        protected void EditValue(float value,Action OnCompleteProcessing = null){
            float previousValue = currentsValue;
            currentsValue = value;
            if (currentsValue <= 0) {
                currentsValue = 0;
            };
            if (currentsValue > maxValue) currentsValue = maxValue;
            UpdateValue(previousValue,currentsValue,OnUpdate,OnCompleteProcessing);
        }

        protected void SetValue(float value)
        {
            currentsValue = value;
            if (currentsValue <= 0) {
                currentsValue = 0;
            };
            if (currentsValue > maxValue) currentsValue = maxValue;
            OnUpdate(currentsValue);
        }

        protected abstract void OnUpdate(float value);
    }
}
