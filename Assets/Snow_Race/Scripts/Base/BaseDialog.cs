using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class BaseDialog : MonoBehaviour
{
    private Animator _animator;
    private string _fadeIn = "FadeIn";
    private string _fadeOut = "FadeOut";
    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    protected virtual void DoAppear()
    {
        if (_animator != null) _animator.Play(_fadeIn);
    }

    protected virtual void OnEnable()
    {
        DoAppear();
    }
    
    protected virtual void OnDisable()
    {
        
    }
    
    protected virtual void Close()
    {
        _animator.Play(_fadeOut);
    }

    public virtual void AciveGameObject()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void DeaciveGameObject()
    {
        gameObject.SetActive(false);
    }
}
