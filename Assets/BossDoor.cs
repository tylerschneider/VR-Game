using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("_Offset", GetComponent<Renderer>().material.GetFloat("_Offset") + 0.0005f);
    }
}
