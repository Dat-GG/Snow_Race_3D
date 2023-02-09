using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityBase.Base.Controller;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ProcessController
{
    
    public delegate void Die();
    public Die dieEvent;
    public delegate void Full();
    public Full fullEvent;
    public Transform ui;
    public Image healthBar;
    public Image realHP;

    Transform target;
    public bool startWithFull = false;
    public bool needFollowCamera = true;
    public float max
    {
        get
        {
            return (float)maxValue;
        }
    }
    public float HP
    {
        get
        {
            return currentsValue;
        }
    }
    private void Awake()
    {
        if(startWithFull) currentsValue = maxValue;
    }
    protected virtual void Start()
    {
        //healthBar = ui.GetChild(0).GetComponent<Image>();
        //realHP = healthBar.transform.GetChild(0).GetComponent<Image>();
        //healthBar.enabled = false;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void PlusMax(int value)
    {
        maxValue += value;
        if(currentsValue > maxValue)
        {
            EditValue(maxValue);
        }
        else
        {
            EditValue(currentsValue += value);
        }
    }
    public void SetMaxHP(float newHP)
    {
        maxValue = newHP;
        currentsValue = newHP;
        EditValue(currentsValue);
    }
    public void SetCurrentHP(float ChangeValue)
    {
        EditValue(ChangeValue);
    }
    public void ChangeHP(float changeHP)
    {
        currentsValue -= changeHP;
        EditValue(currentsValue);
        if (currentsValue < 1f)
        {
            currentsValue = 0;
            if (dieEvent != null)
            {
                dieEvent();
            }
        }
        if(currentsValue >= max)
        {
            realHP.fillAmount = 0;
            healthBar.fillAmount = 0;
            EditValue(maxValue);
            if (fullEvent != null)
            {
                fullEvent();
            }
        }
    }

    protected override void OnUpdate(float value)
    {
        realHP.fillAmount = (currentsValue) / maxValue;
        if (value <= 1f) realHP.fillAmount = 0;
        //float hp = value / maxValue;
        //DOTween.To(() => realHP.fillAmount, x => realHP.fillAmount = x, hp, 0.5f)
        //   .OnUpdate(() => { realHP.fillAmount = hp; })
        //   .OnComplete(() =>
        //   {
        //       realHP.fillAmount = hp;
        //   }).SetUpdate(true);
    }

    private void Update()
    {
        
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (currentsValue - 1) / maxValue, Time.deltaTime * 10f);
    }
    private void LateUpdate()
    {
        if(needFollowCamera) transform.forward = Camera.main.transform.forward;
        //if(target)
        //{
        //transform.position = target.position;

        //}
    }
}
