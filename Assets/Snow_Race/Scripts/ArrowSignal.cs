using DG.Tweening;
using UnityEngine;

public class ArrowSignal : MonoBehaviour
{
    [SerializeField] private float duration;

    private void Start()
    {
        transform.DOMoveZ(transform.position.z + 1.5f, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
