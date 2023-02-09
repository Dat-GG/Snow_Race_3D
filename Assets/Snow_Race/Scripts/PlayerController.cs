using System;
using System.Collections;
using DG.Tweening;
using FoodZombie;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Utilities.Common;

public class PlayerController : CharacterController
{
    public GameObject ground;
    public int doneStage;
    public GameObject camLookAt;
    public bool isSlide;
    public bool isStun;
    public TextMeshPro name;
    [SerializeField] private float ballValue;
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private ParabolaController _parabolaController;
    [SerializeField] private Transform liftPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject ballModel;
    [SerializeField] private GameObject[] ski;
    [SerializeField] private float finishDuration;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float changeColorSpeed;
    [SerializeField] private ParticleSystem iceStun;
    private FloatingJoystick _joystick;
    private SkinnedMeshRenderer _renderer; 
    private int _doneConstruction, _doneSlide, _doneCart;
    private float _minX, _maxX, _minZ, _maxZ;
    private bool _isLiftUp;
    private bool _isWater;
    private bool _isLava;
    private bool _isAscent;
    private bool _isFly;
    private bool _isJump;
    private bool _isBonusSpeed;
    private bool _isChangeAnim;
    private float _speed = 100;
    private GameObject _construction;
    private GameObject _checkPoint;
    private LevelManager _levelManager;
    private CameraFollow _camera;
    private Transform _myTransform;

    public enum PlayerStates
    {
        Idle,
        Walk,
        Stun,
        BuildRoad,
        BuildLadder,
        TurnBack,
        Rested,
        LiftUp,
        Slide,
        GoCart,
        FinishWalk,
        Kick,
        Ice
    }

