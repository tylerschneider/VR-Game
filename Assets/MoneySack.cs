using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneySack : MonoBehaviour
{
    public static MoneySack Instance;
    public int money;

    private void Awake()
    {
        Instance = this;
    }
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
        }
    }

    private void Update()
    {
        if (GetComponent<GrabbableObject>().m_grabbedBy != null && !ItemManager.Instance.gotBag)
        {
            ItemManager.Instance.gotBag = true;
        }

        GetComponentInChildren<TextMeshPro>().SetText("$" + money.ToString());
    }
}
