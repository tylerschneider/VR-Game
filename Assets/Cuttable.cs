using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Cuttable : MonoBehaviour
{
    public ContactPoint startPoint;
    public ContactPoint endPoint;
    public Vector3 vel;
    private GameObject cutPlane;
    public string cutting;

    //public GameObject topConnection;
    //public GameObject bottomConnection;

    public float cancelCutSeconds;
    public int maxCutTimes;
    public int cutTimes;
    public float minSize;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if(cutting == "")
            {
                cutting = "started";

                startPoint = collision.GetContact(0);

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
                endPoint = collision.GetContact(collision.contactCount - 1);
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if (cutting == "started")
            {
                cutPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                cutPlane.transform.position = startPoint.point;
                Vector3 rotLocation = endPoint.point - startPoint.point;
                Quaternion rotation = Quaternion.LookRotation(rotLocation);
                cutPlane.transform.rotation = rotation;


                SlicedHull newSlice = SliceObject(gameObject, null);

                if (newSlice != null)
                {
                    GameObject topSlice = newSlice.CreateUpperHull(gameObject, gameObject.GetComponent<Renderer>().material);
                    GameObject bottomSlice = newSlice.CreateLowerHull(gameObject, gameObject.GetComponent<Renderer>().material);

                    Rigidbody trb = topSlice.AddComponent<Rigidbody>();
                    MeshCollider tmc = topSlice.AddComponent<MeshCollider>();

                    Rigidbody brb = bottomSlice.AddComponent<Rigidbody>();
                    MeshCollider bmc = bottomSlice.AddComponent<MeshCollider>();

                    tmc.convex = true;
                    bmc.convex = true;

                    topSlice.layer = 12;
                    bottomSlice.layer = 12;

                    Debug.Log(tmc.bounds);
                    Debug.Log(bmc.bounds);

                    if (tmc.bounds.size.x < minSize || tmc.bounds.size.y < minSize || tmc.bounds.size.z < minSize || bmc.bounds.size.x < minSize || bmc.bounds.size.y < minSize || bmc.bounds.size.z < minSize)
                    {
                        Destroy(topSlice);
                        Destroy(bottomSlice);
                        Debug.Log("not big enough");
                    }

                    else
                    {
                        trb.AddExplosionForce(50, endPoint.point + (startPoint.point + endPoint.point) / 2, 10);
                        brb.AddExplosionForce(50, endPoint.point + (startPoint.point + endPoint.point) / 2, 10);

                        if(cutTimes < maxCutTimes)
                        {
                            topSlice.AddComponent<Cuttable>().cutTimes = cutTimes++;
                            bottomSlice.AddComponent<Cuttable>().cutTimes = cutTimes++;
                        }


                        Destroy(gameObject);
                    }

                    Destroy(cutPlane);

                }
            }
            else
            {
                cutting = "";
            }
        }
    }

    private void FixedUpdate()
    {
        if(cutting == "started")
        {
            Debug.Log(GameObject.Find("BladeTip"));
            if (GetComponent<Collider>().bounds.Contains(GameObject.Find("BladeTip").transform.position))
            {
                cutting = "canceled";
                Debug.Log("didn't fully cut");
                StopCoroutine(stopCut());
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
