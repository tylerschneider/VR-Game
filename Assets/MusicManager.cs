using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public float fadeSpeed = 0.05f;
    public AudioSource source1;
    public AudioSource source2;
    private string fade1 = "";
    private string fade2 = "";
    private AudioClip lastSong = null;
    public AudioClip currentSong = null;
    private int sourcePlaying = 1;

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        currentSong = source1.clip;

        source1.Play();
        source2.Play();
    }

    public void ChangeMusic(AudioClip newSong)
    {
        Debug.Log(source1.isPlaying + " " + source2.isPlaying);

        //if the new song is not the same as the current song
        if (newSong != currentSong)
        {
            //if the new song was not the last song playing
            if (newSong != lastSong)
            {
                //if source 1 is not playing
                if(sourcePlaying != 1)
                {
                    source1.clip = newSong;
                    source1.mute = false;
                    source1.Play();
                    sourcePlaying = 1;
                    fade1 = "in";
                    fade2 = "out";
                }
                else
                {
                    source2.clip = newSong;
                    source2.mute = false;
                    source2.Play();
                    sourcePlaying = 2;
                    fade1 = "out";
                    fade2 = "in";
                }
            }
            //if the song was the last song playing
            else
            {
                if (sourcePlaying != 1)
                {
                    source1.mute = false;
                    sourcePlaying = 1;
                    fade1 = "in";
                    fade2 = "out";
                }
                else
                {
                    source2.mute = false;
                    sourcePlaying = 2;
                    fade1 = "out";
                    fade2 = "in";
                }
            }

            lastSong = currentSong;
            currentSong = newSong;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fade1 == "in" && source1.volume < 1)
        {
            source1.volume += fadeSpeed;

            if(source1.volume >= 1)
            {
                fade1 = "";
            }
        }
        else if(fade1 == "out" && source1.volume > 0)
        {
            if (source1.volume - fadeSpeed <= 0)
            {
                source1.mute = true;
                fade1 = "";
            }
            else
            {
                source1.volume -= fadeSpeed;
            }
        }

        if (fade2 == "in" && source2.volume < 1)
        {
            source2.volume += fadeSpeed;

            if (source2.volume >= 1)
            {
                fade2 = "";
            }
        }
        else if (fade2 == "out" && source2.volume > 0)
        {
            if (source2.volume - fadeSpeed <= 0)
            {
                source2.mute = true;
                fade2 = "";
            }
            else
            {
                source2.volume -= fadeSpeed;
            }
        }
    }
}
