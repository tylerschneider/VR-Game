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
    
    private void Awake()
    {
        Instance = this;
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
}
