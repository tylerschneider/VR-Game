using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject chestTop;
    public GameObject keyLock;
    public GameObject chestRewards;

    public bool locked = true;

    public void Unlock()
    {
        chestTop.GetComponent<GrabbableObject>().enabled = true;
        chestTop.GetComponent<Rigidbody>().isKinematic = false;

        if (chestRewards != null)
        {
            chestRewards.SetActive(true);
        }

        locked = false;
    }
}
