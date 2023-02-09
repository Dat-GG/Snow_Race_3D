using DG.Tweening;
using UnityEngine;
public class SkiController : MonoBehaviour
{
    public bool isBuff;
    [SerializeField] private GameObject ski;
    [SerializeField] private float duration;
    [SerializeField] private Transform endPos;
    [SerializeField] private Transform startPos;

    private void Start()
    {
        ski.transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void MoveDown()
    {
        ski.transform.DOMove(endPos.position, 3);
        DOVirtual.DelayedCall(5f, delegate
        {
            if (isBuff == false)
            {
                ski.transform.DOMove(startPos.position, 3).OnComplete(() => gameObject.tag = "SkiAds");
            }
        });
    }
}
