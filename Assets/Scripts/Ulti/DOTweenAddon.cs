using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenAddon : MonoBehaviour
{
    public DOTweenAnimation DOTAnim;
    private void OnEnable()
    {
        DOTAnim.DORestart();
    }
}
