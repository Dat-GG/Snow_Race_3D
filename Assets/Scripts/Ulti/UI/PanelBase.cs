using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = PanelController.Instance.GetInstance<T>();
            }
            return instance;
        }
    }

    public void Show()
    {
        Instance.gameObject.SetActive(true);
    }
    public void Hide()
    {
        Instance.gameObject.SetActive(false);
    }
}
