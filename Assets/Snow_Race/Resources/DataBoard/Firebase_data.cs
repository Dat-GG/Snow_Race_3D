using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Firebase;
// using Firebase.Analytics;
// using Firebase.Database;
using TMPro;
using Proyecto26;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using RandomNameAndCountry;


public class Firebase_data : Singleton<Firebase_data>
{

    bool InstallGame;
    //[SerializeField] DatabaseReference reference;
    [SerializeField] int LevelComplete;// số lượt chơi qua màn 
    [SerializeField] public string Database;
    ReadIndexLevelComplete write = new ReadIndexLevelComplete();
    private object console;


    [Header("ReadData")]
    public List<LevelData1> name = new List<LevelData1>();
    public TextAsset TextJson;
    int NumberPlayer;
    bool check = false;
    int CoutPlayer;
    [System.Serializable]
    public class LevelData
    {

        public int Level;
        public float NumberLevelPlay;
        public float NumberLevelComplete;
        public int CoutRemove = 1;

    }

    [System.Serializable]
    public class LevelData1
    {
        public string levelstring;
        public List<NamePl> namepl = new List<NamePl>();
    }
    [System.Serializable]
    public class NamePl
    {
        public string _namePlayer;
        public List<LevelData> level = new List<LevelData>();
    }
    public int _CreateName;


    void Start()
    {

        check = false;
        InstallGame = true;
        if (InstallGame == true)
        {
            InstallGame = false;
        }


        LevelComplete = GameSave.PlayerLevel;
        ReadData();
        Invoke("ReadData", 1f);
        Invoke("JsonParseList", 5f);
        _CreateName = PlayerPrefs.GetInt("name", _CreateName);

    }


    public void OnEndEdit()
    {
        Database = "https://snow-race-io-default-rtdb.asia-southeast1.firebasedatabase.app/";
    }

    [Button]

