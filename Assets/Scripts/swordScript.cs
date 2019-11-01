using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class swordScript : MonoBehaviour
{
    public Transform swordSlot;
    public GameObject objectToSlice;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public GameObject cutPlane;

    public GameObject col;
    public float cancelCutSeconds;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag == "Cuttable")
        {
            if(col == null)
            {
                Debug.Log(col);
                col = collision.gameObject;
                startPoint = collision.GetContact(0).point;
                Debug.Log(col.gameObject);
                objectToSlice = collision.gameObject.transform.parent.gameObject;
                Debug.Log(objectToSlice);

                StartCoroutine(stopCut());
            }
            else if(collision.gameObject != col)
            {
                endPoint = collision.GetContact(collision.contactCount - 1).point;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Cuttable")
        {
            if (collision.gameObject != col && collision.gameObject.transform.parent.gameObject == objectToSlice)
            {
                    Debug.Log(startPoint);
                    Debug.Log(endPoint);

                    cutPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    cutPlane.transform.position = startPoint;
                    Vector3 rotLocation = endPoint - startPoint;
                    Quaternion rotation = Quaternion.LookRotation(rotLocation);
                    cutPlane.transform.rotation = rotation;


                SlicedHull newSlice = SliceObject(objectToSlice, null);

                    if (newSlice != null)
                    {
                        GameObject topSlice = newSlice.CreateLowerHull(objectToSlice, null);
                        GameObject bottomSlice = newSlice.CreateUpperHull(objectToSlice, null);
                        Destroy(objectToSlice);
                        Destroy(cutPlane);

                        Rigidbody trb = topSlice.AddComponent<Rigidbody>();
                        topSlice.AddComponent<MeshCollider>().convex = true;
                        Rigidbody brb = bottomSlice.AddComponent<Rigidbody>();
                        bottomSlice.AddComponent<MeshCollider>().convex = true;

                    trb.AddExplosionForce(50, endPoint + (startPoint + endPoint) / 2, 10);
                    brb.AddExplosionForce(50, endPoint + (startPoint + endPoint) / 2, 10);

                    col = null;
                    }

            }
        }
    }

    private IEnumerator stopCut()
    {
        yield return new WaitForSeconds(cancelCutSeconds);
        col = null;
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        return obj.Slice(cutPlane.transform.position, cutPlane.transform.up, crossSectionMaterial);
    }

    void Update()
    {
        if(GetComponent<OVRGrabbable>().isGrabbed)
        {
            transform.position = swordSlot.position;
            transform.rotation = swordSlot.rotation;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
