using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    private bool savePresent = false;
    private bool confirm = false;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            GetComponent<TextMeshPro>().text = "A: Load Game \nB: New Game \nStart: Quit";
            savePresent = true;
            GameData.Instance.LoadOptions();
        }
        else
        {
            GetComponent<TextMeshPro>().text = "A: New Game \nStart: Quit";
            savePresent = false;
        }
    }

    private void Update()
    {
        if(savePresent == false)
        {
            //new game
            if (OVRInput.GetUp(OVRInput.RawButton.A))
            {
                ClearSaveData();
                SceneChanger.Instance.LoadScene(1);
            }
            //quit game
            if(OVRInput.GetUp(OVRInput.RawButton.Start))
            {
                Application.Quit();
            }
        }
        else if (savePresent == true && confirm == false)
        {
            //load game
            if (OVRInput.GetUp(OVRInput.RawButton.A))
            {
                SceneChanger.Instance.LoadScene(1);
                GameData.Instance.LoadGame();
            }
            //new game
            else if (OVRInput.GetUp(OVRInput.RawButton.B))
            {
                GetComponent<TextMeshPro>().text = "Start a new game? \nA: Yes   B: No";
                confirm = true;
            }
            //quit game
            if (OVRInput.GetUp(OVRInput.RawButton.Start))
            {
                Application.Quit();
            }
        }
        else if(savePresent == true && confirm == true)
        {
            if(OVRInput.GetUp(OVRInput.RawButton.A))
            {
                ClearSaveData();
                SceneChanger.Instance.LoadScene(1);
            }
            else if(OVRInput.GetUp(OVRInput.RawButton.B))
            {
                GetComponent<TextMeshPro>().text = "A: Load Game \nB: New Game \nStart: Quit Game";
                confirm = false;
            }
        }
    }

    private void ClearSaveData()
    {
        GameData.Instance.Clear();
    }
}
