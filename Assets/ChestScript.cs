using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject chestTop;
    public GameObject keyLock;

    public bool chestLocked = true;

    private void OnTriggerEnter(Collider other)
    {
        if(chestLocked && other.tag == "Key")
        {
            chestTop.GetComponent<GrabbableObject>().enabled = false;
            if(other.GetComponent<GrabbableObject>().isGrabbed)
            {
                other.GetComponent<GrabbableObject>().grabbedBy.ForceRelease(other.GetComponent<GrabbableObject>());
                other.GetComponent<GrabbableObject>().enabled = false;
            }

            other.transform.parent = keyLock.transform;
            keyLock.GetComponent<Rigidbody>().useGravity = true;
            keyLock.GetComponent<Rigidbody>().isKinematic = false;
            keyLock.transform.position += keyLock.transform.forward * .1f;
            keyLock.transform.parent = null;

            chestLocked = false;
        }
    }
}
