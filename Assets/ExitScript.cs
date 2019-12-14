using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.RawButton.A) || OVRInput.Get(OVRInput.RawButton.B) || OVRInput.Get(OVRInput.RawButton.X) || OVRInput.Get(OVRInput.RawButton.Y) || OVRInput.Get(OVRInput.RawButton.Start))
        {
            Application.Quit();
        }
    }
}
