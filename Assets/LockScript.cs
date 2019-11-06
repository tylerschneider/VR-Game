using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    public ChestScript lockedChest;

    private void Awake()
    {
        lockedChest = transform.parent.parent.GetComponent<ChestScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Key" && lockedChest.locked == true/* && gameObject.transform.rotation.z % 360 > 90*/)
        {
            lockedChest.Unlock();

            if (other.GetComponent<GrabbableObject>().isGrabbed)
            {
                other.GetComponent<GrabbableObject>().grabbedBy.ForceRelease(other.GetComponent<GrabbableObject>());
                other.GetComponent<GrabbableObject>().enabled = false;
            }

            other.transform.parent = transform;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            transform.position += transform.forward * .1f;
            transform.parent = null;
        }
    }
}
