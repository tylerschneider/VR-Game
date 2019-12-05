using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BandScript : MonoBehaviour
{
    public Transform bandAnchor;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = bandAnchor;
        transform.position = bandAnchor.position;
        transform.rotation = bandAnchor.rotation;
    }
}
