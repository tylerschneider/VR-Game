using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    public ChestScript lockedChest;

    private void Awake()
    {
        lockedChest = transform.parent.GetComponent<ChestScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Key" && lockedChest.locked == true && other.GetComponent<GrabbableObject>().isGrabbed)
        {
            Debug.Log("Unlock");
            lockedChest.Unlock();

            other.GetComponent<GrabbableObject>().grabbedBy.ForceRelease(other.GetComponent<GrabbableObject>());
            Destroy(other.GetComponent<GrabbableObject>());
            Destroy(other.GetComponent<Rigidbody>());
            other.transform.parent = transform;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<MeshCollider>().convex = true;
            GetComponent<Rigidbody>().isKinematic = false;
            transform.position += transform.forward * .1f;
            transform.parent = null;
        }
    }
}
