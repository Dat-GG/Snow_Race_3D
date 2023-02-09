using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float speed;
   
    protected virtual void Move(Vector3 direction)
    {
        var position = transform.position;
        position = Vector3.Lerp(position, position + direction * speed * Time.deltaTime * 25, 0.1f);
        transform.position = position;
    }
}