    public void WriteFireBaseCompleteLevel()
    {
        // Win
        Database = "https://snow-race-io-default-rtdb.asia-southeast1.firebasedatabase.app/";

        if (name.Count == 0)
        {
            write.NumberLevelPlay = 1;

            Debug.Log("1");
        }
        if (name[name.Count - 1].levelstring != DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
        {
            write.NumberLevelPlay = 1;
            Debug.Log("Hello");
        }
        for (int i = 0; i < name.Count; i++)
        {

            if (name[i].levelstring == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
            {

                foreach (var namePlayer in name[i].namepl)
                {
                    Debug.Log("2");
                    if (namePlayer == null)
                    {
                        write.NumberLevelPlay = 1;
                        Debug.Log("3");
                    }
                    foreach (var level in namePlayer.level)
                    {
                        if (GameSave.PlayerLevel != level.Level)
                        {
                            write.NumberLevelPlay = 1;
                            Debug.Log("4");
                        }
                        else
                        {
                            check = true;

                        }
                    }

                }

            }
        }
        if (check == true)
        {
            for (int i = 0; i < name.Count; i++)
            {
                if (name[i].levelstring == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                {
                    for (int j = 0; j < name[i].namepl.Count; j++)
                    {
                        if (name[i].namepl[j]._namePlayer == PlayerPrefs.GetInt("name", _CreateName).ToString() + "Name")
                        {
                            Debug.Log("Day: " + name[i].levelstring + " Name  " + name[i].namepl[j]._namePlayer + "  Level  " + name[i].namepl[j].level[name[i].namepl[j].level.Count - 1].Level);
                           
                            write.NumberLevelPlay = name[i].namepl[j].level[name[i].namepl[j].level.Count - 1].NumberLevelPlay++;
                            Debug.Log("5");
                        }
                    }
                }
            }

        }
        write.NumberLevelComplete = 1;
        write.Level = GameSave.PlayerLevel;
        RestClient.Put(Database + "/" + (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                            DateTime.Now.Day.ToString()) + "/" + PlayerPrefs.GetInt("name", _CreateName).ToString() +"Name" + "/" + (write.Level.ToString() + "level") + "/" + ".json", write);
        RestClient.Get(Database + /*"/" + "SnowRace202316" + */ "/" + ".json").Then(response =>
        {
            string json = JsonUtility.ToJson(response.Text, true);
            File.WriteAllText(Application.dataPath + "/text.json", response.Text);

        });

    }

    [Button]

    public void WriteFireBaseLoseLevel()
    {
        // Lose

        Debug.Log("lose");
        if (name.Count == 0)
        {
            write.NumberLevelPlay = 1;

            Debug.Log("1");
        }
        for (int i = 0; i < name.Count; i++)
        {

            if (name[i].levelstring == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
            {

                foreach (var namePlayer in name[i].namepl)
                {
                    if (namePlayer == null)
                    {
                        write.NumberLevelPlay = 1;

                        Debug.Log("test2");
                    }
                    foreach (var level in namePlayer.level)
                    {
                        if (GameSave.PlayerLevel != level.Level)
                        {
                            write.NumberLevelPlay = 1;

                            Debug.Log("test1");
                        }
                        else
                        {
                            Debug.Log("test");
                            check = true;

                        }
                    }

                }

            }
        }
        if (check == true)
        {
            for (int i = 0; i < name.Count; i++)
            {
                if (name[i].levelstring == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                {
                    for (int j = 0; j < name[i].namepl.Count; j++)
                    {
                        if (name[i].namepl[j]._namePlayer == PlayerPrefs.GetInt("name", _CreateName).ToString() + "Name")
                        {
                            Debug.Log("Day: " + name[i].levelstring + " Name  " + name[i].namepl[j]._namePlayer + "  Level  " + name[i].namepl[j].level[name[i].namepl[j].level.Count - 1].Level);
                            Debug.Log(name[i].namepl[j].level[name[i].namepl[j].level.Count - 1].NumberLevelPlay);
                            write.NumberLevelPlay = name[i].namepl[j].level[name[i].namepl[j].level.Count - 1].NumberLevelPlay++;

                        }
                    }
                }
            }

        }
        write.Level = GameSave.PlayerLevel;
        write.NumberLevelComplete = 0;
        RestClient.Put(Database + "/" + (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                            DateTime.Now.Day.ToString()) + "/" + PlayerPrefs.GetInt("name", _CreateName)+"Name"  + "/" + (write.Level.ToString() + "level") + "/" + ".json", write);
        RestClient.Get(Database + /*"/" + "SnowRace202316" + */ "/" + ".json").Then(response =>
        {
            string json = JsonUtility.ToJson(response.Text, true);
            File.WriteAllText(Application.dataPath + "/text.json", response.Text);

        });

    }
    [Button]
    public void ReadData()
    {
        Database = "https://snow-race-io-default-rtdb.asia-southeast1.firebasedatabase.app/";
        RestClient.Get(Database + /*"/" + "SnowRace202316" + */ "/" + ".json").Then(response =>
        {
            string json = JsonUtility.ToJson(response.Text, true);
            File.WriteAllText(Application.dataPath + "/text.json", response.Text);
            Debug.Log("start");

        });

    }

    [Button]
    public void JsonParseList()
    {
        Database = "https://snow-race-io-default-rtdb.asia-southeast1.firebasedatabase.app/";
        JObject jsonObject = JObject.Parse(TextJson.text);
        RestClient.Get(Database + /*"/" + "SnowRace202316" + */ "/" + ".json").Then(response =>
        {
            string json = JsonUtility.ToJson(response.Text, true);
            File.WriteAllText(Application.dataPath + "/text.json", response.Text);
            Debug.Log("start");

        });

        foreach (JProperty prop in jsonObject.Properties())
        {
            object value = prop.Value;
            string key = prop.Name;


            LevelData1 lv = new LevelData1();
            name.Add(lv);
            lv.levelstring = key.ToString();
            string _value = value.ToString();
            JObject jsonLevel2 = JObject.Parse(_value);
            foreach (JProperty NamePlayer in jsonLevel2.Properties())
            {
                object _NamePlayer = NamePlayer.Value;
                string Name = NamePlayer.Name;

                NamePl name1 = new NamePl();
                lv.namepl.Add(name1);
                name1._namePlayer = Name;
                string _value2 = _NamePlayer.ToString();
                JObject jsonLevel = JObject.Parse(_value2);
                foreach (JProperty propLevel in jsonLevel.Properties())
                {

                    string keyLevel = propLevel.Name;
                    object valueLevel = propLevel.Value;
                    LevelData data = JsonConvert.DeserializeObject<LevelData>(valueLevel.ToString());
                    name1.level.Add(data);
                }
            }

        }
        if (PlayerPrefs.HasKey("name") == false)
        {
            if (name.Count == 0)
            {
                _CreateName = 1;
            }
            else
            {
                _CreateName = UnityEngine.Random.Range(2, 100);
            }
            PlayerPrefs.SetInt("name", _CreateName);
        }
        _CreateName = PlayerPrefs.GetInt("name", _CreateName);


    }

}

