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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter");
        if (collision.gameObject.tag == "Cuttable")
        {
            Debug.Log("cut start");
            startPoint = collision.GetContact(0).point;
            objectToSlice = collision.gameObject;
            endPoint = collision.GetContact(collision.contactCount - 1).point;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == objectToSlice)
        {
            Debug.Log("cut end");
            Debug.Log(endPoint);

            cutPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            cutPlane.transform.position = startPoint;
            cutPlane.transform.rotation = Quaternion.LookRotation(endPoint, Vector3.up);
            

            SlicedHull newSlice = SliceObject(collision.gameObject, null);

            if(newSlice != null)
            {
                GameObject topSlice = newSlice.CreateLowerHull(collision.gameObject, null);
                GameObject bottomSlice = newSlice.CreateUpperHull(collision.gameObject, null);
                Destroy(collision.gameObject);

                topSlice.AddComponent<Rigidbody>();
                topSlice.AddComponent<MeshCollider>().convex = true;
                bottomSlice.AddComponent<Rigidbody>();
                bottomSlice.AddComponent<MeshCollider>().convex = true;
            }


        }
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
