//using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Service.RFirebase;

public class GameTracking
{
    public const string MISSION_PLAY_COUNT = "mission_play_count";
    public const string MISSION_WIN_COUNT = "mission_win_count";
    public const string MISSION_LOSE_COUNT = "mission_lose_count";
    public const string MISSION_REBIRTH = "mission_rebirth";
    public const string MISSION_EXIT = "level_exit";
    public const string USER_PROPERTY_PAUSE = "pause";
    public const string MISSION_LOSE_TOTAL = "mission_lose_total";

    public const string PARAM_MISSION = "mission";


    public const string USER_PROPERTY_MAX_LEVEL = "max_level";
    //public const string USER_PROPERTY_MAX_MISSION = "max_level";
    //  public const string USER_PROPERTY_MAX_GAME_STEP = "max_level";
    //  public const string USER_PROPERTY_LAST = "max_level";

    public static void Log_Level_PlayCount(int missionIndex)
    {
        LogEvent(MISSION_PLAY_COUNT, PARAM_MISSION, missionIndex);
        LogEvent(MISSION_PLAY_COUNT + "_" + missionIndex.ToString("000"));
    }

    public static void Log_Level_PlayWin(int missionIndex)
    {
        LogEvent(MISSION_WIN_COUNT, PARAM_MISSION, missionIndex);
        LogEvent(MISSION_WIN_COUNT + "_" + missionIndex.ToString("000"));
    }
    public static void Log_Level_PlayLose(int missionIndex)
    {
        LogEvent(MISSION_LOSE_COUNT, PARAM_MISSION, missionIndex);
        LogEvent(MISSION_LOSE_COUNT + "_" + missionIndex.ToString("000"));
    }

    public static void Log_Level_Rebirth(int missionIndex)
    {
        LogEvent(MISSION_REBIRTH, PARAM_MISSION, missionIndex);
        LogEvent(MISSION_REBIRTH + "_" + missionIndex.ToString("000"));
    }

    public static void Log_Level_Quit(int missionIndex)
    {
        //FirebaseAnalytics.SetUserProperty(USER_PROPERTY_MAX_LEVEL, missionIndex.ToString());
        LogEvent(MISSION_EXIT + "_" + missionIndex.ToString("000"));
    }
    
    public static void Log_Level_Pause(bool isPause)
    {
        if (isPause)
        {
            //FirebaseAnalytics.SetUserProperty(USER_PROPERTY_PAUSE, 0.ToString());
        }
    }

    public static void UpdatePlayerProperty()
    { }

    public static void LogEvent(string eventName, string paramName, int paramValue)
    {
        try
        {
            // if (RFirebaseManager.initialized)
            //     FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
        }
        catch (System.Exception ex)
        {

            Debug.LogError(ex.ToString());
        }

    }

    public static void LogEvent(string eventName)
    {
        try
        {
            // if (RFirebaseManager.initialized)
            //     FirebaseAnalytics.LogEvent(eventName);
        }
        catch (System.Exception ex)
        {

            Debug.LogError(ex.ToString());
        }

    }

    public static string PrependWithNum(int f, int numToInsert, int nTimes)
    {
        string str = f.ToString();
        for (int i = 0; i < nTimes - 1; i++)
        {
            str = str.Insert(0, numToInsert.ToString());
        }
        return str;
    }
}
