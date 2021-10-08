using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource loop;
    public bool isLoopOnly = true;
    public float volumeLerp = 0.0f;

    public static bool state = true;

    [Range(0.0f, 1.0f)]
    public float Volume = .75f;

    void Start()
    {
        if(volumeLerp <= 0.0f)
        {
            if(!isLoopOnly) intro.volume = Volume;
            loop.volume = Volume;
        }
        else
        {
            if(!isLoopOnly)
            {
                intro.volume = 0.0f;
                loop.volume = Volume;
            }
            else
            {
                loop.volume = 0.0f;
            }
        }

        if(!isLoopOnly)
        {
            intro.Play();
            loop.PlayDelayed(intro.clip.length);
        }
        else
        {
            loop.Play();
        }

        loop.loop = true;
    }

    void Update()
    {
        if(!state)
        {
            loop.enabled = false;
            if(intro != null)
                intro.enabled = false;
        }
        else if(loop.enabled == false)
        {
            loop.enabled = true;
            if(intro != null)
                intro.enabled = true;
            Start();
        }
        
        if(volumeLerp > 0.0f)
        {
            if(isLoopOnly)
                loop.volume = Mathf.Lerp(loop.volume, Volume, volumeLerp * Time.unscaledDeltaTime);
            else
                intro.volume = Mathf.Lerp(intro.volume, Volume, volumeLerp * Time.unscaledDeltaTime);
        }
    }
}
