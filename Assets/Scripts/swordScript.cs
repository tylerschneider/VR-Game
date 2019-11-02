using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    public Transform swordSlot;

    void Update()
    {
        if(GetComponent<OVRGrabbable>().isGrabbed)
        {
            transform.position = swordSlot.position;
            transform.rotation = swordSlot.rotation;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
