using System;
using UnityEngine;
using UnityEngine.UI;
public class UIConfirmController : MonoBehaviour
{
    [SerializeField] private Text txtTitle;
    [SerializeField] private Text txtContent;
    [SerializeField] private Text txtYes;
    [SerializeField] private Text txtNo;
    [SerializeField] private Button btnYes;
    [SerializeField] private Button btnNo;

    Action _onYesEvent, _onNoEvent;
    private void Awake()
    {
        btnYes.onClick.AddListener(OnClickYesEvent);
        btnNo.onClick.AddListener(OnClickNoEvent);
    }

    private void Init(string title, string content,
        string yesContent = "yes", string noContent = "no",
        Action onYes = null, Action onNo = null)
    {
        txtTitle.text = title;
        txtContent.text = content;
        txtYes.text = yesContent;
        txtNo.text = noContent;
        _onNoEvent = onNo;
        _onYesEvent = onYes;
        //Show();
    }

    public void InitConfirm(string title, string content, string okContent = "OK", Action onOk = null)
    {
        title = title.ToLower();
        content = content.ToLower();
        btnYes.gameObject.SetActive(true);
        btnNo.gameObject.SetActive(false);
        Init(title, content, okContent, "", onOk);
    }

    void OnClickYesEvent()
    {
        _onYesEvent?.Invoke();
        gameObject.SetActive(false);
    }
    void OnClickNoEvent()
    {
        _onNoEvent?.Invoke();
        gameObject.SetActive(false);
    }
}
