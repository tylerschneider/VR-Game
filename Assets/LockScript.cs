using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    public ChestScript lockedChest;
    public GameObject lockedDoor;

    private void Awake()
    {
        lockedChest = transform.parent.GetComponent<ChestScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Key" && other.GetComponent<GrabbableObject>().isGrabbed)
        {
            if(lockedChest && lockedChest.locked == true)
            {
                RemoveLock(other);
                lockedChest.Unlock();
            }
            else if(lockedDoor)
            {
                RemoveLock(other);
                Destroy(lockedDoor);
            }
        }
    }

    public void RemoveLock(Collider other)
    {
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
