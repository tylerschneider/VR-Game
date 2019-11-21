using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    public float breakVelocity;
    public GameObject brokenBottle;

    private GameObject newBottle;
    // Update is called once per frame
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player" && other.relativeVelocity.magnitude > breakVelocity && newBottle == null)
        {
            newBottle = Instantiate(brokenBottle, transform.position, transform.rotation);
            newBottle.GetComponentInChildren<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            newBottle.GetComponentInChildren<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
            newBottle.GetComponentInChildren<Rigidbody>().AddExplosionForce(1000, other.GetContact(0).point, 10);

            if(!gameObject.transform.Find("Cork"))
            {
                Destroy(newBottle.transform.Find("Cork").gameObject);
            }

            newBottle.GetComponent<AudioSource>().Play();
            newBottle.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.3f);

            Destroy(gameObject);
        }
    }
}
