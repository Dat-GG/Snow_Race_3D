using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectMapData", menuName = "Assets/Scriptable Objects/ObjectMapData")]
public class ObjectMapData : ScriptableObject
{
    private static ObjectMapData _instance;
    public static ObjectMapData Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<ObjectMapData>("Data/ObjectMapData");
            return _instance;
        }
    }
    
    [TableList] public List<ObjectInfo> objectInfo = new List<ObjectInfo>();

    public ObjectInfo GetObjectByType(Challenge challenge)
    {
        for (int i = 0; i < objectInfo.Count; i++)
        {
            if (objectInfo[i].challenge == challenge)
            {
                return objectInfo[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class ObjectInfo
{
    public int id;
    public Challenge challenge;
    public GameObject prefab;
}
