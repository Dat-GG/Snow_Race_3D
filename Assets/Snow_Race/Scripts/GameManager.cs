using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public enum GameStates
    {
        Init,
        Play,
        Win,
        Lose
    }

    public GameStates _state = GameStates.Init;
    public float countTime;
    
    [SerializeField] private UIUnlockSkinController uiUnlock;
    [SerializeField] private GameObject rainbowBlue;
    [SerializeField] private GameObject reinDeer;
    [SerializeField] private GameObject warningRedPrefab;
    [SerializeField] private GameObject warningBluePrefab;
    private PlayerController _player;
    private float _minX, _maxX, _minZ, _maxZ;
    private bool _saveResult;
    
    //public List<int> mapRecord = new List<int>();
    public override void Awake()
    {
        base.Awake();
        GameUtils.EventHandlerIni();
        if (GameSave.SkinPlayer.Equals(""))
        {
            ShopData.Instance.itemData[0].isBuy = true;
            ShopData.Instance.itemData[0].Select();
            GameSave.SkinPlayer = "0";
        }

        // if (GameSave.MapRecord == "")
        // {
        //     mapRecord = new List<int> {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        //         0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        //         0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        //     GameSave.MapRecord = JsonConvert.SerializeObject(mapRecord);
        // }
        //
        // mapRecord = JsonConvert.DeserializeObject<List<int>>(GameSave.MapRecord);
    }

    private void Start()
    {
        MusicManager.Instance.PlayMusic(MusicDB.Instance.Music_BG);
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        switch (_state)
        {
            case GameStates.Init:
                break;
            case GameStates.Play:
                //countTime += Time.deltaTime;
                break;
            case GameStates.Win:
                if (GameSave.PlayerLevel == 1 && _player.Ball.isDone)
                {
                    _player.Ball.isDone = false;
                    GameSave.PlayerLevel++;
                    uiUnlock.gameObject.SetActive(true);
                    reinDeer.SetActive(true);
                }
                else if (GameSave.PlayerLevel == 4 && _player.Ball.isDone)
                {
                    _player.Ball.isDone = false;
                    GameSave.PlayerLevel++;
                    uiUnlock.gameObject.SetActive(true);
                    rainbowBlue.SetActive(true);
                }
                else if (_player.Ball.isDone)
                {
                    _player.Ball.isDone = false;
                    GameSave.PlayerLevel++;
                    if (GameSave.PlayerLevel > CreateMapData.Instance.mapObjects.Count - 1)
                    {
                        GameSave.PlayerLevel = 1;
                    }
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                }
                break;
            case GameStates.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private int _rd;
    private IEnumerator SpawnDisaster()
    {
        _rd = Random.Range(1, 4);
        for (int i = 0; i < _rd; i++)
        {
            float j = Random.Range(0, 1f);
            if (j > 0.5f)
            {
                Instantiate(warningRedPrefab, GetNewPosition(), Quaternion.identity);
            }
            else
            {
                Instantiate(warningBluePrefab, GetNewPosition(), Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(Random.Range(8f, 15f));
        StartCoroutine(SpawnDisaster());
    }
    
    private void GetGroundSize()
    {
        Renderer groundSize = _player.ground.GetComponent<Renderer>();
        var bounds = groundSize.bounds;
        _minX = bounds.center.x - bounds.extents.x + 4f;
        _maxX = bounds.center.x + bounds.extents.x - 4f;
        _minZ = bounds.center.z - bounds.extents.z + 4f;
        _maxZ = bounds.center.z + bounds.extents.z - 4f;
    }

    private Vector3 GetNewPosition()
    {
        GetGroundSize();
        var rdX = Random.Range(_minX, _maxX);
        var rdZ = Random.Range(_minZ, _maxZ);
        var newPosition = new Vector3(rdX, _player.ground.transform.position.y + 2.9f, rdZ);
        return newPosition;
    }
    
    #region FSM
    public void ChangeState(GameStates newstate)
    {
        if (newstate == _state) return;
        ExitCurrentState();
        _state = newstate;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (_state)
        {
            case GameStates.Init:
                break;
            case GameStates.Play:
                if (CreateMapData.Instance.mapObjects[GameSave.PlayerLevel - 1].isHaveDisaster)
                {
                    DOVirtual.DelayedCall(Random.Range(8f, 15f), delegate
                    {
                        StartCoroutine(SpawnDisaster());
                    });
                }
                break;
            case GameStates.Win:
                Firebase_data.Instance.WriteFireBaseCompleteLevel();
                GameTracking.Log_Level_PlayWin(GameSave.PlayerLevel);
                // if (countTime <= 45f)
                // {
                //     Debug.LogError("3 Star");
                //     mapRecord[GameSave.PlayerLevel -1] = 3;
                //     GameSave.MapRecord = JsonConvert.SerializeObject(mapRecord);
                // }
                // else if (countTime > 45f && countTime <= 90f)
                // {
                //     Debug.LogError("2 Star");
                //     mapRecord[GameSave.PlayerLevel -1] = 2;
                //     GameSave.MapRecord = JsonConvert.SerializeObject(mapRecord);
                // }
                // else
                // {
                //     Debug.LogError("1 Star");
                //     mapRecord[GameSave.PlayerLevel -1] = 1;
                //     GameSave.MapRecord = JsonConvert.SerializeObject(mapRecord);
                // }
                break;
            case GameStates.Lose:
                Firebase_data.Instance.WriteFireBaseLoseLevel();
                GameTracking.Log_Level_PlayLose(GameSave.PlayerLevel);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ExitCurrentState()
    {
        switch (_state)
        {
            case GameStates.Init:
                break;
            case GameStates.Play:
                break;
            case GameStates.Win:
                break;
            case GameStates.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        GameTracking.Log_Level_Quit(GameSave.PlayerLevel);
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        GameTracking.Log_Level_Pause(pauseStatus);
    }
}
