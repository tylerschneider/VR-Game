using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyScript : MonoBehaviour
{
    void Update()
    {
        if(!GetComponent<ParticleSystem>())
        {
            Destroy(this.gameObject);
        }
    }
}
