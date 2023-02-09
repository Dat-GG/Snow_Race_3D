using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LadderController : Construction
{
    public List<GameObject> ladder;
    public bool isDescentBridge;
    public bool isAscentBridge;
    public override void SetScale(float scale, CharacterController character)
    { // scale 0 - 0.5 - 1 - 1.5....
        for (int i = 0; i < ladder.Count; i++)
        {
            if (i <= scale - 1)
            {
                ladder[i].gameObject.SetActive(true); 
                endPoint = ladder[i].transform;
                if (i < ladder.Count - 1)
                {
                    character.transform.position = Vector3.Lerp
                        (ladder[i].transform.position, ladder[i + 1].transform.position, scale % 1);
                }
                else
                {
                    character.transform.position = ladder[i].transform.position;
                }
            }
        }
    }
    
    public override IEnumerator MoveBack(CharacterController character)
    {
        float timeScale = roadFill * 0.1f;
        float timer = 0;
        Vector3 startPoint;
        if (isDescentBridge)
        {
            startPoint = new Vector3(ladder[0].transform.position.x, ladder[0].transform.position.y + 0.15f,
                ladder[0].transform.position.z - 3f);
        }
        else if (isAscentBridge)
        {
            startPoint = new Vector3(ladder[0].transform.position.x, ladder[0].transform.position.y - 0.295f,
                ladder[0].transform.position.z - 3f);
        }
        else
        {
            startPoint = new Vector3(ladder[0].transform.position.x, ladder[0].transform.position.y - 0.25f,
                ladder[0].transform.position.z - 3f);
        }

        while (timer < timeScale)
        {
            timer += Time.deltaTime;
            character.transform.position = Vector3.Lerp(endPoint.position, startPoint, timer / timeScale);
            yield return null;
        }
        character.IsMoveOnObject = false;
    }
    
    private void OnEnable()
    {
        roadSize = ladder.Count;
        for (int i = 0; i < ladder.Count; i++)
        {
            ladder[i].gameObject.SetActive(false);
        }
    }
}
