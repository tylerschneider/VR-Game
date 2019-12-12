using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public Transform playerController;

    public bool gotSword = false;
    public Transform sword;
    public Transform swordSlot;

    public bool gotBag = false;
    public Transform bag;
    public Transform bagSlot;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(OVRInput.GetUp(OVRInput.RawButton.Start))
        {
            if(gotSword == true && sword.GetComponent<GrabbableObject>().m_grabbedBy == null)
            {
                sword.transform.position = swordSlot.position;
                sword.transform.rotation = swordSlot.rotation;
                sword.transform.parent = playerController;
                sword.GetComponent<Rigidbody>().isKinematic = true;
            }

            if (gotBag == true && bag.GetComponent<GrabbableObject>().m_grabbedBy == null)
            {
                bag.transform.position = bagSlot.position;
                bag.transform.rotation = bagSlot.rotation;
                bag.transform.parent = playerController;
                bag.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
