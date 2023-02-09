using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Animation))]
public class BackObj : MonoBehaviour
{
    [Header("Add Close Button Here")]
    public Button closeBtn;
    private Animation animPopUp;
    [Header("Animations")]
    public AnimationClip popUpShow;
    public AnimationClip popUpHide;
    const string SHOW_STRING = "Show";
    const string HIDE_STRING = "Hide";
    public delegate void ActionDelegate();
    public ActionDelegate delegateOnThisPopUpShow;
    public ActionDelegate delegateOnThisPopUpHide;
    private List<string> nameClips = new List<string>();
    public bool allowBack = true;

    [HideInInspector] public float timeAnimClose;
    public UnityAction actionDoOff;
    [HideInInspector] public bool isDoOffed;

    public bool isPopup;
    [HideInInspector] public Canvas popupCanvas;
    [HideInInspector] public int IDLayerIndexPopup;
    [HideInInspector] public int indexSortingLayerPopup;
    public UnityAction actionChangeLayer;
    private void Awake()
    {
        animPopUp = GetComponent<Animation>();
        popupCanvas = GetComponent<Canvas>();
        if (popUpShow)
        {
            animPopUp.AddClip(popUpShow, SHOW_STRING);
            nameClips.Add(SHOW_STRING);
        }
        if (popUpHide)
        {
            animPopUp.AddClip(popUpHide, HIDE_STRING);
            nameClips.Add(HIDE_STRING);
        }
    }

    public void ShowStack()
    {
        this.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        isDoOffed = false;
        PlayAnim(SHOW_STRING);
        BaseScenes.AddNewBackObj(this);
        if (closeBtn != null)
        {
            closeBtn.enabled = true;
            closeBtn.onClick.AddListener(DoOff);
        }
        if (delegateOnThisPopUpShow != null)
        {
            delegateOnThisPopUpShow();
            delegateOnThisPopUpShow = null;
        }
    }
    public void DoOff()
    {
        if (isDoOffed)
            return;
        //AudioAssistant.Shot(StringHelper.SOUND_UI_OK_BACK);
        if(gameObject.activeInHierarchy) StartCoroutine(IDoOff());
    }
    private IEnumerator IDoOff()
    {
        if (closeBtn != null)
            closeBtn.enabled = false;

        BaseScenes.Remove();
        PlayAnim(HIDE_STRING);
        if (actionDoOff != null)
            actionDoOff();
        yield return new WaitForSecondsRealtime(timeAnimClose);
        gameObject.SetActive(false);
    }

    public void DoOffNotDelay()
    {
        if (isDoOffed)
            return;
        BaseScenes.Remove();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (closeBtn != null)
            closeBtn.onClick.RemoveListener(DoOff);
        if (delegateOnThisPopUpHide != null)
        {
            delegateOnThisPopUpHide();
            delegateOnThisPopUpHide = null;
        }

    }
    private void PlayAnim(string anim)
    {
        if (!nameClips.Contains(anim))
            return;
        animPopUp.Play(anim);
    }
    private float getHideLenght
    {
        get
        {
            if (popUpHide)
                return popUpHide.length;
            return 0;
        }
    }

}