    public PlayerStates state = PlayerStates.Idle;

    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _joystick = FindObjectOfType<FloatingJoystick>();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _camera = FindObjectOfType<CameraFollow>();
        _myTransform = transform;
    }

    private void Start()
    {
        LoadMapData(_doneConstruction, doneStage, _doneSlide);
        ball.isPlayer = true;
        ball.ballValue = ballValue;
        _joystick.isNotRun = true;
        name.text = GameSave.Cache_UserName;
        startColor = _renderer.material.color;
    }

    private void Update()
    {
        var rotation = _camera.transform.rotation;
        name.transform.LookAt(name.transform.position + rotation * -Vector3.back, rotation * Vector3.up);

        switch (state)
        {
            case PlayerStates.Idle:
                if (_joystick.isNotRun == false && GameManager.Instance._state == GameManager.GameStates.Play && _joystick.Direction != Vector3.zero)
                {
                    ChangeState(PlayerStates.Walk);
                }
                break;
            case PlayerStates.Walk:
                if (_joystick.isNotRun)
                {
                    ChangeState(PlayerStates.Idle);
                }
                Move();
                if (_isChangeAnim == false && ball.currentTimeScale > 5)
                {
                    _isChangeAnim = true;
                    playerCharacter.AnimateWalkPush();
                }
                break;
            case PlayerStates.Stun:
                _renderer.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * changeColorSpeed, 1));
                break;
            case PlayerStates.BuildRoad:
                break;
            case PlayerStates.TurnBack:
                if (IsMoveOnObject)
                {
                    return;
                }
                ChangeState(PlayerStates.Idle);
                break;
            case PlayerStates.Rested:
                break;
            case PlayerStates.LiftUp:
                break;
            case PlayerStates.Kick:
                break;
            case PlayerStates.FinishWalk:
                break;
            case PlayerStates.BuildLadder:
                break;
            case PlayerStates.Slide:
                var dist1 = Vector3.Distance(_myTransform.position,
                    _levelManager.playerSlide[_doneSlide].GetComponent<SlideController>().jumpPoint.position);
                
                if (_parabolaController.Animation == false)
                {
                    isSlide = false;
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(PlayerStates.Idle);
                }
                if (dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    playerCharacter.AnimateFly();
                }
                break;
            case PlayerStates.GoCart:
                var cart1 = Vector3.Distance(_levelManager.playerCart[_doneCart].transform.position,
                    _levelManager.playerSlide[_doneSlide].GetComponent<SlideController>().jumpPoint.position);
                var cart2 = Vector3.Distance(_levelManager.playerCart[_doneCart].transform.position,
                    _levelManager.playerSlide[_doneSlide].GetComponent<SlideController>().fallPoint.position);
                
                if (_levelManager.playerCart[_doneCart].GetComponent<CartController>().parabolaController.Animation == false)
                {
                    isSlide = false;
                    ChangeState(PlayerStates.Idle);
                    _myTransform.parent = null;
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                }
                if (cart1 <= 0.3f && _isFly == false)
                {
                    MusicManager.Instance.StopSound();
                    _isFly = true;
                    playerCharacter.AnimateFly();
                }

                if (cart2 <= 0.3f)
                {
                    _levelManager.playerCart[_doneCart].GetComponent<CartController>().FallingDown();
                }
                break;
            case PlayerStates.Ice:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region Data
    private void LoadMapData(int a, int b, int c)
    {
        ground = _levelManager.ground[b].gameObject;
        _construction = _levelManager.playerConstruction[a].gameObject;
        _levelManager.playerCheckPoint[a].gameObject.SetActive(true);
        _checkPoint = _levelManager.playerCheckPoint[a].gameObject;
        _parabolaController.ParabolaRoot = _levelManager.playerSlide[c].GetComponent<SlideController>().parabolaRoot;
        _parabolaController.parabolaFly =
            new ParabolaController.ParabolaFly(
                _levelManager.playerSlide[c].GetComponent<SlideController>().parabolaRoot.transform);
        GetGroundSize();
        ChangeState(PlayerStates.Idle);
    }
    
    private void GetGroundSize()
    {
        Renderer groundSize = ground.GetComponent<Renderer>();
        var bounds = groundSize.bounds;
        _minX = bounds.center.x - bounds.extents.x + 2f;
        _maxX = bounds.center.x + bounds.extents.x - 2f;
        _minZ = bounds.center.z - bounds.extents.z + 1.5f;
        _maxZ = bounds.center.z + bounds.extents.z - 1.5f;
    }

    public void BonusBall()
    {
        if (ball == null)
        {
            return;
        }
        ball.point = 10;
        ball.currentTimeScale = 10;
        ball.isIncrease = true;
        float scale = Mathf.Pow(ball.delta, ball.currentTimeScale);
        ball.transform.localScale = new Vector3(scale, scale, scale);
    }
    
    private void GroundCheck()
    {
        if (_myTransform.position.x < _minX - 2f || _myTransform.position.x > _maxX + 2f ||
            _myTransform.position.z < _minZ - 2.5f || _myTransform.position.z > _maxZ + 2.5f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }
    
    private IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("https://google.com");
        yield return request.SendWebRequest();
        if (request.error != null) {
            Debug.Log ("Error");
            action (false);
        } else{
            Debug.Log ("Success");
            action (true);
        }
    }
    #endregion

    #region Action
    private void Move()
    {
        if (isGrounded)
        {
            var position = _myTransform.position;
            var joystick = _joystick.Direction;
            position = Vector3.Lerp(position, position + joystick.normalized * _speed * Time.deltaTime, 0.1f);
            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            position.z = Mathf.Clamp(position.z, _minZ, _maxZ);
            _myTransform.position = position;
            if (_joystick.Direction != Vector3.zero)
            {
                _myTransform.rotation = Quaternion.LookRotation(joystick.normalized);
                ball.IncreaseSize();
            }
        }
    }

    private IEnumerator BuildConstruction()
    {
        _myTransform.rotation = Quaternion.identity;
        var road = _construction.GetComponent<Construction>();

        if (road.isWater)
        {
            _isWater = true;
        }
        if (road.isLava)
        {
            _isLava = true;
        }
        if (road.isAscent)
        {
            _isAscent = true;
        }
        
        while (IsMoveOnObject)
        {
            yield return null;
        }
        if (!BridgeDone)
        {
            IsMoveOnObject = true;
            ChangeState(PlayerStates.TurnBack);
        }
        else
        {
            MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Build_Finish);
            if (doneStage < _levelManager.ground.Count - 1)
            {
                doneStage++;
            }
            if (_doneConstruction < _levelManager.playerConstruction.Count - 1)
            {
                _doneConstruction++;
            }
            if (_isLiftUp == false)
            {
                ChangeState(PlayerStates.Rested);
            }
        }
    }

    private IEnumerator Finish()
    {
        float time = 0;
        var target = GameObject.FindWithTag("ScorePos");
        var pos = _myTransform.position;
        var targetDir = target.transform.position - _myTransform.position;
        var newDir = Vector3.RotateTowards(_myTransform.forward, targetDir, 0.3f, 0);
        _myTransform.rotation = Quaternion.LookRotation(newDir);
        while (time < finishDuration)
        {
            _myTransform.position = Vector3.Lerp(pos, target.transform.position, time / finishDuration);
            time += Time.deltaTime;
            ball.IncreaseSize();
            yield return null;
        }

        _myTransform.position = target.transform.position;
    }
    
    private IEnumerator TimeStun(float timeFall)
    {
        isStun = true;
        yield return new WaitForSeconds(timeFall);
        isStun = false;
        ChangeState(PlayerStates.Idle);
    }
    
    private IEnumerator TimeIce(float timeIce)
    {
        isStun = true;
        yield return new WaitForSeconds(timeIce);
        isStun = false;
        ChangeState(PlayerStates.Idle);
    }
    
    private IEnumerator BonusSpeed()
    {
        playerCharacter.AnimateSki();
        _speed *= 1.5f;
        _isBonusSpeed = true;
        ball.isBonusSpeed = true;
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < ski.Length; i++)
        {
            ski[i].SetActive(false);
        }
        _isBonusSpeed = false;
        ball.isBonusSpeed = false;
        _speed /= 1.5f;
    }

    private IEnumerator PlayerModelTranslate(float duration)
    {
        float time = 0;
        playerModel.transform.position = ball.topOfBall.transform.position;
        while (time < duration)
        {
            time += Time.deltaTime;
            playerModel.transform.position = Vector3.Lerp(playerModel.transform.position, ball.topOfBall.transform.position, 0.8f);
            yield return null;
        }
        playerModel.transform.localPosition = Vector3.zero;
    }
    
    private void LiftUp()
    {
        ball.transform.position = liftPoint.position;
        DOVirtual.DelayedCall(2f, delegate
        {
            ball.transform.position = startPoint.position;
            _isLiftUp = false;
            LoadMapData(_doneConstruction, doneStage, _doneSlide);
            ChangeState(PlayerStates.Idle);
        });
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !isStun)
        {
            var obj = other.GetComponentInParent<BallController>();
            if (ball.point < obj.point)
            {
                MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Stun);
                ChangeState(PlayerStates.Stun);
                ball.point = 0;
                ball.transform.localScale = Vector3.one;
                ballModel.gameObject.SetActive(false);
                ball.currentTimeScale = 0;
                ball.isIncrease = false;
                ball.explosive.SetActive(true);
            }
        }

        if (other.CompareTag("PlayerPoint"))
        {
            ChangeState(PlayerStates.BuildRoad);
            collider.enabled = false;
        }
        
        if (other.CompareTag("PlayerLadder"))
        {
            ChangeState(PlayerStates.BuildLadder);
            collider.enabled = false;
        }
        
        if (other.CompareTag("Elevator"))
        {
            _isLiftUp = true;
            ChangeState(PlayerStates.LiftUp);
        }
        
        if (other.CompareTag("Slide"))
        {
            ChangeState(PlayerStates.Slide);
        }
        
        if (other.CompareTag("Cart"))
        {
            var target = other.GetComponent<CartController>().startPoint;
            _myTransform.parent = other.transform;
            if (_isJump == false)
            {
                _isJump = true;
                other.tag = "Untagged";
                _myTransform.DOJump(target.position, 2f, 1, 0.5f)
                    .OnComplete(() => ChangeState(PlayerStates.GoCart));
            }
        }

        if (other.CompareTag("ScorePos"))
        {
            _myTransform.rotation = Quaternion.identity;
            ball.transform.parent = null;
            ball.isIncrease = false;
            ChangeState(PlayerStates.Kick);
        }

        if (other.CompareTag("FinishPos"))
        {
            ChangeState(PlayerStates.FinishWalk);
            GameManager.Instance.ChangeState(GameManager.GameStates.Win);
            MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Win1);
        }

        if (other.CompareTag("SkiAds"))
        {
            StartCoroutine(CheckInternetConnection(isConnected =>
            {
                if (isConnected)
                {
                    Time.timeScale = 0;
                    AdsHelper.__ShowVideoRewardedAd("GetBonusSpeed", OnViewRewardDone);
                }
                else
                {
                    other.tag = "Untagged";
                    other.GetComponent<SkiController>().MoveDown();
                }
            }));
        }

        if (other.CompareTag("Lava"))
        {
            MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Stun);
            ChangeState(PlayerStates.Stun);
            ball.point = 0;
            ball.transform.localScale = Vector3.one;
            ballModel.gameObject.SetActive(false);
            ball.highlightEffect.enabled = false;
            ball.currentTimeScale = 0;
            ball.isIncrease = false;
            ball.explosive.SetActive(true);
        }

        if (other.CompareTag("Ice"))
        {
            MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Stun);
            ChangeState(PlayerStates.Ice);
        }
    }
    
    private void OnViewRewardDone(bool obj)
    {
        Time.timeScale = 1;
        if (obj)
        {
            if (_levelManager.ski[doneStage] == null)
                return;
            _levelManager.ski[doneStage].tag = "Untagged";
            _levelManager.ski[doneStage].MoveDown();
            _levelManager.ski[doneStage].isBuff = true;
            if (doneStage + 1 < _levelManager.ground.Count - 1)
            {
                _levelManager.ski[doneStage + 1].SetActive(false);
            }
            for (int i = 0; i < ski.Length; i++)
            {
                ski[i].SetActive(true);
            }

            StartCoroutine(BonusSpeed());
        }
    }

    #region FSM
    public void ChangeState(PlayerStates newstate)
    {
        if (newstate == state) return;
        ExitCurrentState();
        state = newstate;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (state)
        {
            case PlayerStates.Idle:
                collider.enabled = true;
                playerCharacter.AnimateIdle();
                break;
            case PlayerStates.Walk:
                GroundCheck();
                ball.snow.Play();
                MusicManager.Instance.PlaySingle(MusicDB.Instance.SFX_Move);
                if (isGrounded)
                {
                    ball.wheelTrack.emitting = true;
                }
                if (_isBonusSpeed)
                {
                    playerCharacter.AnimateSki();
                }
                else
                {
                    if (ball.currentTimeScale > 5)
                    {
                        playerCharacter.AnimateWalkPush();
                    }
                    else
                    {
                        _isChangeAnim = false;
                        playerCharacter.AnimateWalkSmall();
                    }
                }
                break;
            case PlayerStates.Stun:
                playerCharacter.AnimateStun();
                StartCoroutine(TimeStun(2f));
                break;
            case PlayerStates.BuildRoad:
                ball.snow.Play();
                ball.wheelTrack.emitting = false;
                StartCoroutine(_construction.GetComponent<Construction>().
                    FillRoad(ball.currentTimeScale,this));
                StartCoroutine(BuildConstruction());
                if (_isLava)
                {
                    StartCoroutine(PlayerModelTranslate(_construction.GetComponent<Construction>().duration));
                    playerCharacter.AnimateWalkLava();
                }
                else if (_isWater)
                {
                    playerCharacter.AnimateWalkWater();
                }
                else
                {
                    playerCharacter.AnimateWalkPush();
                }
                break;
            case PlayerStates.TurnBack:
                playerModel.transform.localPosition = Vector3.zero;
                _myTransform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                StartCoroutine(_construction.GetComponent<Construction>().MoveBack(this));
                if (_isAscent)
                {
                    playerCharacter.AnimateWalkWater();
                }
                else
                {
                    playerCharacter.AnimateRunBack();
                }
                break;
            case PlayerStates.Rested:
                playerModel.transform.localPosition = Vector3.zero;
                playerCharacter.AnimateIdle();
                LoadMapData(_doneConstruction, doneStage, _doneSlide);
                break;
            case PlayerStates.LiftUp:
                MusicManager.Instance.PlaySingle(MusicDB.Instance.SFX_Elevator);
                playerCharacter.AnimateLiftUp();
                LiftUp();
                break;
            case PlayerStates.Kick:
                var pos = playerModel.transform.position;
                playerModel.transform.position = new Vector3(pos.x, pos.y, pos.z - 2f);
                playerCharacter.AnimateKick();
                DOVirtual.DelayedCall(1f, delegate
                {
                    StartCoroutine(ball.MoveToScore());
                });
                break;
            case PlayerStates.FinishWalk:
                ball.snow.Play();
                ball.wheelTrack.emitting = true;
                if (ball.currentTimeScale > 5)
                {
                    playerCharacter.AnimateWalkPush();
                }
                else
                {
                    playerCharacter.AnimateWalkSmall();
                }

                StartCoroutine(Finish());
                break;
            case PlayerStates.BuildLadder:
                ball.snow.Play();
                ball.wheelTrack.emitting = false;
                playerCharacter.AnimateWalkPush();
                StartCoroutine(_construction.GetComponent<Construction>().
                    FillRoad(ball.currentTimeScale, this));
                StartCoroutine(BuildConstruction());
                break;
            case PlayerStates.Slide:
                isSlide = true;
                playerCharacter.AnimateSlide();
                _parabolaController.FollowParabola();
                break;
            case PlayerStates.GoCart:
                MusicManager.Instance.PlaySingle(MusicDB.Instance.SFX_Elevator);
                isSlide = true;
                playerCharacter.AnimateSlide();
                _levelManager.playerCart[_doneCart].GetComponent<CartController>().parabolaController.FollowParabola();
                break;
            case PlayerStates.Ice:
                playerCharacter.animator.speed = 0;
                iceStun.gameObject.SetActive(true);
                iceStun.Play();
                StartCoroutine(TimeIce(3f));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ExitCurrentState()
    {
        switch (state)
        {
            case PlayerStates.Idle:
                break;
            case PlayerStates.Walk:
                MusicManager.Instance.StopSound();
                ball.snow.Stop();
                break;
            case PlayerStates.Stun:
                _renderer.material.color = startColor;
                ball.explosive.SetActive(false);
                break;
            case PlayerStates.BuildRoad:
                Ball.point = Ball.currentTimeScale;
                ball.snow.Stop();
                _isLava = false;
                _isWater = false;
                break;
            case PlayerStates.TurnBack:
                ball.gameObject.SetActive(true);
                break;
            case PlayerStates.Rested:
                break;
            case PlayerStates.LiftUp:
                MusicManager.Instance.StopSound();
                break;
            case PlayerStates.Kick:
                break;
            case PlayerStates.FinishWalk:
                break;
            case PlayerStates.BuildLadder:
                Ball.point = Ball.currentTimeScale;
                _isAscent = false;
                ball.snow.Stop();
                break;
            case PlayerStates.Slide:
                _isFly = false;
                if (_doneSlide < _levelManager.playerSlide.Count - 1)
                {
                    _doneSlide++;
                }
                break;
            case PlayerStates.GoCart:
                if (_doneSlide < _levelManager.playerSlide.Count - 1)
                {
                    _doneSlide++;
                }
                if (_doneCart < _levelManager.playerCart.Count - 1)
                {
                    _doneCart++;
                }
                _isJump = false;
                _isFly = false;
                break;
            case PlayerStates.Ice:
                playerCharacter.animator.speed = 1;
                iceStun.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion
}
