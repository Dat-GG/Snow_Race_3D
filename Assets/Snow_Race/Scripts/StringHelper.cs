using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    public const string DATAVERSION = "DATAVERSION";
    public const string GOLD = "GOLD";
    public const string TUTORIAL = "TUTORAIL";
    public const string INDEX_UNLOCK = "INDEX_UNLOCK";
    public const string UNLOCK_PROGRESS_AMOUNT = "UNLOCK_PROGRESS_AMOUNT";
    public const string CURRENT_PIECE_REWARD = "CURRENT_PIECE_REWARD";
    public const string LEVEL_PLAYER = "LEVEL_PLAYER";
    public const string PLAYER_NAME = "PLAYER_NAME";
    public const string CURRENT_SKIN = "CURRENT_SKIN";
    public const string CURRENT_WEAPON = "CURRENT_WEAPON";
    public const string REMOVE_ADS = "REMOVE_ADS";
    public const string LAST_TIME_DAILY_LOGIN = "LAST_TIME_DAILY_LOGIN";
    public const string CURRENT_ID_DAILY_LOGIN = "CURRENT_ID_DAILY_LOGIN";
    public const string PLAYER_UNLOCK_DATA = "PLAYER_UNLOCK_DATA";
    public const string LAST_TIME_SHOW_INTER_ADS = "LAST_TIME_SHOW_INTER_ADS";
    public const string LAST_TIME_SHOW_ADS = "LAST_TIME_SHOW_ADS";
    public const string LAST_TIME_SHOW_ADS_ADD_PIECE = "LAST_TIME_SHOW_ADS_ADD_PIECE";
    public const string LAST_TIME_SHOW_ADS_UNLOCK_SKIN_ON_HOME = "LAST_TIME_SHOW_ADS_UNLOCK_SKIN_ON_HOME";
    public const string LAST_TIME_SHOW_ADS_REVIVE = "LAST_TIME_SHOW_ADS_REVIVE";
    
    public class GameIAPID
    {
        public const string ID_NO_ADS = "com.fishking.io.removeads";
    }
    public static string StringColor(Enums.ColorString color, string str)
    {
        switch (color)
        {
            case Enums.ColorString.yellow:
                return $"<color=yellow>{str}</color>";
            case Enums.ColorString.green:
                return $"<color=green>{str}</color>";
            case Enums.ColorString.red:
                return $"<color=red>{str}</color>";
            default:
                return str;
        }
    }

    public static string StringColorHexan(string str, string hexan)
    {
        return $"<color=\"{hexan}\">{str}</color>";
    }

    public const string LAYER_GUI_EFFECT = "GUI Effect";
}
public static class ObserverName
{
    public const string ON_FISH_DIE = "ON_FISH_DIE";
    public const string WATCH_ADS = "WATCH_ADS";
    public const string ON_PLAYER_DIE = "ON_PLAYER_DIE";
    public const string ON_START_PLAY = "ON_START_PLAY";
    public const string ON_END_GAME = "ON_END_GAME";
    public const string ON_FISH_CHANGE_POINT = "ON_FISH_CHANGE_POINT";
    public const string ON_CHANGE_NAME = "ON_CHANGE_NAME";
    public const string ON_PLAYER_REVIVE = "ON_PLAYER_REVIVE";
    public const string ON_CLICK_SHOP_FISH_SLOT = "ON_CLICK_SHOP_FISH_SLOT";
    public const string ON_CLICK_SHOP_WEAPON_SLOT = "ON_CLICK_SHOP_WEAPON_SLOT";
    public const string ON_CLICK_SELECT_FISH = "ON_CLICK_SELECT_FISH";
    public const string ON_CLICK_UNLOCK_FISH = "ON_CLICK_UNLOCK_FISH";
    public const string ON_CLICK_SELECT_WEAPON = "ON_CLICK_SELECT_WEAPON";
    public const string ON_CLICK_UNLOCK_WEAPON = "ON_CLICK_UNLOCK_WEAPON";
    public const string ON_PLAYER_LEVEL_UP = "ON_PLAYER_LEVEL_UP";
    public const string ON_PLAYER_PICK_UP_GREEN_PEARL = "ON_PLAYER_PICK_UP_GREEN_PEARL";
}
