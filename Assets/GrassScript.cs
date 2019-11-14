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
        float playerDist = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < minDist)
        {

            //transform.LookAt(Player.Instance.transform);
            Quaternion rot = transform.rotation;

            rotateAmount = ((minDist - playerDist) / (minDist - maxDist)) * maxRot;

            //transform.rotation = Quaternion.Euler(rot.x, rot.y, rotateAmount);

            float angle = Mathf.Atan2(transform.position.x - Player.Instance.transform.position.x, transform.position.z - Player.Instance.transform.position.z) * Mathf.Rad2Deg;
            //Debug.Log(angle + " Sin: " + Mathf.Sin(angle) * minDist + " Cos: " + Mathf.Cos(angle) * minDist + " Tan: " + Mathf.Tan(angle) * minDist);

            transform.rotation = Quaternion.Euler(-angle, 0, -angle);
            Debug.Log(transform.rotation.x + " " + transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
