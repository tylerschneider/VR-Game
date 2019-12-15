using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int money = 0;
    public bool gotSword = false;
    public bool gotBag = false;

    [Space(10)]
    [Header("Options")]

    public bool muted = false;
    public float volume = 0.5f;

    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            money = save.money;
            gotSword = save.gotSword;
            gotBag = save.gotBag;

            Debug.Log("Game Loaded");
        }
    }

    public void LoadOptions()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            muted = save.muted;
            volume = save.volume;

            Debug.Log("Options Loaded");
        }
    }

    private void Update()
    {
        if(!muted)
        {
            if (volume > 1f)
            {
                volume = 1f;
            }
            else if (volume < 0f)
            {
                volume = 0f;
            }
        }
        else
        {
            volume = 0f;
        }

        AudioListener.volume = volume;
    }

    public void Clear()
    {
        money = 0;
        gotSword = false;
        gotBag = false;
        volume = 50;
        muted = false;
    }
}
