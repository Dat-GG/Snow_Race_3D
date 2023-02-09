using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BaseScenes : MonoBehaviour
{
    public static Stack<BackObj> _backObjs = new Stack<BackObj>();
    private const string keyScene = "PrevOf";

    public bool isLockEscape;
    public static UnityAction actionStackEmty;

    public const int BASE_INDEX_LAYER = 20;

    /// <summary>
    /// Stack lưu trữ UI sẽ được mở khi UI cuối cùng  trong stack _backObjs đóng lại
    /// </summary>
    public static Stack<BackObj> _openStack = new Stack<BackObj>();

    public static void AddNewBackObj(BackObj obj)
    {
        _backObjs.Push(obj);
        SettingOderLayerPopup();
    }

    private static void SettingOderLayerPopup()
    {
        if (_backObjs == null && _backObjs.Count <= 0)
            return;
        //BackObj[] lst_backObjs = _backObjs.ToArray();
        //int lenght = lst_backObjs.Length;
        //int index = 0;
        //for (int i = lenght - 1; i >= 0; i--)
        //{
        //    if (lst_backObjs[i].isPopup)
        //    {
        //        if (lst_backObjs[i].popupCanvas != null)
        //        {
        //            lst_backObjs[i].popupCanvas.sortingOrder = BASE_INDEX_LAYER + index;
        //            lst_backObjs[i].popupCanvas.planeDistance = 5;
        //            lst_backObjs[i].indexSortingLayerPopup = BASE_INDEX_LAYER + index;
        //            lst_backObjs[i].IDLayerIndexPopup = lst_backObjs[i].popupCanvas.sortingLayerID;
        //            if (lst_backObjs[i].actionChangeLayer != null)
        //                lst_backObjs[i].actionChangeLayer();
        //            index++;
        //        }
        //    }
        //}
    }

    string PrevScene()
    {
        return PlayerPrefs.GetString(keyScene + SceneManager.GetActiveScene().name, String.Empty);
    }
    protected virtual void Update()
    {
        if (isLockEscape)
            return;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ProcessBackBtn();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < _backObjs.Count; i++)
                Debug.Log("_backObjs " + _backObjs.ToArray()[i].gameObject.name);
        }
    }


    public static void Remove()
    {
        if (_backObjs.Count == 0)
            return;
        BackObj obj = _backObjs.Pop();
        // Debug.Log("_backObjs_ " + BaseScenes._backObjs.Peek().gameObject.name);
        if(_backObjs.Count == 0)
        OnStackEmpty();

        SettingOderLayerPopup();
    }

    private static void OnStackEmpty()
    {
        if(actionStackEmty != null)
        actionStackEmty();
    }

    public virtual void ProcessBackBtn()
    {
        if (_backObjs.Count != 0)
        {
            if (_backObjs.Peek().allowBack)
            {
                _backObjs.Peek().DoOff();
            }
            else
            {
            }
        }
        else
        {
            OnBaseBack();
        }
    }
    protected virtual void OnBaseBack()
    {

    }
    protected void LoadPrevScene()
    {
        if (String.Equals(PrevScene(), String.Empty, StringComparison.Ordinal))
        {

            Application.Quit();
        }
        else
        {

        }
        //GameMannager.LoadSceneWithLoading(PrevScene());
        //SceneManager.LoadScene(PrevScene());
    }
    public void LoadSceneSavePrev(string sceneName)
    {
        PlayerPrefs.SetString(keyScene + sceneName, SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(sceneName);
        // GameMannager.LoadSceneWithLoading(sceneName);
    }
    public void LoadScene(string sceneName)
    {
        //GameMannager.LoadSceneWithLoading(sceneName);
        //SceneManager.LoadScene(sceneName);
    }
    protected virtual void OnDisable()
    {
        _backObjs.Clear();
    }

    protected virtual void OnDestroy()
    {
        _backObjs.Clear();
        actionStackEmty = null;
    }

    protected virtual void Start()
    {
        Debug.Log($"Start Scene : <color=yellow>{gameObject.name}</color>");
    }

    protected virtual void OnEnable()
    {
        Debug.Log($"Start Scene : <color=yellow>{gameObject.name}</color>");
    }
}
