using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Cuttable : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    private GameObject cutPlane;
    public string cutting;

    public Joint topJoint;
    public Joint bottomJoint;

    public float cancelCutSeconds;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if(cutting == "")
            {
                cutting = "started";

                startPoint = collision.GetContact(0).point;

                StartCoroutine(stopCut());
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            Debug.Log(collision.gameObject);
            if (cutting == "started")
            {
                if (GetComponent<Collider>().bounds.Contains(collision.transform.Find("BladeTip").transform.position))
                {
                    cutting = "canceled";
                    Debug.Log("didn't fully cut");
                    StopCoroutine(stopCut());
                }

                endPoint = collision.GetContact(collision.contactCount - 1).point;
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if(cutting == "started")
            {
                cutPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                cutPlane.transform.position = startPoint;
                Vector3 rotLocation = endPoint - startPoint;
                Quaternion rotation = Quaternion.LookRotation(rotLocation);
                cutPlane.transform.rotation = rotation;


                SlicedHull newSlice = SliceObject(gameObject, null);

                if (newSlice != null)
                {
                    GameObject topSlice = newSlice.CreateLowerHull(gameObject, gameObject.GetComponent<Renderer>().material);
                    GameObject bottomSlice = newSlice.CreateUpperHull(gameObject, gameObject.GetComponent<Renderer>().material);
                    Destroy(cutPlane);

                    Rigidbody trb = topSlice.AddComponent<Rigidbody>();
                    topSlice.AddComponent<MeshCollider>().convex = true;
                    if(topJoint != null)
                    {
                        topSlice.AddComponent<Joint>().connectedBody = topJoint.connectedBody;
                    }
                    Rigidbody brb = bottomSlice.AddComponent<Rigidbody>();
                    bottomSlice.AddComponent<MeshCollider>().convex = true;
                    if (bottomJoint != null)
                    {
                        bottomSlice.AddComponent<Joint>().connectedBody = bottomJoint.connectedBody;
                    }

                    trb.AddExplosionForce(50, endPoint + (startPoint + endPoint) / 2, 10);
                    brb.AddExplosionForce(50, endPoint + (startPoint + endPoint) / 2, 10);
                    //trb.AddForce(collision.relativeVelocity);

                    topSlice.AddComponent<Cuttable>();
                    bottomSlice.AddComponent<Cuttable>();

                    Destroy(gameObject);
                }
            }
            else
            {
                cutting = "";
            }
        }
    }

    private IEnumerator stopCut()
    {
        yield return new WaitForSeconds(cancelCutSeconds);
        cutting = "canceled";
        Debug.Log("Timeout");
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial)
    {
        // slice the provided object using the transforms of this object
        return obj.Slice(cutPlane.transform.position, cutPlane.transform.up, crossSectionMaterial);
    }

}
