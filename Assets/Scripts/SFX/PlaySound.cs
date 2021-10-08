using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public SoundFxKey sound;
    public float volume = 1.0f;
    private AudioSource audioSource = null;

    void OnEnable()
    {
        if(audioSource == null) audioSource = GetComponent<AudioSource>();
        if(audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        SoundFXManager.PlayOneShot(sound, audioSource);
    }
}
