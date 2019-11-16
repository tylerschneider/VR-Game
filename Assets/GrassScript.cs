using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour
{
    public float minDist = 1.1f;
    public float maxDist = 1f;
    public float rotateAmount;
    public float maxRot = 60f;

    void Update()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 playerDist = new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z);
        if (playerDist.magnitude < minDist)
        {

            transform.LookAt(playerPos);
            Quaternion rot2 = transform.rotation;
            rotateAmount = ((minDist - playerDist.magnitude) / (minDist - maxDist)) * maxRot;
            transform.eulerAngles = new Vector3(-rotateAmount, rot2.eulerAngles.y, rot2.eulerAngles.z);

            Transform grass = transform.Find("Model");
            grass.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            Vector3 rot = grass.transform.localEulerAngles;
            grass.transform.localEulerAngles = Vector3.Scale(rot, new Vector3(0, 1, 0));


            /*float angle = Mathf.Atan2(transform.position.x - Player.Instance.transform.position.x, transform.position.z - Player.Instance.transform.position.z) * Mathf.Rad2Deg;
            //Debug.Log(angle + " Sin: " + Mathf.Sin(angle) * minDist + " Cos: " + Mathf.Cos(angle) * minDist + " Tan: " + Mathf.Tan(angle) * minDist);

            transform.rotation = Quaternion.Euler(-angle, 0, -angle);
            Debug.Log(transform.rotation.x + " " + transform.rotation.z);*/
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.Find("Model").transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
