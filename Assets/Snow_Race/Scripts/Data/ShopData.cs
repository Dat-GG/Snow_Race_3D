using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Assets/Scriptable Objects/ShopData")]
public class ShopData : ScriptableObject
{
    private static ShopData _instance;
    public static ShopData Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<ShopData>("Data/ShopData");
            return _instance;
        }
    }
    
    [TableList] public ItemData[] itemData;
    // [Button("OnValidate")]
    // private void OnValidate()
    // {
    //     foreach (var item in itemData)
    //     {
    //         item.itemType = ItemData.ItemType.PLAYER;
    //     }
    //     itemData = itemData.OrderBy(x => int.Parse(x.ID)).ToArray();
    // }
    public ItemData PlayerSkinSelect()
    {
        foreach (var item in itemData)
        {
            if (item.isSelect()) return item;
        }
        return null;
    }
    
    public List<string> IAP_ids
    {
        get
        {
            List<string> _output = new List<string>();
            foreach (var item in itemData)
            {
                if (item.unlockType == ItemData.UnlockType.MONEY)
                {
                    if (string.IsNullOrEmpty(item.IAPID))
                        Debug.LogError("item.IAPID null");
                    _output.Add(item.IAPID);
                }
            }

            return _output;
        }
    }
}

[System.Serializable]
public class ItemData
{
    public enum ItemType
    {
        PLAYER,
        BALL
    }

    public enum UnlockType
    {
        COIN,
        MONEY,
        VIDEO
    }

    public ItemType itemType;
    public UnlockType unlockType;
    public string ID;
    public string name;
    public string IAPID;
    public float IAPPrice;
    public Sprite sprView;
    public int priceCoin;
    public Material mat;
    public GameObject prefab;
    public string customData;

    public bool isBuy
    {
        get => GameSave.ItemBuy_Get(itemType, ID);
        set => GameSave.ItemBuy_Set(itemType, ID, value);
    }

    public int AdsWatched_Get => GameSave.Item_Video_Get(itemType, ID);

    public void AdsWatched()
    {
        GameSave.Item_Video_Add(itemType, ID);
    }

    public bool isSelect()
    {
        if (!isBuy) return false;
        switch (itemType)
        {
            case ItemType.PLAYER:
                return GameSave.SkinPlayer.Equals(this.ID);
                break;
            case ItemType.BALL:
                return GameSave.SkinBall.Equals(this.ID);
                break;
            default:
                return false;
                break;
        }
    }

    public void Select()
    {
        if (!isBuy) return;
        switch (itemType)
        {
            case ItemType.PLAYER:
                GameSave.SkinPlayer = (this.ID);
                GameUtils.RaiseMessage(GameEvent.OnSkinChange.Instance);
                break;
            case ItemType.BALL:
                GameSave.SkinBall = (this.ID);
                //GameUtils.RaiseMessage(GameEvent.OnBasketChange.Instance);
                break;
            default:
                //      return false;
                break;
        }
    }

}
