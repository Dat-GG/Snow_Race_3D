using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    static PanelController instance;
    public static PanelController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PanelController>();
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

   
    public T GetInstance<T>() where T : MonoBehaviour
    {
        T _t = null;

        int _count = PanelAssets.Instance.Datas.Count;
        for (int i = 0; i < _count; i++)
        {
            _t = PanelAssets.Instance.Datas[i].GetComponent<T>();
            if (_t != null)
            {
                break;
            }
        }

        if (_t == null)
        {
            Debug.LogError("Not found ");
            throw new System.Exception("Not found panel");
        }

        var instance = _t;
        var _object = Instantiate(instance, this.transform);
        // instance.transform.SetParent(PanelController.Instance.transform);
        return _object;
    }


}
