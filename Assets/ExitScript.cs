using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.Any))
        {
            Application.Quit();
        }
    }
}
