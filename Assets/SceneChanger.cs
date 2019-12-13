using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public Object[] scenes;
    public AudioClip[] sceneMusic;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(scenes[sceneNum].name);
        MusicManager.Instance.ChangeMusic(sceneMusic[sceneNum]);
    }
}
