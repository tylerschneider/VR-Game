using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public Transform playerController;

    public Transform sword;
    public Transform swordSlot;

    public Transform bag;
    public Transform bagSlot;

    private void Awake()
    {
        Instance = this;
        ItemManager.Instance.ReturnItems();
    }

    private void Update()
    {
        if(OVRInput.GetUp(OVRInput.RawButton.Start))
        {
            ReturnItems();
        }
    }

    public void ReturnItems()
    {
        if (GameData.Instance.gotSword == true && sword.GetComponent<GrabbableObject>().m_grabbedBy == null)
        {
            sword.transform.position = swordSlot.position;
            sword.transform.rotation = swordSlot.rotation;
            sword.transform.parent = playerController;
            sword.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (GameData.Instance.gotBag == true && bag.GetComponent<GrabbableObject>().m_grabbedBy == null)
        {
            bag.transform.position = bagSlot.position;
            bag.transform.rotation = bagSlot.rotation;
            bag.transform.parent = playerController;
            bag.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
