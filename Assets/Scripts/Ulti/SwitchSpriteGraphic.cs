using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSpriteGraphic : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    public SpriteRenderer spriteRenderer;
    private bool isActive;

    private void Start()
    {
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // private void Update()
    // {
    //     if(spriteRenderer is null) return;
    //     
    //     spriteRenderer.sprite = isActive ? sprites[1] : sprites[0];
    // }

    public void SwitchSprite(bool isActive)
    {
        this.isActive = isActive;
        
        if(spriteRenderer is null) return;
        spriteRenderer.sprite = isActive ? sprites[1] : sprites[0];
    }
}
