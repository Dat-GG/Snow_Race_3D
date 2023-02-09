using UnityEngine;
public class PieceOfMap : MonoBehaviour
{
    public Transform endPoint;
    public bool isEnd;
    [SerializeField] private GameObject normalIsland;
    [SerializeField] private GameObject scoreIsland;
    public GameObject slideIsland;
    public GameObject railwayIsland;

    public void SetUp()
    {
        if (isEnd)
        {
            normalIsland.SetActive(false);
            scoreIsland.SetActive(true);
        }
    }
}
