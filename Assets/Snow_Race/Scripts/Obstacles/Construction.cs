using System.Collections;
using UnityEngine;

public abstract class Construction : MonoBehaviour
{
    public bool isWater;
    public bool isLava;
    public bool isAscent;
    public int roadSize;
    public int roadFill;
    public float duration;
    public Transform endPoint;
    public GameObject road;
    public GameObject lava;
    public GameObject water;

    public virtual void Init()
    {
        
    }
    
    public IEnumerator FillRoad(float b, CharacterController character)
    {
        character.IsMoveOnObject = true;
        character.BridgeDone = false;
        int currentScale = roadFill;

        int need = roadSize - roadFill;
        int ballAfter = 0;
        if (b < need)
        {
            // cho qua ball ve 0 sau do.
            roadFill = roadFill + Mathf.FloorToInt(b);
            duration = roadFill * 0.1f;
            character.BridgeDone = false;
        }
        else // a > need
        {
            ballAfter =  Mathf.FloorToInt(b)- need;
            roadFill = roadSize;
            duration = roadFill * 0.1f;
            character.BridgeDone = true;
        }
        // di chuyen character den diem hien tai
        float timeScale = currentScale * 0.1f;
        float timer = 0;
        while (timer < timeScale)
        {
            timer += Time.deltaTime;
            character.transform.position = Vector3.Lerp(transform.position,endPoint.position,timer/timeScale);
            yield return null;
        }

        timeScale = (roadFill - currentScale) * 0.1f;
        // duration = timeScale;
        timer = 0;
        
        while (timer < timeScale)
        {
            timer += Time.deltaTime;
            var scale = Mathf.Lerp(currentScale,roadFill,timer/timeScale);
            SetScale(scale, character);
            character.Ball.currentTimeScale = Mathf.FloorToInt(Mathf.Lerp(b, ballAfter, timer / timeScale));
            character.Ball.ScaleBall();
            yield return null;
        }
        
        SetScale(roadFill, character);
        character.Ball.currentTimeScale = ballAfter;
        character.Ball.ScaleBall();
        character.IsMoveOnObject = false;
        if (!character.BridgeDone)
        {
            timeScale = roadFill * 0.1f;
            timer = 0;
            while (timer < timeScale)
            {
                timer += Time.deltaTime;
                character.transform.position = Vector3.Lerp(endPoint.position,transform.position,timer/timeScale);
                character.Ball.currentTimeScale = Mathf.FloorToInt(Mathf.Lerp(b, ballAfter, timer / timeScale));
                character.Ball.ScaleBall();
                yield return null;
            }
        }
    }
    
    public virtual void SetScale(float scale,CharacterController character)
    {
        road.transform.localScale = new Vector3(road.transform.localScale.x, road.transform.localScale.y, scale);
        character.transform.position = endPoint.position;
    }

    public virtual IEnumerator MoveBack(CharacterController character)
    {
        yield return null;
    }
}
