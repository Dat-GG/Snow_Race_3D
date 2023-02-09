using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehavier<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if(instance == null)
                {
                    Debug.LogError(instance.GetType() + " == null");
                }
            }
            return instance;
        }
    }
}
