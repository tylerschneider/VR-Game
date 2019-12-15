using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetUp(OVRInput.RawButton.A) || OVRInput.GetUp(OVRInput.RawButton.B) || OVRInput.GetUp(OVRInput.RawButton.X) || OVRInput.GetUp(OVRInput.RawButton.Y) || OVRInput.GetUp(OVRInput.RawButton.Start))
        {
            AutoSave.Instance.SaveGame();
            SceneChanger.Instance.LoadScene(0);
        }
    }
}
