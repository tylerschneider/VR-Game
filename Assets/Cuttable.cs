using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Cuttable : MonoBehaviour
{
    private GameObject cutPlane;

    public bool destroyObject;

    //public GameObject topConnection;
    //public GameObject bottomConnection;

    public float minSize;

    private void FixedUpdate()
    {
        if (GetComponent<ConfigurableJoint>())
        {
            if(GetComponent<ConfigurableJoint>().connectedBody == null)
            {
                Destroy(GetComponent<ConfigurableJoint>());
            }
        }
    }

    public void CutObject(Vector3 startPoint, Vector3 endPoint)
    {
        if(!destroyObject)
        {
            cutPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            cutPlane.transform.position = startPoint;
            Vector3 rotLocation = endPoint - startPoint;
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

                if (tmc.bounds.size.x < minSize || tmc.bounds.size.y < minSize || tmc.bounds.size.z < minSize || bmc.bounds.size.x < minSize || bmc.bounds.size.y < minSize || bmc.bounds.size.z < minSize)
                {
                    Destroy(topSlice);
                    Destroy(bottomSlice);
                    Debug.Log("not big enough");
                }

                else
                {
                    trb.AddExplosionForce(50, endPoint + (startPoint + endPoint) / 2, 10);
                    brb.AddExplosionForce(50, endPoint + (startPoint + endPoint) / 2, 10);

                    /*if (cutTimes < maxCutTimes)
                    {
                        Cuttable tc = topSlice.AddComponent<Cuttable>();
                        Cuttable bc = bottomSlice.AddComponent<Cuttable>();
                        tc.cutTimes = cutTimes++;
                        bc.cutTimes = cutTimes++;
                        tc.maxCutTimes = maxCutTimes;
                        bc.maxCutTimes = maxCutTimes;
                        tc.minSize = minSize;
                        bc.minSize = minSize;
                    }*/


                    Destroy(gameObject);
                }

                Destroy(cutPlane);
            }
        }
        else
        {
            Destroy(gameObject);
        }


    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial)
    {
        // slice the provided object using the transforms of this object
        return obj.Slice(cutPlane.transform.position, cutPlane.transform.up, crossSectionMaterial);
    }

}
