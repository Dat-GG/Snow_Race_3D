using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Common;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
public class EnemyController : CharacterController
{
    public bool isDance;
    public enum EnemyStates
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
        Dance,
        Attack,
        Afk,
        GoBuild,
        Ice
    }
    public EnemyStates state = EnemyStates.Idle;
    public MoveAction moveAction;
    public AttackAction attackAction;
    public AfkAction afkAction;
    public BuildAction buildAction;
    public GameObject ground;

    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private float ballValueBuff;
    [SerializeField] private EnemyCharacter enemyCharacter;
    [SerializeField] private ParabolaController parabolaController;
    [SerializeField] private GameObject enemyModel;
    [SerializeField] private GameObject ballModel;
    [SerializeField] private float timeMoveToDance;
    [SerializeField] private Transform liftPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private TextMeshPro name;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float changeColorSpeed;
    [SerializeField] private ParticleSystem iceStun;
    private float _currentWaitTime;
    private float _countTime;
    private float _random;
    private float _minX, _maxX, _minZ, _maxZ;
    private int _id, _index, _doneConstruction, _doneStage, _doneSlide, _doneCart;
    private Vector3 _moveSpot;
    private bool _isStun;
    private bool _isAscent;
    private bool _isLiftUp;
    private bool _isWater;
    private bool _isLava;
    private bool _isSlide;
    private bool _isFly;
    private bool _isJump;
    private bool _isChangeAnim;
    private GameObject _construction;
    private GameObject _checkPoint;
    private LevelManager _levelManager;
    private PlayerController _player;
    private EnemyController[] _enemy;
    private CameraFollow _camera;
    private SkinnedMeshRenderer _renderer;
    private Vector3 _targetDir;
    private Vector3 _newDir;
    private Vector3 _target;
    private Transform _myTransform;
    private double _accumulatedWeight;
    private readonly List<int> _value = new List<int>();
    private readonly List<double> _weight = new List<double>();
    private readonly System.Random _rand = new System.Random();

    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _id = _levelManager.enemyCount;
        switch (_id)
        {
            case 0:
                _index = CreateMapData.Instance.mapObjects[GameSave.PlayerLevel - 1].index[0];
                break;
            case 1:
                _index = CreateMapData.Instance.mapObjects[GameSave.PlayerLevel - 1].index[1];
                break;
            case 2:
                _index = CreateMapData.Instance.mapObjects[GameSave.PlayerLevel - 1].index[2];
                break;
        }

        _myTransform = transform;
        name.text = RandomName();
    }

    private void Start()
    {
        var randomData = 
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        startColor = _renderer.material.color;
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectsOfType<EnemyController>();
        _camera = FindObjectOfType<CameraFollow>();
        LoadMapData(_doneConstruction, _doneStage, _doneSlide);
        _checkPoint.GetComponent<Collider>().enabled = false;
        LoadEnemyAIData();
    }

    private void Update()
    {
        var rotation = _camera.transform.rotation;
        name.transform.LookAt(name.transform.position + rotation * -Vector3.back, rotation * Vector3.up);
        if (GameManager.Instance._state == GameManager.GameStates.Win ||
            GameManager.Instance._state == GameManager.GameStates.Lose && isDance == false)
        {
            ChangeState(EnemyStates.Idle);
        }

        if (ball.currentTimeScale == ball.maxTimeScale)
        {
            _countTime = 0;
            ChangeState(EnemyStates.GoBuild);
        }

        switch (state)
        {
            case EnemyStates.Idle:
                if (GameManager.Instance._state == GameManager.GameStates.Play)
                {
                    _moveSpot = GetNewPosition();
                    ChangeState(EnemyStates.Walk);
                }
                break;
            case EnemyStates.Walk:
                if (_isChangeAnim == false && ball.currentTimeScale > 5)
                {
                    _isChangeAnim = true;
                    enemyCharacter.AnimateWalkPush();
                }
                Move();
                break;
            case EnemyStates.Stun:
                _renderer.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * changeColorSpeed, 1));
                break;
            case EnemyStates.BuildRoad:
                break;
            case EnemyStates.TurnBack:
                if (IsMoveOnObject)
                {
                    return;
                }
                ChangeState(EnemyStates.Rested);
                break;
            case EnemyStates.Rested:
                break;
            case EnemyStates.LiftUp:
                break;
            case EnemyStates.Dance:
                break;
            case EnemyStates.BuildLadder:
                break;
            case EnemyStates.Slide:
                SlideAction();
                break;
            case EnemyStates.GoCart:
                GoCartAction();
                break;
            case EnemyStates.Attack:
                MoveToAttack();
                break;
            case EnemyStates.Afk:
                AfkAction();
                break;
            case EnemyStates.GoBuild:
                MoveToBuildRoad();
                break;
            case EnemyStates.Ice:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region RandomAction
    private void CalculateWeightAndIndices()
    {
        if (ball.currentTimeScale < 10)
        {
            for (int i = 0; i < _value.Count - 1; i++)
            {
                _accumulatedWeight += _value[i];
                _weight.Add(_accumulatedWeight);
            }
        }
        else
        {
            for (int i = 0; i < _value.Count; i++)
            {
                _accumulatedWeight += _value[i];
                _weight.Add(_accumulatedWeight);
            }
        }
    }
    
    private int GetRandomPieceIndex()
    {
        var r = _rand.NextDouble() * _accumulatedWeight;

        for (var i = 0; i < _weight.Count; i++)
            if (_weight[i] >= r)
                return i;

        return 0;
    }

    private void RandomAction()
    {
        CalculateWeightAndIndices();
        var index = GetRandomPieceIndex();
        switch (index)
        {
            case 0:
                //Debug.LogError("Walk");
                ChangeState(EnemyStates.Walk);
                break;
            case 1:
                //Debug.LogError("Attack");
                ChangeState(EnemyStates.Walk);
                break;
            case 2:
                //Debug.LogError("Afk");
                ChangeState(EnemyStates.Afk);
                break;
            case 3:
                //Debug.LogError("Build");
                ChangeState(EnemyStates.GoBuild);
                break;
        }
    }
    #endregion
    
    #region Action
    private void Move()
    {
        _countTime += Time.deltaTime;
        if (_countTime <= _random)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, _moveSpot, speed * Time.deltaTime);
            Turn();
            if (Vector3.Distance(_myTransform.position, _moveSpot) <= 0.2f)
            {
                if (_currentWaitTime <= 0)
                {
                    _moveSpot = GetNewPosition();
                    _currentWaitTime = waitTime;
                }
                else
                {
                    _currentWaitTime -= Time.deltaTime;
                }
            }
            ball.IncreaseSize();
        }
        else
        {
            _countTime = 0;
            RandomAction();
        }
    }

    private void Turn()
    {
        _targetDir = _moveSpot - _myTransform.position;
        _newDir = Vector3.RotateTowards(_myTransform.forward, _targetDir, 0.3f, 0);
        _myTransform.rotation = Quaternion.LookRotation(_newDir);
    }

    private void MoveToBuildRoad()
    {
        _target = _checkPoint.transform.position;
        _targetDir = _checkPoint.transform.position - _myTransform.position;
        _newDir = Vector3.RotateTowards(_myTransform.forward, _targetDir, 0.3f, 0);
        _myTransform.rotation = Quaternion.LookRotation(_newDir);
        _myTransform.position = Vector3.MoveTowards(_myTransform.position, _target, speed * Time.deltaTime);
    }

    private void MoveToAttack()
    {
        var rd = Random.Range(attackAction.timeMin, attackAction.timeMax);
        float time = 0;
        time += Time.deltaTime;
        if (_target != Vector3.zero && _target.z <= _maxZ && time <= rd)
        {
            _newDir = Vector3.RotateTowards(_myTransform.forward, _targetDir, 0.3f, 0);
            _myTransform.rotation = Quaternion.LookRotation(_newDir);
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, _target, speed * Time.deltaTime);
        }
        else
        {
            ChangeState(EnemyStates.Idle);
        }
    }

    private void LiftUp()
    {
        ball.transform.position = liftPoint.position;
        DOVirtual.DelayedCall(2f, delegate
        {
            ball.transform.position = startPoint.position;
            _isLiftUp = false;
            LoadMapData(_doneConstruction, _doneStage, _doneSlide);
            ChangeState(EnemyStates.Walk);
        });
    }

    private IEnumerator TimeStun(float timeFall)
    {
        _isStun = true;
        yield return new WaitForSeconds(timeFall);
        _isStun = false;
        ChangeState(EnemyStates.Idle);
    }
    
    private IEnumerator TimeIce(float timeIce)
    {
        _isStun = true;
        yield return new WaitForSeconds(timeIce);
        _isStun = false;
        ChangeState(EnemyStates.Idle);
    }

    private void AfkAction()
    {
        _countTime += Time.deltaTime;
        if (_countTime >= _random)
        {
            _countTime = 0;
            RandomAction();
        }
    }

    private void TryAttack()
    {
        if (ground == _player.ground && ball.point > _player.Ball.point)
        {
            Debug.LogError("Attack Player");
            _target = _player.transform.position;
            _targetDir = _player.transform.position - _myTransform.position;
        }
        else
        {
            for (int i = 0; i < _enemy.Length; i++)
            {
                if (ground == _enemy[i].ground && ball.point > _enemy[i].ball.point)
                {
                    Debug.LogError("Attack Enemy");
                    _target = _enemy[i].transform.position;
                    _targetDir = _enemy[i].transform.position - _myTransform.position;
                    break;
                }
                else
                {
                    Debug.LogError("Cant Attack");
                    _target = Vector3.zero;
                    RandomAction();
                }
            }
        }
    }
    
    private IEnumerator MoveOnBridge()
    {
        _myTransform.rotation = Quaternion.identity;
        var road = _construction.GetComponent<Construction>();
        if (road.isWater)
        {
            _isWater = true;
        }
        else if (road.isLava)
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
            ChangeState(EnemyStates.TurnBack);
        }
        else
        {
            if (_doneStage < _levelManager.ground.Count - 1)
            {
                _doneStage++;
            }
            if (_doneConstruction < _levelManager.enemyConstruction0.Count - 1)
            {
                _doneConstruction++;
            }
            if (_isLiftUp == false || _isSlide == false)
            {
                ChangeState(EnemyStates.Rested);
            }
        }
    }
    
    private IEnumerator EnemyModelTranslate(float duration)
    {
        enemyModel.transform.position = ball.topOfBall.transform.position;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            enemyModel.transform.position = Vector3.Lerp(enemyModel.transform.position, ball.topOfBall.transform.position, 0.8f);
            yield return null;
        }
        enemyModel.transform.localPosition = Vector3.zero;
    }
    
    private void SlideAction()
    {
        switch (_id)
        {
            case 0:
                var e0Dist1 = Vector3.Distance(_myTransform.position,
                    _levelManager.enemySlide0[_doneSlide].GetComponent<SlideController>().jumpPoint.position);

                if (parabolaController.Animation == false)
                {
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[_doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(EnemyStates.Rested);
                }
                if (e0Dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    enemyCharacter.AnimateFly();
                }
                break;
            case 1:
                var e1Dist1 = Vector3.Distance(_myTransform.position,
                    _levelManager.enemySlide1[_doneSlide].GetComponent<SlideController>().jumpPoint.position);
                
                if (parabolaController.Animation == false)
                {
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[_doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(EnemyStates.Rested);
                }
                if (e1Dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    enemyCharacter.AnimateFly();
                }
                break;
            case 2:
                var e2Dist1 = Vector3.Distance(_myTransform.position,
                    _levelManager.enemySlide2[_doneSlide].GetComponent<SlideController>().jumpPoint.position);

                if (parabolaController.Animation == false)
                {
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[_doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(EnemyStates.Rested);
                }
                if (e2Dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    enemyCharacter.AnimateFly();
                }
                break;
        }
    }

    private void GoCartAction()
    {
        switch (_id)
        {
            case 0:
                var e0Dist1 = Vector3.Distance(_levelManager.enemyCart0[_doneCart].transform.position,
                    _levelManager.enemySlide0[_doneSlide].GetComponent<SlideController>().jumpPoint.position);
                var e0Dist2 = Vector3.Distance(_levelManager.enemyCart0[_doneCart].transform.position,
                    _levelManager.enemySlide0[_doneSlide].GetComponent<SlideController>().fallPoint.position);

                if (_levelManager.enemyCart0[_doneCart].GetComponent<CartController>().parabolaController.Animation == false)
                {
                    _myTransform.parent = null;
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[_doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(EnemyStates.Walk);
                }
                if (e0Dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    enemyCharacter.AnimateFly();
                }
                if (e0Dist2 <= 0.3f)
                {
                    _levelManager.enemyCart0[_doneCart].GetComponent<CartController>().FallingDown();
                }
                break;
            case 1:
                var e1Dist1 = Vector3.Distance(_levelManager.enemyCart1[_doneCart].transform.position,
                    _levelManager.enemySlide1[_doneSlide].GetComponent<SlideController>().jumpPoint.position);
                var e1Dist2 = Vector3.Distance(_levelManager.enemyCart1[_doneCart].transform.position,
                   _levelManager.enemySlide1[_doneSlide].GetComponent<SlideController>().fallPoint.position);

                if (_levelManager.enemyCart1[_doneCart].GetComponent<CartController>().parabolaController.Animation == false)
                {
                    _myTransform.parent = null;
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[_doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(EnemyStates.Walk);
                }
                if (e1Dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    enemyCharacter.AnimateFly();
                }
                if (e1Dist2 <= 0.3f)
                {
                    _levelManager.enemyCart1[_doneCart].GetComponent<CartController>().FallingDown();
                }
                break;
            case 2:
                var e2Dist1 = Vector3.Distance(_levelManager.enemyCart2[_doneCart].transform.position,
                    _levelManager.enemySlide2[_doneSlide].GetComponent<SlideController>().jumpPoint.position);
                var e2Dist2 = Vector3.Distance(_levelManager.enemyCart2[_doneCart].transform.position,
                   _levelManager.enemySlide2[_doneSlide].GetComponent<SlideController>().fallPoint.position);

                if (_levelManager.enemyCart2[_doneCart].GetComponent<CartController>().parabolaController.Animation == false)
                {
                    _myTransform.parent = null;
                    var pos = _myTransform.position;
                    pos = new Vector3(pos.x, _levelManager.ground[_doneStage].transform.position.y + 2.83f, pos.z);
                    _myTransform.position = pos;
                    ChangeState(EnemyStates.Walk);
                }
                if (e2Dist1 <= 0.3f && _isFly == false)
                {
                    _isFly = true;
                    enemyCharacter.AnimateFly();
                }
                if (e2Dist2 <= 0.3f)
                {
                    _levelManager.enemyCart2[_doneCart].GetComponent<CartController>().FallingDown();
                }
                break;
        }
    }

    private IEnumerator MoveToDance()
    {
        var pos1 = _myTransform.position;
        var pos2 = GameObject.FindWithTag("ScorePos");
        float time = 0;
        while (time < timeMoveToDance)
        {
            _myTransform.position = Vector3.Lerp(pos1, pos2.transform.position, time / timeMoveToDance);
            time += Time.deltaTime;
            yield return null;
        }
        MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Lose);
        _myTransform.position = pos2.transform.position;
        _myTransform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        enemyCharacter.AnimateDance();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    #endregion

    #region Data
    private void GetGroundSize()
    {
        Renderer groundSize = ground.GetComponent<Renderer>();
        var bounds = groundSize.bounds;
        _minX = bounds.center.x - bounds.extents.x + 2f;
        _maxX = bounds.center.x + bounds.extents.x - 2f;
        _minZ = bounds.center.z - bounds.extents.z + 1.5f;
        _maxZ = bounds.center.z + bounds.extents.z - 1.5f;
    }

    private Vector3 GetNewPosition()
    {
        var rdX = Random.Range(_minX, _maxX);
        var rdZ = Random.Range(_minZ, _maxZ);
        var newPosition = new Vector3(rdX, _levelManager.ground[_doneStage].transform.position.y + 2.86f, rdZ);
        return newPosition;
    }
    
    private string RandomName()
    {
        return EnemyData.Instance.names[Random.Range(0, EnemyData.Instance.names.Length)];
    }

    private void CheckPlayerGround()
    {
        if (_player.doneStage > _doneStage)
        {
            Ball.ballValue = ballValueBuff;
        }
        else
        {
            Ball.ballValue = EnemyData.Instance.GetEnemyAIObjectData(_index).ballValue;
        }
        Debug.Log(Ball.ballValue);
    }

    private void LoadEnemyAIData()
    {
        var enemyAIData = EnemyData.Instance.GetEnemyAIObjectData(_index);
        if (enemyAIData != null)
        {
            ball.ballValue = enemyAIData.ballValue;
            speed = enemyAIData.speed;
            moveAction = enemyAIData.enemyAIAction.moveAction;
            _value.Add(moveAction.value);
            attackAction = enemyAIData.enemyAIAction.attackAction;
            _value.Add(attackAction.value);
            afkAction = enemyAIData.enemyAIAction.afkAction;
            _value.Add(afkAction.value);
            buildAction = enemyAIData.enemyAIAction.buildAction;
            _value.Add(buildAction.value);
        }
    }

    private void LoadMapData(int a, int b, int c)
    {
        ground = _levelManager.ground[b].gameObject;
        switch (_id)
            {
                case 0:
                    _construction = _levelManager.enemyConstruction0[a].gameObject;
                    _levelManager.enemyCheckPoint0[a].gameObject.SetActive(true);
                    _checkPoint = _levelManager.enemyCheckPoint0[a].gameObject;
                    parabolaController.ParabolaRoot = 
                        _levelManager.enemySlide0[c].GetComponent<SlideController>().parabolaRoot;
                    parabolaController.parabolaFly =
                        new ParabolaController.ParabolaFly(
                            _levelManager.enemySlide0[c].GetComponent<SlideController>().parabolaRoot.transform);
                    break;
                case 1:
                    _construction = _levelManager.enemyConstruction1[a].gameObject;
                    _levelManager.enemyCheckPoint1[a].gameObject.SetActive(true);
                    _checkPoint = _levelManager.enemyCheckPoint1[a].gameObject;
                    parabolaController.ParabolaRoot =
                        _levelManager.enemySlide1[c].GetComponent<SlideController>().parabolaRoot;
                    parabolaController.parabolaFly =
                        new ParabolaController.ParabolaFly(
                            _levelManager.enemySlide1[c].GetComponent<SlideController>().parabolaRoot.transform);
                break;
                case 2:
                    _construction = _levelManager.enemyConstruction2[a].gameObject;
                    _levelManager.enemyCheckPoint2[a].gameObject.SetActive(true);
                    _checkPoint = _levelManager.enemyCheckPoint2[a].gameObject;
                    parabolaController.ParabolaRoot =
                        _levelManager.enemySlide2[c].GetComponent<SlideController>().parabolaRoot;
                    parabolaController.parabolaFly =
                        new ParabolaController.ParabolaFly(
                            _levelManager.enemySlide2[c].GetComponent<SlideController>().parabolaRoot.transform);
                break;
            }
        GetGroundSize();
        _moveSpot = GetNewPosition();
        if (GameManager.Instance._state == GameManager.GameStates.Play)
        {
            ChangeState(EnemyStates.Walk);
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !_isStun)
        {
            var obj = other.GetComponentInParent<BallController>();
            if (ball.point <= obj.point)
            {
                if (obj.isPlayer)
                {
                    MusicManager.Instance.PlayOneShot(MusicDB.Instance.SFX_Stun);
                }
                ChangeState(EnemyStates.Stun);
                ball.point = 0;
                ball.transform.localScale = Vector3.one;
                ballModel.gameObject.SetActive(false);
                ball.currentTimeScale = 0;
                ball.isIncrease = false;
                ball.explosive.SetActive(true);
            }
        }

        if (other.CompareTag("CheckPoint1") && _id == 0 || 
            other.CompareTag("CheckPoint2") && _id == 1 ||
            other.CompareTag("CheckPoint3") && _id == 2)
        {
            ChangeState(EnemyStates.BuildRoad);
            collider.enabled = false;
        }
        
        if (other.CompareTag("EnemyLadder1") && _id == 0 || 
            other.CompareTag("EnemyLadder2") && _id == 1 ||
            other.CompareTag("EnemyLadder3") && _id == 2)
        {
            ChangeState(EnemyStates.BuildLadder);
            collider.enabled = false;
        }
        
        if (other.CompareTag("Elevator"))
        {
            _isLiftUp = true;
            ChangeState(EnemyStates.LiftUp);
        }

        if (other.CompareTag("FinishPos"))
        {
            GameManager.Instance.ChangeState(GameManager.GameStates.Lose);
            isDance = true;
            ChangeState(EnemyStates.Dance);
        }

        if (other.CompareTag("Slide"))
        {
            ChangeState(EnemyStates.Slide);
            _isSlide = true;
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
                    .OnComplete(() => ChangeState(EnemyStates.GoCart));
            }
        }
        
        if (other.CompareTag("Lava"))
        {
            ChangeState(EnemyStates.Stun);
            ball.point = 0;
            ball.transform.localScale = Vector3.one;
            ballModel.gameObject.SetActive(false);
            ball.currentTimeScale = 0;
            ball.isIncrease = false;
            ball.explosive.SetActive(true);
        }
        
        if (other.CompareTag("Ice"))
        {
            ChangeState(EnemyStates.Ice);
        }
    }

    #region FSM
    private void ChangeState(EnemyStates newstate)
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
            case EnemyStates.Idle:
                collider.enabled = true;
                enemyCharacter.AnimateIdle();
                break;
            case EnemyStates.Walk:
                CheckPlayerGround();
                _random = Random.Range(moveAction.timeMin, moveAction.timeMax);
                ball.snow.Play();
                ball.wheelTrack.emitting = true;

                if (ball.currentTimeScale > 5)
                {
                    enemyCharacter.AnimateWalkPush();
                }
                else
                {
                    enemyCharacter.AnimateWalkSmall();
                }
                break;
            case EnemyStates.Stun:
                enemyCharacter.AnimateStun();
                StartCoroutine(TimeStun(2f));
                break;
            case EnemyStates.BuildRoad:
                ball.snow.Play();
                ball.wheelTrack.emitting = false;
                StartCoroutine(_construction.GetComponent<Construction>().
                    FillRoad(ball.currentTimeScale,this));
                StartCoroutine(MoveOnBridge());
                if (_isWater)
                {
                    enemyCharacter.AnimateWalkWater();
                }
                else if (_isLava)
                {
                    StartCoroutine(EnemyModelTranslate(_construction.GetComponent<Construction>().duration));
                    enemyCharacter.AnimateWalkLava();
                }
                else
                {
                    enemyCharacter.AnimateWalkPush();
                }
                break;
            case EnemyStates.TurnBack:
                enemyModel.transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                StartCoroutine(_construction.GetComponent<Construction>().MoveBack(this));
                if (_isAscent)
                {
                    enemyCharacter.AnimateWalkWater();
                }
                else
                {
                    enemyCharacter.AnimateRunBack();
                }
                break;
            case EnemyStates.Rested:
                enemyModel.transform.localPosition = Vector3.zero;
                _checkPoint.GetComponent<Collider>().enabled = false;
                collider.enabled = true;
                enemyCharacter.AnimateIdle();
                LoadMapData(_doneConstruction, _doneStage, _doneSlide);
                break;
            case EnemyStates.LiftUp:
                ball.wheelTrack.emitting = false;
                enemyCharacter.AnimateLiftUp();
                LiftUp();
                break;
            case EnemyStates.Dance:
                ball.gameObject.SetActive(false);
                enemyCharacter.AnimateRunBack();
                StartCoroutine(MoveToDance());
                break;
            case EnemyStates.BuildLadder:
                ball.snow.Play();
                ball.wheelTrack.emitting = false;
                enemyCharacter.AnimateWalkPush();
                StartCoroutine(_construction.GetComponent<Construction>().
                    FillRoad(ball.currentTimeScale, this));
                StartCoroutine(MoveOnBridge());
                break;
            case EnemyStates.Slide:
                ball.wheelTrack.emitting = false;
                enemyCharacter.AnimateSlide();
                parabolaController.FollowParabola();
                break;
            case EnemyStates.GoCart:
                ball.wheelTrack.emitting = false;
                enemyCharacter.AnimateSlide();
                switch (_id)
                {
                    case 0:
                        _levelManager.enemyCart0[_doneCart].GetComponent<CartController>().parabolaController.FollowParabola();
                        break;
                    case 1:
                        _levelManager.enemyCart1[_doneCart].GetComponent<CartController>().parabolaController.FollowParabola();
                        break;
                    case 2:
                        _levelManager.enemyCart2[_doneCart].GetComponent<CartController>().parabolaController.FollowParabola();
                        break;
                }
                break;
            case EnemyStates.Attack:
                ball.snow.Play();
                if (ball.currentTimeScale > 5)
                {
                    enemyCharacter.AnimateWalkPush();
                }
                else
                {
                    enemyCharacter.AnimateWalkSmall();
                }
                TryAttack();
                break;
            case EnemyStates.Afk:
                _random = Random.Range(afkAction.timeMin, afkAction.timeMax);
                enemyCharacter.AnimateSwing();
                break;
            case EnemyStates.GoBuild:
                ball.wheelTrack.emitting = true;
                ball.snow.Play();
                _checkPoint.GetComponent<Collider>().enabled = true;
                enemyCharacter.AnimateWalkPush();
                break;
            case EnemyStates.Ice:
                enemyCharacter.animator.speed = 0;
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
            case EnemyStates.Idle:
                break;
            case EnemyStates.Walk:
                ball.snow.Stop();
                _isChangeAnim = false;
                break;
            case EnemyStates.Stun:
                _renderer.material.color = startColor;
                ball.explosive.SetActive(false);
                break;
            case EnemyStates.BuildRoad:
                Ball.point = Ball.currentTimeScale;
                ball.snow.Stop();
                _isLava = false;
                _isWater = false;
                break;
            case EnemyStates.TurnBack:
                ball.gameObject.SetActive(true);
                break;
            case EnemyStates.Rested:
                break;
            case EnemyStates.LiftUp:
                break;
            case EnemyStates.Dance:
                break;
            case EnemyStates.BuildLadder:
                Ball.point = Ball.currentTimeScale;
                ball.snow.Stop();
                _isAscent = false;
                break;
            case EnemyStates.Slide:
                switch (_id)
                {
                    case 0:
                        if (_doneSlide < _levelManager.enemySlide0.Count - 1)
                        {
                            _doneSlide++;
                        }
                        break;
                    case 1:
                        if (_doneSlide < _levelManager.enemySlide1.Count - 1)
                        {
                            _doneSlide++;
                        }
                        break;
                    case 2:
                        if (_doneSlide < _levelManager.enemySlide2.Count - 1)
                        {
                            _doneSlide++;
                        }
                        break;
                }
                _isFly = false;
                _isSlide = false;
                break;
            case EnemyStates.GoCart:
                switch (_id)
                {
                    case 0:
                        if (_doneSlide < _levelManager.enemySlide0.Count - 1)
                        {
                            _doneSlide++;
                        }
                        break;
                    case 1:
                        if (_doneSlide < _levelManager.enemySlide1.Count - 1)
                        {
                            _doneSlide++;
                        }
                        break;
                    case 2:
                        if (_doneSlide < _levelManager.enemySlide2.Count - 1)
                        {
                            _doneSlide++;
                        }
                        break;
                }
                
                switch (_id)
                {
                    case 0:
                        if (_doneCart < _levelManager.enemyCart0.Count - 1)
                        {
                            _doneCart++;
                        }
                        break;
                    case 1:
                        if (_doneCart < _levelManager.enemyCart1.Count - 1)
                        {
                            _doneCart++;
                        }
                        break;
                    case 2:
                        if (_doneCart < _levelManager.enemyCart2.Count - 1)
                        {
                            _doneCart++;
                        }
                        break;
                }
                _isJump = false;
                _isFly = false;
                break;
            case EnemyStates.Attack:
                ball.snow.Stop();
                break;
            case EnemyStates.Afk:
                break;
            case EnemyStates.GoBuild:
                ball.snow.Stop();
                break;
            case EnemyStates.Ice:
                enemyCharacter.animator.speed = 1;
                iceStun.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion
}
