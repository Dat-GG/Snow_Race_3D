using System;
using UnityEngine;
namespace UnityBase.Base.Controller
{
    public class BehaviourController : MonoBehaviour
    {
        public float timePerforme = 0.3f;
        public float timeClose = 0.02f;
        private int id;

	    public LeanTweenType _LeanTweenType = LeanTweenType.easeSpring;
        public LeanTweenType _LeanTweenCloseType = LeanTweenType.linear;

        public void MoveToPos(Vector3 posMoveTo,Action callBackComplete = null)
	    {
		    LeanTween.move (gameObject, posMoveTo, timePerforme).setEase(_LeanTweenType).setOnComplete(callBackComplete);
	    }

        public void MoveToPosLocal(Vector3 posMoveTo,Action callBackComplete = null)
        {
            LeanTween.moveLocal(gameObject, posMoveTo, timePerforme).setEase(_LeanTweenType).setOnComplete(callBackComplete);
        }

        protected void MoveUpdate(Vector3 posMoveTo, Action callBackComplete = null)
	    {
		    LeanTween.cancel (id);
		    id = LeanTween.move(gameObject, posMoveTo, timePerforme).setEase(_LeanTweenType).setOnComplete(callBackComplete).id;
	    }

        public void MoveUpdateLocal(Vector3 posMoveTo,Action callBackComplete = null)
            {
            LeanTween.cancel(id);
            id = LeanTween.moveLocal(gameObject, posMoveTo, timePerforme).setOnComplete(callBackComplete).setEase(_LeanTweenType).id;
        }
        public void RotationTo(Vector3 rotationTo,Action callBackComplete = null)
        {
            LeanTween.rotate(gameObject, rotationTo, timePerforme).setEase(_LeanTweenType).setOnComplete(callBackComplete);
        }
        protected void RotationUpdate(Vector3 rotationTo)
	    {
		    LeanTween.cancel (id);
		    id = LeanTween.rotate(gameObject, rotationTo, timePerforme).setEase(_LeanTweenType).id;
	    }

	    protected void UpdateValue(float firstValue , float lastValue, Action<float> updateValue = null,Action onComplete = null)
	    {
           LeanTween.cancel(id);
           id = LeanTween.value(gameObject, updateValue,firstValue,lastValue, timePerforme).setEase(_LeanTweenType).setOnComplete(onComplete).id;
	    }

        protected void UpdateValue(Vector2 firstValue, Vector2 lastValue, Action<Vector2> updateValue = null, Action onComplete = null)
        {
            LeanTween.cancel(id);
            id = LeanTween.value(gameObject, updateValue, firstValue, lastValue, timePerforme).setEase(_LeanTweenType).setOnComplete(onComplete).id;
        }
        public void ScaleUpdate(Vector3 ScaleValue)
	    {
		    LeanTween.cancel (id);
		    id = LeanTween.scale (gameObject, ScaleValue, timePerforme).setIgnoreTimeScale(true).setEase(_LeanTweenType).id;
	    }

	    public void ScaleTo(Vector3 ScaleValue,Action callBackComplete = null)
	    {
		    LeanTween.scale (gameObject, ScaleValue, timePerforme).setEase(_LeanTweenType).setOnComplete(callBackComplete).setIgnoreTimeScale(true);
	    }

        protected void ScaleToClose(Vector3 ScaleValue, Action callBackComplete = null)
        {
            LeanTween.scale(gameObject, ScaleValue, timeClose).setEase(_LeanTweenCloseType).setOnComplete(callBackComplete);
        }

        protected void UpdateColor(Color firstColor, Color lastColor, Action<Color> updateValue = null, Action onComplete = null)
	    {
		    LeanTween.cancel(id);
		    id = LeanTween.value(gameObject, updateValue, firstColor, lastColor, timePerforme).setEase(_LeanTweenType).setOnComplete(onComplete).id;
	    }
    }
}

