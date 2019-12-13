﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AutoSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SaveData());
    }

    IEnumerator SaveData()
    {
        yield return new WaitForSeconds(10f);

        Save save = CreateSave();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        if(save != null)
        {
            Debug.Log("Game Saved");
        }

        StartCoroutine(SaveData());
    }

    private Save CreateSave()
    {
        Save save = new Save();

        if (GameData.Instance != null)
        {
            save.money = GameData.Instance.money;
            save.gotSword = GameData.Instance.gotSword;
            save.gotBag = GameData.Instance.gotBag;
        }

        return save;
    }
}
