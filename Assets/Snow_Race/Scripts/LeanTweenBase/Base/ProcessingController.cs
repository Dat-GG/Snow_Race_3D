using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProcessingController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float value;
    [SerializeField]
    protected int Max;

    public float currentValue;
    protected float Value
    {
        set
        {
            if(value < 0)
            {
                this.value = 0;
            }
            else if(value > Max)
            {
                this.value = Max;
            }
            else
            {
                this.value = value;
            }    
        }
        get
        {
            return value;
        }
    }
    protected abstract void UpdateValue(float value);

    protected virtual void Update()
    {
        UpdateValue(currentValue);
        /*currentValue = Mathf.Lerp(currentValue, value, 0.9f);
        Vector3 newScale = new Vector3(
            (float)currentValue / Max,
            transform.localScale.y,
            transform.localScale.z
            );
        transform.localScale = newScale;*/
        //UpdateValue(currentValue);
    }
}
