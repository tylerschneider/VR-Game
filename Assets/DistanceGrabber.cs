using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceGrabber : MonoBehaviour
{
    public float grabRange = 1.5f;

    public float grabThickness = 0.1f;

    private float m_prevFlex;

    public GameObject grabPoint;

    private RaycastHit hit;

    private GrabbableObject go;

    private float grabAlpha;

    private void Awake()
    {
        grabAlpha = grabPoint.GetComponent<Renderer>().material.GetFloat("_Alpha");
    }

    void Update()
    {
        //get the trigger values for grabbing
        float prevFlex = m_prevFlex;
        m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, GetComponent<Grabber>().m_controller);


        //check raycast for collision
        if(Physics.SphereCast(transform.position, grabThickness, transform.forward, out hit, grabRange))
        {
            //check for and get the grabbable object
            if(hit.collider.GetComponent<GrabbableObject>())
            {
                go = hit.collider.GetComponent<GrabbableObject>();
            }
            else if (hit.collider.GetComponentInParent<GrabbableObject>())
            {
                go = hit.collider.GetComponentInParent<GrabbableObject>();
            }
            else
            {
                go = null;
            }

            //if the object is not grabbable or is grabbable but not grabbed, move the grab marker
            if (go == null || go && !go.isGrabbed)
            {
                grabPoint.GetComponent<Renderer>().material.SetFloat("_Alpha", grabAlpha);
                grabPoint.transform.position = hit.point;
            }
            else
            {
                //if the object is grabbed, hide the marker
                grabPoint.GetComponent<Renderer>().material.SetFloat("_Alpha", 0);
            }

            //make sure object is grabbable, allows for distance grab, and it not currently grabbed
            if(go && go.allowDistanceGrab && !go.isGrabbed)
            {
                //make sure the right collider is being hit
                foreach (Collider col in go.grabPoints)
                {
                    if(hit.collider == col)
                    {
                        grabPoint.GetComponent<Renderer>().material.SetFloat("_Alpha", grabAlpha * 2.5f);

                        //if the triggers are pulled, put the object in hand and grab it
                        if ((m_prevFlex >= GetComponent<Grabber>().grabBegin) && (prevFlex < GetComponent<Grabber>().grabBegin))
                        {
                            hit.transform.position = transform.position;
                            GetComponent<Grabber>().m_grabbedObj = go;
                            go.GrabBegin(GetComponent<Grabber>(), hit.collider);
                        }
                    }
                }
            }
        }
        else
        {
            grabPoint.transform.position = transform.position + transform.forward * grabRange;
        }
    }
}
