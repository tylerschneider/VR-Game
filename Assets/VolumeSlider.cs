using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    public Transform slider;
    public Transform minVolume;
    public Transform maxVolume;
    public TextMeshPro volumeText;

    private float lastVolume;

    void Update()
    {
        slider.localPosition = new Vector3(0, slider.localPosition.y, 0);

        if(GameData.Instance.muted == true)
        {
            slider.GetComponent<GrabbableObject>().enabled = false;
            slider.GetComponent<Collider>().enabled = false;
        }
        else
        {
            slider.GetComponent<GrabbableObject>().enabled = true;
            slider.GetComponent<Collider>().enabled = true;
        }

        //get the distance between the min and max volume
        float maxDist = Mathf.Abs(Vector3.Distance(minVolume.localPosition, maxVolume.localPosition));

        //if the object is not being grabbed, make sure it's in the correct volume position
        if(!slider.GetComponent<GrabbableObject>().isGrabbed)
        {
            slider.localPosition = new Vector3(0, minVolume.localPosition.y - Mathf.Abs(maxDist) * GameData.Instance.volume, 0);
        }

        if (slider.localPosition.y > minVolume.localPosition.y)
        {
            slider.localPosition = minVolume.localPosition;
        }
        else if(slider.localPosition.y < maxVolume.localPosition.y)
        {
            slider.localPosition = maxVolume.localPosition;
        }

        float dist = Vector3.Distance(minVolume.localPosition, slider.localPosition);

        lastVolume = GameData.Instance.volume;
        {
            if(dist/maxDist != lastVolume)
            {
                GameData.Instance.volume = Mathf.Round((dist / maxDist) * 100)/100;
            }
        }

        volumeText.text = (GameData.Instance.volume * 100).ToString();
    }
}
