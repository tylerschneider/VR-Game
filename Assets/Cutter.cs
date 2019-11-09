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
    public Rigidbody swordRigidbody;
    public GameObject hitParticles;

    void FixedUpdate()
    {
       if(cutting == true)
        {
            cutTimePassed += Time.deltaTime;
        }

        if(GetComponentInParent<GrabbableObject>().isGrabbed)
        {
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(swordRigidbody.velocity);
        GameObject particles = Instantiate(hitParticles);
        particles.transform.position = transform.position;
        

        if(other.tag == "Cuttable")
        {
            cutting = true;
            startPoint = transform.position;
            objectBeingCut = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == objectBeingCut && cutTimePassed < cutTime)
        {
            endPoint = transform.position;

            if (startPoint != null && endPoint != null)
            {
                other.GetComponent<Cuttable>().CutObject(startPoint, endPoint);
                cutting = false;
                cutTimePassed = 0f;
            }
        }
    }
}
