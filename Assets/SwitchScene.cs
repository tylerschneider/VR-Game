using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public Object startScene;
    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.RawButton.A))
        {
            SceneManager.LoadScene(startScene.name);
        }
        else if (OVRInput.Get(OVRInput.RawButton.X))
        {
            SceneManager.LoadScene(startScene.name);
            Load.Instance.LoadGame();
        }
    }
}
