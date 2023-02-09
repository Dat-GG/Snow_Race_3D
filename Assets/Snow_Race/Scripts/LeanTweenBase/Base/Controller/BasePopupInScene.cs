using UnityBase.Base.Controller;
using UnityEngine;

public class BasePopupInScene : BasePopUp
{
    protected System.Action _CloseEvent;
    protected System.Action _OpenEvent;

    protected override void OnActive()
    {
        //Time.timeScale = qq;
        transform.localScale = Vector3.zero;
        ScaleTo(Vector3.one, () =>
        {
            //Time.timeScale = 1f;
            if (_OpenEvent != null) _OpenEvent();
            currentTypeClosePopUp = typeClosePopUp;

        });
    }
    public void OpenPopUp()
    {
        gameObject.SetActive(true);
        timePerforme = 0.3f;
        //_LeanTweenType = LeanTweenType.easeSpring;
        OnActive();
    }
    public override void ClosePopUp()
    {
        //Time.timeScale = 1;
        ScaleTo(Vector3.zero, () => {
            //Time.timeScale = 1f;
            if (_callbackClosePopUp != null)
                _callbackClosePopUp();

            if (_CloseEvent != null) _CloseEvent();
            gameObject.SetActive(false);
        });
    }
    public virtual void ClosePopUpEvent(System.Action closeEven)
    {
        _CloseEvent += closeEven;
    }
    public virtual void OpenPopUpEvent(System.Action openEven)
    {
        _OpenEvent += openEven;
    }
}
