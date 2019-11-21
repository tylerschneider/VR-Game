using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float cutTime = 0.3f;
    public float cutTimePassed = 0f;
    public bool cutting = false;
    private GameObject objectBeingCut;
    public GameObject hitParticles;
    public float sparkForce = 0.7f;

    void FixedUpdate()
    {

        //if cutting, update time
       if(cutting == true)
        {
            cutTimePassed += Time.deltaTime;
            if (cutTimePassed > cutTime)
            {
                cutting = false;
                cutTimePassed = 0;
                objectBeingCut = null;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.relativeVelocity.magnitude > sparkForce)
        {
            GameObject particles = Instantiate(hitParticles, other.GetContact(0).point, Quaternion.identity);
        }

        //if cuttable, set to cutting, get collision point and object, and set to trigger
        if(other.gameObject.tag == "Cuttable")
        {
            GameObject particles = Instantiate(hitParticles, other.GetContact(0).point, Quaternion.identity);
            Debug.Log("cut start");
            cutting = true;
            startPoint = other.GetContact(0).point;
            objectBeingCut = other.gameObject;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject == objectBeingCut && cutting)
        {
            //set the end point
            endPoint = other.GetContact(other.contactCount - 1).point;
        }

    }

    private void OnCollisionExit(Collision other)
    {
        //if the object exit is the one being cut
        if(other.gameObject == objectBeingCut && cutting)
        {
            //cut the object, set cutting to false, and time passed to 0
            Debug.Log("cut end");
            objectBeingCut.GetComponent<Cuttable>().CutObject(startPoint, endPoint);
            objectBeingCut = null;
        }
    }
}
