using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    public bool pressed = false;
    public bool down = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Grabber>())
        {
            if(!GameData.Instance.muted)
            {
                pressed = true;
                GameData.Instance.muted = true;
            }
            else
            {
                pressed = false;
                GameData.Instance.muted = false;
            }
        }
    }

    private void Update()
    {
        if(pressed == true && down == false)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, 0.5f, 1));
            down = true;
        }
        else if(pressed == false && down == true)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, 2f, 1));
            down = false;
        }
    }
}
