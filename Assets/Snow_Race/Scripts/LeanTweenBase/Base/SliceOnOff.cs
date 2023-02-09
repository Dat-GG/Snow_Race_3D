
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SliceOnOff : MonoBehaviour, IPointerClickHandler
{
    public bool isOn = true;
    [SerializeField] Image circle, bgSlice;
    [SerializeField] Transform pointOn, pointOff;
    public System.Action onClickOn, onClickOff;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(isOn) 
        {
            LeanTween.moveLocal(circle.gameObject, pointOff.localPosition, 0.1f).setOnComplete(()=>
            {
                if(onClickOn != null) onClickOff.Invoke();
            });
            bgSlice.color = Color.black;
            isOn = false;
        }
        else
        {
            LeanTween.moveLocal(circle.gameObject, pointOn.localPosition, 0.1f).setOnComplete(()=>
            {
                if(onClickOn != null) onClickOn.Invoke();
            });
            bgSlice.color = Color.yellow;
            isOn = true;
        }
    }
    public void SetOn(bool On)
    {
        if(On) 
        {
            LeanTween.moveLocal(circle.gameObject, pointOn.localPosition, 0.1f).setOnComplete(()=>
            {
                if(onClickOn != null) onClickOn.Invoke();
            });
            bgSlice.color = Color.yellow;
            isOn = true;
        }
        else
        {
            LeanTween.moveLocal(circle.gameObject, pointOff.localPosition, 0.1f).setOnComplete(()=>
            {
                if(onClickOn != null) onClickOff.Invoke();
            });
            bgSlice.color = Color.black;
            isOn = false;
            
        }
    }
}
