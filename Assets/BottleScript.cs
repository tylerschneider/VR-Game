using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    public float breakVelocity;
    public GameObject brokenBottle;
    // Update is called once per frame
    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.relativeVelocity.magnitude);
        if(other.gameObject.tag != "Player" && other.relativeVelocity.magnitude > breakVelocity)
        {
            GameObject go = Instantiate(brokenBottle, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            go.GetComponentInChildren<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;

            Destroy(gameObject);
        }
    }
}
