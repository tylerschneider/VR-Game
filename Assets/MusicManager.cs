using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //create a static variable for this script to access it from other scripts
    public static MusicManager Instance;
    //speed the music fades
    public float fadeSpeed = 0.05f;
    //audio sources that play the music
    public AudioSource source1;
    public AudioSource source2;
    //variable to tell whether a source should be fading in or out
    private string fade1 = "";
    private string fade2 = "";
    //variables to track the last and current songs
    private AudioClip lastSong = null;
    public AudioClip currentSong = null;
    //variable to track which source should be playing
    private int sourcePlaying = 1;

    void Awake()
    {
        Instance = this;

        //dont destroy so that we can keep the music manager between scenes
        DontDestroyOnLoad(gameObject);

        //get the current song
        currentSong = source1.clip;

        //play both audio sources
        source1.Play();
        source2.Play();
    }

    public void ChangeMusic(AudioClip newSong)
    {

        //if the new song is not the same as the current song
        if (newSong != currentSong)
        {
            //if the new song was not the last song playing
            if (newSong != lastSong)
            {
                //if source 1 is not playing
                if(sourcePlaying != 1)
                {
                    //set source 1's clip to the new song
                    source1.clip = newSong;
                    //unmute the audio source
                    source1.mute = false;
                    //make sure the source is playing
                    source1.Play();
                    //track which source is playing
                    sourcePlaying = 1;
                    //fade this source in
                    fade1 = "in";
                    //fade the other source out
                    fade2 = "out";
                }
                //if source 2 is playing, do the same but reversed
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
                //switch the audio sources sounds instead of assigning a new song, so that the song does not start over
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

            //set the last song to the current song
            lastSong = currentSong;
            //set the current song as the new song
            currentSong = newSong;
        }
    }

    void Update()
    {
        //if source 1 is set to fade in and has not reached max volume
        if(fade1 == "in" && source1.volume < 1)
        {
            //increase the volume
            source1.volume += fadeSpeed;

            //if volume is maxed, stop fading
            if(source1.volume >= 1)
            {
                fade1 = "";
            }
        }
        //do the same but reversed if fading out
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

        //repeat for the other source
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
