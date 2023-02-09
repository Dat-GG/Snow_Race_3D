using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject ExplosionPrefab;
    public float DestroyExplosion;
    public float DestroyChildren;
    public Vector2 Velocity;

    Rigidbody rb;
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        var exp = Instantiate(ExplosionPrefab, transform.position, ExplosionPrefab.transform.rotation);
        DOVirtual.DelayedCall(DestroyExplosion, delegate
        {
            exp.SetActive(false);
        });
        //Destroy(exp, DestroyExplosion);
        // Transform child;
        // child = transform.GetChild(0);
        // transform.DetachChildren();
        // DOVirtual.DelayedCall(DestroyChildren, delegate
        // {
        //     child.gameObject.SetActive(false);
        // });
        //Destroy(child.gameObject, DestroyChildren);
        //Destroy(gameObject);
    }
}
