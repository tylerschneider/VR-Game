﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if(transform.rotation.x < -0.5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
