using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicUI : MonoBehaviour
{
    public enum PlayType
    {
        OnEnable,
        OnClick
    }

    public enum FX
    {
        Button, Popup
    }

    public PlayType playType;
    public FX fx;

    private void Awake()
    {
        if (playType == PlayType.OnClick)
        {
            var _button = this.GetComponent<Button>();
            if (_button != null)
            {
                _button.onClick.AddListener(PlayFX);
            }
        }
    }

    private void OnEnable()
    {

        if (playType == PlayType.OnEnable)
            PlayFX();
    }

    void PlayFX()
    {
        switch (fx)
        {
            case FX.Button:
                MusicManager.Instance.PlayOneShot(MusicManager.Instance.MusicDB.SFX_button);
                break;
            case FX.Popup:
                MusicManager.Instance.PlayOneShot(MusicManager.Instance.MusicDB.SFX_popup);
                break;
            default:
                break;
        }
    }
}
