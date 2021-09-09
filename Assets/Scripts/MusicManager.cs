using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource loop;

    [Range(0.0f, 1.0f)]
    public float Volume = .75f;

    // Start is called before the first frame update
    void Start()
    {
        intro.volume = Volume;
        loop.volume = Volume;

        intro.Play();
        loop.PlayDelayed(intro.clip.length);
        loop.loop = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
