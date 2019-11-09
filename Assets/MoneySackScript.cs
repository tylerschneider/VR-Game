using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneySackScript : MonoBehaviour
{
    public int money;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Money")
        {
            if(other.GetComponent<GrabbableObject>().isGrabbed == true)
            {
                other.GetComponent<GrabbableObject>().grabbedBy.ForceRelease(other.GetComponent<GrabbableObject>());
            }

            Destroy(other.gameObject);
            money++;

            GetComponentInChildren<TextMeshPro>().SetText("$" + money.ToString());
        }
    }
}
