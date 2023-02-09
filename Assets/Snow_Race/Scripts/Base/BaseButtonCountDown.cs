using System;
using SimpleTimer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseButtonCountDown : MonoBehaviour
{
    [SerializeField] private Text txtTime;
    private DateTime _dateTimeEnd;
    protected UnityAction actionComplete;
    private TimerCountDown _timerCountDown;

    protected void CountDown(TimeSpan timeGap, string strDefault = "")
    {
        _timerCountDown?.Stop();
        txtTime.text = strDefault;
        _timerCountDown = new TimerCountDown(() => DateTime.Now, timeGap,
            (step) => { txtTime.text = $"{step.Minutes:D2}:{step.Seconds:D2}"; },
            () =>
            {
                txtTime.text = strDefault;
                actionComplete?.Invoke();
            });
        _timerCountDown.Start();
    }
    
    private void OnDestroy()
    {
        _timerCountDown?.Stop();
    }

    protected virtual void OnClickButton()
    {
        Debug.Log(StringHelper.StringColor(Enums.ColorString.green, $"BaseButtonCountDown OnClickButton"));
    }
}
