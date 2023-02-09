using DG.Tweening;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public Transform startPoint;
    public ParabolaController parabolaController;
    public GameObject mineCart;

    public void FallingDown()
    {
        mineCart.transform.parent = null;
        var target = new Vector3(mineCart.transform.position.x, mineCart.transform.position.y - 15, mineCart.transform.position.z);
        mineCart.transform.DOMove(target, 2f).OnComplete(() => Destroy(mineCart));
    }
}
