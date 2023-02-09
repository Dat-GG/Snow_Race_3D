using System.Collections;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;
using Utilities.Common;

public class BallController : MonoBehaviour
{
    public float ballValue;
    public float point;
    public float delta;
    public int maxTimeScale;
    public int currentTimeScale;
    public int multi;
    public bool isPlayer;
    public bool isIncrease;
    public bool isDone;
    public bool isBonusSpeed;
    public TrailRenderer wheelTrack;
    public Transform topOfBall;
    public ParticleSystem snow;
    public ParticleSystem explosive;
    public PlayerController player;
    public HighlightEffect highlightEffect;
    [SerializeField] private float timeDuration1;
    [SerializeField] private float timeDuration2;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject ballModel;
    [SerializeField] private ParticleSystem[] confetti;
    private float _increaseTime;
    private Transform _myTransform;
    private ScoringController _score;

    private void Awake()
    {
        _score = FindObjectOfType<ScoringController>();
        _myTransform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava") && !player.isStun && isPlayer)
        {
            player.ChangeState(PlayerController.PlayerStates.Stun);
            MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Stun);
            point = 0;
            transform.localScale = Vector3.one;
            ballModel.gameObject.SetActive(false);
            highlightEffect.enabled = false;
            currentTimeScale = 0;
            isIncrease = false;
            explosive.SetActive(true);
        }
        
        if (other.CompareTag("Ice") && !player.isStun && isPlayer)
        {
            player.ChangeState(PlayerController.PlayerStates.Ice);
            MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Stun);
        }
    }

    public void IncreaseSize()
    {
        // if (isPlayer)
        // {
        //     if (isBonusSpeed)
        //     {
        //         point += Time.deltaTime * 1.05f * 1.5f;
        //     }
        //     else
        //     {
        //         point += Time.deltaTime * 1.05f;
        //     }
        // }
        // else
        // {
        //     point += Time.deltaTime;
        // }

        if (currentTimeScale == maxTimeScale)
        {
            highlightEffect.enabled = true;
        }
        
        ballModel.transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
        _increaseTime += Time.deltaTime;
        if (isBonusSpeed)
        {
            if (_increaseTime >= ballValue / 1.5f)
            {
                if (currentTimeScale <= 0 && isIncrease == false)
                {
                    ballModel.gameObject.SetActive(true);
                    isIncrease = true;
                    //currentTimeScale++;
                    _increaseTime = 0;
                }
                else if(isIncrease && currentTimeScale < maxTimeScale)
                {
                    //_myTransform.localScale *= delta;
                    var localScale = _myTransform.localScale;
                    _myTransform.DOScale(new Vector3(localScale.x * delta, localScale.y * delta, localScale.z * delta), 0.5f);
                    currentTimeScale++;
                    point++;
                    _increaseTime = 0;
                }
            }
        }
        else
        {
            if (_increaseTime >= ballValue)
            {
                if (currentTimeScale <= 0 && isIncrease == false)
                {
                    ballModel.gameObject.SetActive(true);
                    isIncrease = true;
                    _increaseTime = 0;
                }
                else if(isIncrease && currentTimeScale < maxTimeScale)
                {
                    //_myTransform.localScale *= delta;
                    var localScale = _myTransform.localScale;
                    _myTransform.DOScale(new Vector3(localScale.x * delta, localScale.y * delta, localScale.z * delta), 0.5f);
                    currentTimeScale++;
                    point++;
                    _increaseTime = 0;
                }
            }
        }
    }

    public void ScaleBall()
    {
        ballModel.transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
        highlightEffect.enabled = false;
        if (currentTimeScale == 0)
        {
            ballModel.gameObject.SetActive(false);
            isIncrease = false;
            point = 0;
        }
        else
        {
            float scale = Mathf.Pow(delta, currentTimeScale);
            _myTransform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public IEnumerator MoveToScore()
    {
        wheelTrack.emitting = false;
        float time = 0;
        var startPos = _myTransform.position;
        while (time < timeDuration1)
        {
            _myTransform.position = Vector3.Lerp(startPos, _score.startScorePos.position, time / timeDuration1);
            time += Time.deltaTime;
            yield return null;
        }
        Vector3 realEndPoint = default;
        if (currentTimeScale <= 2)
        {
            realEndPoint = _score.scorePos[0].position;
            multi += 1;
        }
        else if (currentTimeScale > 2 && currentTimeScale <= 4)
        {
            realEndPoint = _score.scorePos[1].position;
            multi += 2;
        }
        else if (currentTimeScale > 4 && currentTimeScale <= 6)
        {
            realEndPoint = _score.scorePos[2].position;
            multi += 3;
        }
        else if (currentTimeScale > 6 && currentTimeScale <= 8)
        {
            realEndPoint = _score.scorePos[3].position;
            multi += 4;
        }
        else if (currentTimeScale > 8 && currentTimeScale <= 10)
        {
            realEndPoint = _score.scorePos[4].position;
            multi += 5;
        }
        else if (currentTimeScale > 10 && currentTimeScale <= 12)
        {
            realEndPoint = _score.scorePos[5].position;
            multi += 6;
        }
        else if (currentTimeScale > 12 && currentTimeScale <= 14)
        {
            realEndPoint = _score.scorePos[6].position;
            multi += 7;
        }
        else if (currentTimeScale > 14 && currentTimeScale <= 16)
        {
            realEndPoint = _score.scorePos[7].position;
            multi += 8;
        }
        else if (currentTimeScale > 16 && currentTimeScale <= 18)
        {
            realEndPoint = _score.scorePos[8].position;
            multi += 9; 
        }
        else if (currentTimeScale > 18 && currentTimeScale <= 20)
        {
            realEndPoint = _score.scorePos[9].position;
            multi += 100;
        }
        
        GameSave.PlayerCoin += EnemyData.Instance.RewardByLevel(GameSave.PlayerLevel - 1) * multi;
        time = 0;
        var index = currentTimeScale;
        while (time < timeDuration2)
        {
            time += Time.deltaTime;
            _myTransform.position = Vector3.Lerp(_score.startScorePos.position, realEndPoint, time / timeDuration2);
            currentTimeScale = Mathf.FloorToInt(Mathf.Lerp(index, 1, time / timeDuration2));
            ScaleBall();
            yield return null;
        }
        _myTransform.position = realEndPoint;
        MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Win2);
        for (int i = 0; i < confetti.Length; i++)
        {
            confetti[i].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        isDone = true;
    }
}
