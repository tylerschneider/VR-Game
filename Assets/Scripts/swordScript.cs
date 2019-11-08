using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    public Transform swordSlot;

    void Update()
    {
        if(GetComponent<GrabbableObject>().isGrabbed)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
