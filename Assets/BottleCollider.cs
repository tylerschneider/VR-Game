using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCollider : MonoBehaviour
{
    public bool hitting = false;

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            hitting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hitting = false;
        }
    }
}
