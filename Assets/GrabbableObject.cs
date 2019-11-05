using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public GameObject objectSlot;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == objectSlot && GetComponent<OVRGrabbable>().isGrabbed == true)
        {
            objectSlot.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == objectSlot && objectSlot.GetComponent<MeshRenderer>().enabled == true && GetComponent<OVRGrabbable>().isGrabbed == false)
        {
            objectSlot.GetComponent<MeshRenderer>().enabled = false;
            transform.position = objectSlot.transform.position;
            transform.rotation = objectSlot.transform.rotation;
            transform.parent = objectSlot.transform.parent;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == objectSlot && GetComponent<OVRGrabbable>().isGrabbed == true)
        {
            objectSlot.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if(GetComponent<OVRGrabbable>().isGrabbed == false && GetComponent<Rigidbody>().isKinematic == true)
        {
            if(objectSlot == null || transform.parent != objectSlot.transform.parent)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = true;
            }

        }
    }

}
