﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public string[] scenes;
    public AudioClip[] sceneMusic;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(scenes[sceneNum]);
        MusicManager.Instance.ChangeMusic(sceneMusic[sceneNum]);
    }
}
