using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateMapData", menuName = "Assets/Scriptable Objects/CreateMapData")]
public class CreateMapData : ScriptableObject
{
    private static CreateMapData _instance;
    public static CreateMapData Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<CreateMapData>("Data/CreateMapData");
            return _instance;
        }
    }
    [TableList] public List<MapObjects> mapObjects = new List<MapObjects>();
}

[System.Serializable]
public class MapObjects
{
    public int id;
    public List<ConstructionType> constructionType;
    public List<int> index;
    public bool isHaveDisaster;
}

public enum Challenge
{
    Island1,
    Island2,
    Island3,
    Bridge,
    BridgeSlide,
    BridgeRailway,
    BridgeElevator,
    AscentBridge,
    AscentBridgeSlide,
    AscentBridgeRailway,
    AscentBridgeElevator,
    DescentBridge,
    DescentBridgeSlide,
    DescentBridgeRailway,
    DescentBridgeElevator,
    LadderUp,
    LadderUpSlide,
    LadderUpRailway,
    LadderUpElevator,
    LadderDown,
    LadderDownSlide,
    LadderDownRailway,
    LadderDownElevator,
    ScoreIsland,
    Island4,
    Island5
}

[System.Serializable]
public class ConstructionType
{
    public Challenge challenge;
    public bool isAscent;
    public bool isBridge;
    public bool isEnd;
    [ShowIf("isBridge")]public int roadLength;
    [ShowIf("isBridge")]public bool isLava;
    [ShowIf("isBridge")]public bool isWater;
    [ShowIf("isBridge")]public bool isHaveSlide;
    [ShowIf("isBridge")]public bool isHaveRailway;
}


