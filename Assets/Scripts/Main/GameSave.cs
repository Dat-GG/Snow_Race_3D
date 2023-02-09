using UnityEngine;

public class GameSave
{
    public static string Cache_UserName
    {
        get
        {
            return PlayerPrefs.GetString(GameSaveKey.KEY_USERNAME, "PLAYER");
        }
        set
        {
            PlayerPrefs.SetString(GameSaveKey.KEY_USERNAME, value);
            PlayerPrefs.Save();
        }
    }

    public static string Cache_Password
    {
        get
        {
            return PlayerPrefs.GetString(GameSaveKey.KEY_PASSWORD, "");
        }
        set
        {
            PlayerPrefs.SetString(GameSaveKey.KEY_PASSWORD, value);
            PlayerPrefs.Save();
        }
    }
    
    public static bool ItemBuy_Get(ItemData.ItemType itemType, string ID)
    {
#if DEV
return true;
#endif
        return PlayerPrefs.GetInt(GameSaveKey.KEY_ITEM_BUY + itemType.ToString() + ID, 0) == 1;
    }
    public static void ItemBuy_Set(ItemData.ItemType itemType, string ID, bool isBuy)
    {
        PlayerPrefs.SetInt(GameSaveKey.KEY_ITEM_BUY + itemType.ToString() + ID, isBuy ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static int Item_Video_Get(ItemData.ItemType itemType, string ID)
    {
        return PlayerPrefs.GetInt(GameSaveKey.KEY_ITEM_VIDEO_WATCHED + itemType.ToString() + ID, 0);
    }
    public static void Item_Video_Add(ItemData.ItemType itemType, string ID)
    {

        int _value = Item_Video_Get(itemType, ID);
        PlayerPrefs.SetInt(GameSaveKey.KEY_ITEM_VIDEO_WATCHED + itemType.ToString() + ID, _value + 1);
        PlayerPrefs.Save();
    }
    
    public static string SkinPlayer
    {
        get
        {
            return PlayerPrefs.GetString(GameSaveKey.KEY_SKIN_PLAYER, "");
        }
        set
        {
            PlayerPrefs.SetString(GameSaveKey.KEY_SKIN_PLAYER, value);
            PlayerPrefs.Save();
        }
    }

    public static string SkinBall
    {
        get
        {
            return PlayerPrefs.GetString(GameSaveKey.KEY_SKIN_BALL, "");
        }
        set
        {
            PlayerPrefs.SetString(GameSaveKey.KEY_SKIN_BALL, value);
            PlayerPrefs.Save();
        }
    }


    public static int PlayerLevel
    {
        get
        {
            return PlayerPrefs.GetInt(GameSaveKey.KEY_LEVEL, 1);
        }
        set
        {
            PlayerPrefs.SetInt(GameSaveKey.KEY_LEVEL, value);
            PlayerPrefs.Save();
        }
    }
    
    public static int CountFistOpen
    {
        get
        {
            return PlayerPrefs.GetInt(GameSaveKey.KEY_COUNT_FIRST_OPEN, 0);
        }
        set
        {
            PlayerPrefs.SetInt(GameSaveKey.KEY_COUNT_FIRST_OPEN, value);
            PlayerPrefs.Save();
        }
    }

    public static int PlayerCoin
    {
        get
        {
#if DEV
return 9999;
#endif
            return PlayerPrefs.GetInt(GameSaveKey.KEY_COIN, 0);
        }
        set
        {


            PlayerPrefs.SetInt(GameSaveKey.KEY_COIN, value);
            PlayerPrefs.Save();
        }
    }


    public static bool NoAds
    {
        get
        {
            return PlayerPrefsX.GetBool(GameSaveKey.KEY_NOADS, false);
        }
        set
        {
            PlayerPrefsX.SetBool(GameSaveKey.KEY_NOADS, value);
            PlayerPrefs.Save();
        }
    }
    
    public static string MapRecord
    {
        get
        {
            return PlayerPrefs.GetString(GameSaveKey.KEY_MAP_RESULT, "");
        }
        set
        {
            PlayerPrefs.SetString(GameSaveKey.KEY_MAP_RESULT, value);
            PlayerPrefs.Save();
        }
    }
}

public class GameSaveKey
{
    public const string KEY_NOADS = "KEY_NOADS";
    public const string KEY_COUNT_FIRST_OPEN = "KEY_COUNT_FIRST_OPEN";

    public const string KEY_USERNAME = "KEY_USERNAME";
    public const string KEY_PASSWORD = "KEY_PASSWORD";

    public const string KEY_LEVEL = "KEY_LEVEL";
    public const string KEY_COIN = "KEY_COIN";
    public const string KEY_SKIN_PLAYER = "KEY_SKIN_PLAYER";
    public const string KEY_SKIN_BALL = "KEY_SKIN_BASKET";


    public const string KEY_ITEM_VIDEO_WATCHED = "KEY_ITEM_VIDEO_WATCHED";
    public const string KEY_ITEM_BUY = "KEY_ITEM_BUY";

    public const string KEY_MAP_RESULT = "KEY_MAP_RESULT";
}

public enum ResourceType
{
    COIN
}