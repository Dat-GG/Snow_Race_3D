using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private bool isHaveEnemy;
    [SerializeField] private Image bgImg;
    [SerializeField] private Sprite[] backgroundImg;
    public GameObject playerPrefab;
    public List<GameObject> enemyPrefab;
    public List<Ground> ground;
    public List<SkiController> ski;
    //Construction
    public List<PlayerConstruction> playerConstruction;
    public List<EnemyConstruction0> enemyConstruction0;
    public List<EnemyConstruction1> enemyConstruction1;
    public List<EnemyConstruction2> enemyConstruction2;
    //CheckPoint
    public List<PlayerCheckPoint> playerCheckPoint;
    public List<EnemyCheckPoint0> enemyCheckPoint0;
    public List<EnemyCheckPoint1> enemyCheckPoint1;
    public List<EnemyCheckPoint2> enemyCheckPoint2;
    //Slide
    public List<EnemySlide0> enemySlide0;
    public List<EnemySlide1> enemySlide1;
    public List<EnemySlide2> enemySlide2;
    public List<PlayerSlide> playerSlide;
    //Cart
    public List<PlayerCart> playerCart;
    public List<EnemyCart0> enemyCart0;
    public List<EnemyCart1> enemyCart1;
    public List<EnemyCart2> enemyCart2;
    public int enemyCount;
    private float _minX, _maxX, _minZ, _maxZ;
    private GameObject _enemy;
    private GameObject _player;

    public override void Awake()
    {
        base.Awake();
        LoadBackground();
        SpawnMap(GameSave.PlayerLevel - 1);
        playerPrefab = ShopData.Instance.PlayerSkinSelect().prefab;
        LoadData();
        GetGroundSize();
        SpawnPlayer();
        if (isHaveEnemy)
        { 
            SpawnEnemy();
        }
        SetUpForEnemy();
        SetUpForPlayer();
    }

    [Button]
    private void SpawnMap(int level)
    {
        Vector3 spawnPos = transform.position;
        for (int i = 0; i < CreateMapData.Instance.mapObjects[level].constructionType.Count; i++)
        {
            GameObject _go =  Instantiate(ObjectMapData.Instance.GetObjectByType
                (CreateMapData.Instance.mapObjects[level].constructionType[i].challenge).prefab, spawnPos, Quaternion.identity);
            
            if (CreateMapData.Instance.mapObjects[level].constructionType[i].isEnd)
            {
                _go.GetComponent<PieceOfMap>().isEnd = true;
                _go.GetComponent<PieceOfMap>().SetUp();
            }
            
            if (CreateMapData.Instance.mapObjects[level].constructionType[i].isBridge)
            {
                foreach (Transform obj in _go.transform)
                {
                    var bridge = obj.GetComponent<Construction>();
                    bridge.roadSize = CreateMapData.Instance.mapObjects[level].constructionType[i].roadLength;
                    bridge.Init();
                }

                if (CreateMapData.Instance.mapObjects[level].constructionType[i].isHaveSlide)
                {
                    var pos = _go.GetComponent<PieceOfMap>().slideIsland.transform.position;
                    pos = new Vector3(pos.x, pos.y,
                        pos.z + CreateMapData.Instance.mapObjects[level].constructionType[i].roadLength);
                    _go.GetComponent<PieceOfMap>().slideIsland.transform.position = pos;
                }
                
                if (CreateMapData.Instance.mapObjects[level].constructionType[i].isHaveRailway)
                {
                    var pos = _go.GetComponent<PieceOfMap>().railwayIsland.transform.position;
                    pos = new Vector3(pos.x, pos.y,
                        pos.z + CreateMapData.Instance.mapObjects[level].constructionType[i].roadLength);
                    _go.GetComponent<PieceOfMap>().railwayIsland.transform.position = pos;
                }
            }
            
            if (CreateMapData.Instance.mapObjects[level].constructionType[i].isLava)
            {
                foreach (Transform obj in _go.transform)
                {
                    var bridge = obj.GetComponent<Construction>();
                    bridge.isLava = true;
                    bridge.Init();
                }
            }
            else if (CreateMapData.Instance.mapObjects[level].constructionType[i].isWater)
            {
                foreach (Transform obj in _go.transform)
                {
                    var bridge = obj.GetComponent<Construction>();
                    bridge.isWater = true;
                    bridge.Init();
                }
            }
            spawnPos = _go.GetComponent<PieceOfMap>().endPoint.transform.position;
        }
        GameTracking.Log_Level_PlayCount(level);
    }
    
    [Button]
    private void LoadData()
    {
        ground = new List<Ground>(FindObjectsOfType<Ground>());
        ground.Reverse();
        ski = new List<SkiController>(FindObjectsOfType<SkiController>());
        ski.Reverse();
        playerConstruction = new List<PlayerConstruction>(FindObjectsOfType<PlayerConstruction>());
        playerConstruction.Reverse();
        playerCheckPoint = new List<PlayerCheckPoint>(FindObjectsOfType<PlayerCheckPoint>());
        playerCheckPoint.Reverse();
        playerSlide = new List<PlayerSlide>(FindObjectsOfType<PlayerSlide>());
        playerSlide.Reverse();
        playerCart = new List<PlayerCart>(FindObjectsOfType<PlayerCart>());
        playerCart.Reverse();
        enemyConstruction0 = new List<EnemyConstruction0>(FindObjectsOfType<EnemyConstruction0>());
        enemyConstruction0.Reverse();
        enemyConstruction1 = new List<EnemyConstruction1>(FindObjectsOfType<EnemyConstruction1>());
        enemyConstruction1.Reverse();
        enemyConstruction2 = new List<EnemyConstruction2>(FindObjectsOfType<EnemyConstruction2>());
        enemyConstruction2.Reverse();
        enemyCheckPoint0 = new List<EnemyCheckPoint0>(FindObjectsOfType<EnemyCheckPoint0>());
        enemyCheckPoint0.Reverse();
        enemyCheckPoint1 = new List<EnemyCheckPoint1>(FindObjectsOfType<EnemyCheckPoint1>());
        enemyCheckPoint1.Reverse();
        enemyCheckPoint2 = new List<EnemyCheckPoint2>(FindObjectsOfType<EnemyCheckPoint2>());
        enemyCheckPoint2.Reverse();
        enemySlide0 = new List<EnemySlide0>(FindObjectsOfType<EnemySlide0>());
        enemySlide0.Reverse();
        enemySlide1 = new List<EnemySlide1>(FindObjectsOfType<EnemySlide1>());
        enemySlide1.Reverse();
        enemySlide2 = new List<EnemySlide2>(FindObjectsOfType<EnemySlide2>());
        enemySlide2.Reverse();
        enemyCart0 = new List<EnemyCart0>(FindObjectsOfType<EnemyCart0>());
        enemyCart0.Reverse();
        enemyCart1 = new List<EnemyCart1>(FindObjectsOfType<EnemyCart1>());
        enemyCart1.Reverse();
        enemyCart2 = new List<EnemyCart2>(FindObjectsOfType<EnemyCart2>());
        enemyCart2.Reverse();
    }

    private Vector3 GetNewPosition()
    {
        var rdX = Random.Range(_minX, _maxX);
        var rdZ = Random.Range(_minZ, _maxZ);
        var newPosition = new Vector3(rdX, ground[0].transform.position.y + 2.86f, rdZ);
        return newPosition;
    }
    
    private void GetGroundSize()
    {
        Renderer groundSize = ground[0].GetComponent<Renderer>();
        var bounds = groundSize.bounds;
        _minX = bounds.center.x - bounds.extents.x + 2f;
        _maxX = bounds.center.x + bounds.extents.x - 2f;
        _minZ = bounds.center.z - bounds.extents.z + 1.5f;
        _maxZ = bounds.center.z + bounds.extents.z - 1.5f;
    }

    private void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab, GetNewPosition(), Quaternion.identity);
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < enemyPrefab.Count; i++)
        {
            _enemy = Instantiate(enemyPrefab[i], GetNewPosition(), Quaternion.identity);
            enemyCount++;
        }
    }

    private void SetUpForPlayer()
    {
        for (int i = 1; i < playerCheckPoint.Count; i++)
        {
            playerCheckPoint[i].gameObject.SetActive(false);
        }
    }

    private void SetUpForEnemy()
    {
        for (int i = 1; i < enemyCheckPoint0.Count; i++)
        {
            enemyCheckPoint0[i].gameObject.SetActive(false);
        }
        
        for (int i = 1; i < enemyCheckPoint1.Count; i++)
        {
            enemyCheckPoint1[i].gameObject.SetActive(false);
        }
        
        for (int i = 1; i < enemyCheckPoint2.Count; i++)
        {
            enemyCheckPoint2[i].gameObject.SetActive(false);
        }
    }

    private void LoadBackground()
    {
        if (GameSave.PlayerLevel < 3)
        {
            bgImg.sprite = backgroundImg[0];
        }
        else
        {
            var index = Random.Range(0, backgroundImg.Length);
            bgImg.sprite = backgroundImg[index];
        }
    }
}
