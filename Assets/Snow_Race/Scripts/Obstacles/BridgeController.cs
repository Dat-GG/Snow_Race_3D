using System.Collections;
using UnityEngine;

public class BridgeController : Construction
{
    public GameObject wall;
    private Transform _myTransform;

    private void Awake()
    {
        _myTransform = transform;
    }

    public override void Init()
    {
        if (isLava)
        {
            lava.SetActive(true);
        }
        else if (isWater)
        {
            water.SetActive(true);
        }
        roadFill = 0;
        var localScale = wall.transform.localScale;
        localScale = new Vector3(localScale.x, localScale.y, roadSize);
        wall.transform.localScale = localScale;
    }

    public override IEnumerator MoveBack(CharacterController character)
    {
        float timeScale = roadFill * 0.1f;
        float timer = 0;
        while (timer < timeScale)
        {
            timer += Time.deltaTime;
            character.transform.position = Vector3.Lerp(endPoint.position,
                new Vector3(_myTransform.position.x, _myTransform.position.y + 0.5f, _myTransform .position.z - 3f), timer / timeScale);
            yield return null;
        }

        character.IsMoveOnObject = false;
    }
    
    private void OnEnable()
    {
        Init();
    }
}
