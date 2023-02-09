using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelLoading : MonoBehaviour
{
    [SerializeField]
    Image img_process;

    public static string nextSceneName;

    AsyncOperation _operation;
    public static void LoadScene(string name)
    {
        nextSceneName = name;
        SceneManager.LoadScene(GameConstant.GameScene.LOADING);
    }
    // Start is called before the first frame update
    void Start()
    {
        _operation = SceneManager.LoadSceneAsync(nextSceneName);
        _operation.allowSceneActivation = false;
        StartCoroutine(IELoading());
    }


    IEnumerator IELoading()
    {
        float startTime = Time.time;
        while (true)
        {
            img_process.rectTransform.localScale = new Vector3(_operation.progress / 0.9f, 1, 1);
            //Debug.LogError(_operation.progress);
            yield return null;

            if ((_operation.isDone || _operation.progress >= .9f) && Time.time - startTime > 1) break;
        }
        _operation.allowSceneActivation = true;

    }

}
