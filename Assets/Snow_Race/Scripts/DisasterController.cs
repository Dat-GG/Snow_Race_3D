using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DisasterController : MonoBehaviour
{
    [SerializeField] private GameObject lavaPrefab;
    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private GameObject warning;

    private void OnEnable()
    {
        var rd = Random.Range(warning.transform.position.x + 10f, warning.transform.position.x - 3f);
        var pos = new Vector3(rd, warning.transform.position.y + 20f, warning.transform.position.z);
        lavaPrefab.transform.position = pos;
        lavaPrefab.SetActive(true);

        DOVirtual.DelayedCall(2.75f, delegate
        {
            lavaPrefab.transform.DOMove(warning.transform.position, 1.75f).SetEase(Ease.InSine).OnComplete(() =>
            {
                explosivePrefab.SetActive(true);
                lavaPrefab.SetActive(false);
                DOVirtual.DelayedCall(1f, delegate
                {
                    gameObject.SetActive(false);
                });
            });
        });
    }
}
