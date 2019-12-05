using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeverScript : MonoBehaviour
{
    public bool lockLever = true;
    // Start is called before the first frame update
    void Update()
    {
        if(transform.rotation.x < -0.5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if(lockLever == true)
            {
                GetComponent<GrabbableObject>().grabbedBy.ForceRelease(GetComponent<GrabbableObject>());
                Destroy(GetComponent<GrabbableObject>());
            }
        }
    }
}
