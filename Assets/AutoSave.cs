using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AutoSave : MonoBehaviour
{
    public static AutoSave Instance;
    public bool autoSave = true;
    // Start is called before the first frame update
    void Start()
    {
        if(!Instance)
        {
            Instance = this;
        }
        StartCoroutine(DoAutoSave());
    }

    public void SaveGame()
    {
        Save save = CreateSave();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        if (save != null)
        {
            Debug.Log("Game Saved");
        }
    }

    IEnumerator DoAutoSave()
    {
        yield return new WaitForSeconds(10f);

        if(autoSave == true)
        {
            SaveGame();
        }

        StartCoroutine(DoAutoSave());
    }

    private Save CreateSave()
    {
        Save save = new Save();

        if (GameData.Instance != null)
        {
            save.money = GameData.Instance.money;
            save.gotSword = GameData.Instance.gotSword;
            save.gotBag = GameData.Instance.gotBag;
            save.volume = GameData.Instance.volume;
        }

        return save;
    }
}
