using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Assets/Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    private static EnemyData _instance;
    public static EnemyData Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<EnemyData>("Data/EnemyData");
            return _instance;
        }
    }
    
    [TableList] public List<EnemyAIObjects> enemyAIObjects = new List<EnemyAIObjects>();
    public List<int> rewardPerLevel;

    public string[] names =
    {
        "Karen", "Mercy", "Aether", "Lamine", "Milo", "Hanna", "Jerry", "Tom", "Cole", "Thor", "Jack", "Dawson",
        "Phoenix", "Billy", "King", "Queen", "Jim", "Chris", "Ivan", "Logan", "Olsen", "Albedo", "Dilute", "Chilled"," Yang"
    };

    public int RewardByLevel(int levelIndex)
    {
        return rewardPerLevel[levelIndex];
    }

    public EnemyAIObjects GetEnemyAIObjectData(int id)
    {
        for (int i = 0; i < enemyAIObjects.Count; i++)
        {
            if (enemyAIObjects[i].id == id)
            {
                return enemyAIObjects[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class EnemyAIObjects
{
    public int id;
    public Mode mode;
    [Range(0f, 5f)]public float ballValue;
    [Range(5f, 10f)] public float speed;
    public EnemyAIAction enemyAIAction;
}

[System.Serializable]
public class EnemyAIAction
{
    public MoveAction moveAction;
    public AttackAction attackAction;
    public AfkAction afkAction;
    public BuildAction buildAction;
}

[System.Serializable]
public class MoveAction
{
    public float timeMin;
    public float timeMax;
    [Tooltip("Probability in %")] [Range(0f, 100f)]
    public int value;
}

[System.Serializable]
public class AttackAction
{
    public float timeMin;
    public float timeMax;
    [Tooltip("Probability in %")] [Range(0f, 100f)]
    public int value;
}

[System.Serializable]
public class AfkAction
{
    public float timeMin;
    public float timeMax;
    [Tooltip("Probability in %")] [Range(0f, 100f)]
    public int value;
}

[System.Serializable]
public class BuildAction
{
    [Tooltip("Probability in %")] [Range(0f, 100f)]
    public int value;
}

public enum Mode
{
    Easy,
    Medium,
    Hard
}
