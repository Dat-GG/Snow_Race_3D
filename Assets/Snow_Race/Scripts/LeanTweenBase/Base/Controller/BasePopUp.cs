using UnityBase.Base.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityBase.Base.Controller
{
    public abstract class BasePopUp : BehaviourController,IPointerClickHandler {
		#region IPointerClickHandler implementation

    [SerializeField]
	protected ButtonController BtnExit;
     protected virtual void Start()
	{
		if (BtnExit != null)
        {
            typeClosePopUp = TypeClosePopUp.ClickButtonToClose;
			BtnExit.OnClick((ButtonController Btn)=>
			{
				ClosePopUp();
			});
        }
        else
        {
			typeClosePopUp = TypeClosePopUp.ClickUpToClose;
        }

        OnActive();
	}

    protected virtual void OnActive()
    {
        transform.localScale = Vector3.zero;
        ScaleTo(Vector3.one, () => {

            currentTypeClosePopUp = typeClosePopUp;
        });
    }

	public void OnPointerClick (PointerEventData eventData)
	{
	 //   Debug.Log(currentTypeClosePopUp);
		//if (currentTypeClosePopUp == TypeClosePopUp.ClickButtonToClose) {
		//	ClosePopUp ();
		//}
	}

	#endregion

	public enum TypeClosePopUp
	{
		ClickUpToClose,
		ClickButtonToClose,
		None

	}

	public Action _callbackClosePopUp;


	[SerializeField]
	protected TypeClosePopUp typeClosePopUp = TypeClosePopUp.None;
	protected TypeClosePopUp currentTypeClosePopUp = TypeClosePopUp.None;
	public virtual void ClosePopUp()
	{
        ScaleToClose(Vector3.zero,()=>{
			if (_callbackClosePopUp != null)
			    _callbackClosePopUp ();
			//PopUp.Instance.ClosePopUp (this);
		});
	}
}
}

