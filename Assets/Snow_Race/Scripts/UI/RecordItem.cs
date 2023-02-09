using UnityEngine;
using UnityEngine.UI;

public class RecordItem : MonoBehaviour
{
    [SerializeField] private Text lvTxt;
    [SerializeField] private Text resultTxt;
    public void InitData(int a, int b)
    {
        lvTxt.text = "Level " + a;
        resultTxt.text = b + " Star";
    }
}
