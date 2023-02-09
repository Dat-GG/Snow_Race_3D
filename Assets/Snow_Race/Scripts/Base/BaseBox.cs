using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseBox : MonoBehaviour
{    
	public static Stack<BaseBox> StackBox = new Stack<BaseBox> ();
	public static BaseBox currentBaseBox{
		get{
			//return StackBox.Count > 0 ? StackBox.Peek() : null;
			return null;

		}
	}  
	public delegate void BoxCallbackDelegate();
	public BoxCallbackDelegate OnCloseBox;	   
	[SerializeField]
	protected RectTransform mainPanel;

	[HideInInspector] public BackObj backObj;
	private bool isClose;

    [SerializeField] protected bool isAnim = true;

    public UnityAction OnAppearDone;
    public UnityAction actionCloseBase;

    public bool isStack = true;
	private Canvas canvas;

	protected virtual void Awake()
	{
		canvas = this.GetComponent<Canvas>();
		if (canvas != null)
		{
			canvas.worldCamera = Camera.main;
			canvas.sortingLayerID = SortingLayer.NameToID(StringHelper.LAYER_GUI_EFFECT);
		}
	}

	protected virtual void Start()
	{
		
	}

	public virtual void OnClickCloseButton()
	{
		CloseCurrentBox();
	}

	protected virtual void OnStart()
	{
		if (backObj == null)
			backObj = this.GetComponent<BackObj>();
        if (backObj != null)
            backObj.actionDoOff = ActionDoOff;

		var boxs = FindObjectsOfType<BaseBox>();
		int layerMax = 0;
		for (int i = 0; i < boxs.Length; i++)
		{
			if (!boxs[i].gameObject.activeInHierarchy) continue;

			var boxCanvas = boxs[i].GetComponent<Canvas>();
			if (boxCanvas != null)
			{
				if (boxCanvas.sortingOrder >= layerMax)
				{
					layerMax = boxCanvas.sortingOrder;
				}
			}
		}
		canvas.sortingOrder = layerMax + 1;
	}

	protected virtual void ActionDoOff()
	{
	}

	protected virtual void DoAppear()
	{
		if (isAnim)
        {
            if (mainPanel != null)
            {
                mainPanel.localScale = Vector3.zero;
                mainPanel.DOScale(1, 0.5f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(()=> { if (OnAppearDone != null) OnAppearDone(); });
            }
        }
        else
        {
            if (OnAppearDone != null) OnAppearDone();
        }
	}

	protected virtual void OnEnable()
	{
		if (!isStack)
            return;
		if (currentBaseBox != null && currentBaseBox != this) {			
			currentBaseBox.Hide ();
		}
		StackBox.Push (this);
		//		FunctionHelper.ShowDebug ("StackBox.Push ("+ this.name +");");
		DoAppear ();
        OnStart();
        isClose = false;

	}

	protected virtual void OnDisable()
	{
        if (!isStack)
            return;
        if (actionCloseBase != null)
		{
			actionCloseBase();
			actionCloseBase = null;
		}
	}

	public virtual void Show()
	{
		gameObject.SetActive(true);       
	}		  

	protected void DestroyBox()
	{
		if (OnCloseBox != null)
			OnCloseBox ();
		if (currentBaseBox != null) {
//			FunctionHelper.ShowDebug ("StackBox.Pop ("+ currentBaseBox.name +");");
			StackBox.Pop ();
		}
		Destroy(gameObject);
	}

	public virtual void Hide()
	{
		gameObject.SetActive(false);
	}

	public virtual bool CloseCurrentBox()
	{
        if (backObj == null)
            return false;

		if (!backObj.isDoOffed)
		{
			backObj.DoOff();
			backObj.isDoOffed = true;
			return false;
		}
		return false;
	}

	public virtual void Close()
	{
        if (backObj == null)
            return;

        backObj.DoOff();
	}
}
