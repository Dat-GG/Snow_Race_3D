using System.Collections;
using UnityEngine;
public class ElevatorController : MonoBehaviour
{
    [SerializeField] private GameObject frontDoor;
    [SerializeField] private GameObject backDoor;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform standingPoint;
    [SerializeField] private float duration;
    [SerializeField] private BoxCollider boxCollider;
    private Transform _myTransform;
    private void Awake()
    {
        _myTransform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            frontDoor.SetActive(false);
            other.transform.position = standingPoint.position;
            StartCoroutine(MoveUp(other.gameObject));
            boxCollider.size = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    private IEnumerator MoveUp(GameObject obj)
    {
        float time = 0;
        var objEndPoint = endPoint.position - new Vector3(0, 0.75f, 0);
        var elevatorStartPoint = _myTransform.position;
        var objStartPoint = obj.transform.position;
        while (time < duration)
        {
            time += Time.deltaTime;
            _myTransform.position = Vector3.Lerp(elevatorStartPoint, endPoint.position, time / duration);
            obj.transform.position = Vector3.Lerp(objStartPoint, objEndPoint, time / duration);
            yield return null;
        }
        _myTransform.position = endPoint.position;
        obj.transform.position = objEndPoint;
        obj.transform.position = objEndPoint + new Vector3(0, 0, 1.5f);
        frontDoor.SetActive(true);
        backDoor.SetActive(false);
    }
}
