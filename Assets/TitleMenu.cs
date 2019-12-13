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
            GetComponent<TextMeshPro>().text = "A: Load Game \n B: New Game";
            savePresent = true;
        }
        else
        {
            GetComponent<TextMeshPro>().text = "A: New Game";
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
                SceneChanger.Instance.LoadScene(0);
            }
        }
        else if (savePresent == true && confirm == false)
        {
            //load game
            if (OVRInput.GetUp(OVRInput.RawButton.A))
            {
                SceneChanger.Instance.LoadScene(0);
                GameData.Instance.LoadGame();
            }
            //new game
            else if (OVRInput.GetUp(OVRInput.RawButton.B))
            {
                GetComponent<TextMeshPro>().text = "Start a new game? \n A: Yes   B: No";
                confirm = true;
            }
        }
        else if(savePresent == true && confirm == true)
        {
            if(OVRInput.GetUp(OVRInput.RawButton.A))
            {
                SceneChanger.Instance.LoadScene(0);
            }
            else if(OVRInput.GetUp(OVRInput.RawButton.B))
            {
                GetComponent<TextMeshPro>().text = "A: Load Game \n B: New Game";
                confirm = false;
            }
        }
    }
}
