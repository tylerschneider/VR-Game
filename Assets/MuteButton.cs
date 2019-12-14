using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Grabber>())
        {
            if(AudioListener.volume != 0f)
            {
                AudioListener.volume = 0f;
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, 0.5f, 1));
            }
            else
            {
                AudioListener.volume = 1f;
                transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, 2f, 1));
            }

        }
    }
}
