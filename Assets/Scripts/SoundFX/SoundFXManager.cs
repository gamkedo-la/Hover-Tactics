using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [Header("OneShot Audio Sources")]
    [SerializeField] private AudioSource mainAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip select;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip shoot;

    [Header("Debug Switch")]
    [SerializeField] private bool logDebug = false;

    private static SoundFXManager _instance;
    private static SoundFXManager Instance
    {
        get
        {
            if(_instance == null)
            {
                // instead of logging an error, we could instantiate this as a prefab.
                Debug.LogError($"There is no object in the current scene with this script attached to it.");
            }

            return _instance;
        }
    }

    private Dictionary<SoundFxKey, AudioClip[]> soundFxToAudioClipMap = new Dictionary<SoundFxKey, AudioClip[]>();

    private void Awake()
    {
        _instance = this;

        Setup();
    }

    private void Setup()
    {
        SetupOneShotAudioClips();
    }

    private void SetupOneShotAudioClips()
    {
        soundFxToAudioClipMap.Add(SoundFxKey.Select, new AudioClip[] { select });
        soundFxToAudioClipMap.Add(SoundFxKey.Explosion, new AudioClip[] { explosion });
        soundFxToAudioClipMap.Add(SoundFxKey.Shoot, new AudioClip[] { shoot });
    }

    public static void PlayOneShot(SoundFxKey soundFxKey)
    {  
        PlayOneShot(soundFxKey, Instance.mainAudioSource);
    }

    public static void PlayOneShot(SoundFxKey soundFxKey, AudioSource audioSource)
    {
        // use this method to play oneshots in a specific audio source
        // this is useful when we want to play audio in specific places in the world.

        if(audioSource == null)
        {
            Debug.LogError($"Unable to play sound fx [{soundFxKey}]. AudioSource can not be null.");
            return;
        }

        if(false == TryGetRandomClip(soundFxKey, out AudioClip clip))
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    private static bool TryGetRandomClip(SoundFxKey soundFxKey, out AudioClip clip)
    {
        clip = null;
        if(false == Instance.soundFxToAudioClipMap.ContainsKey(soundFxKey))
        {
            Debug.LogError($"no audio clip found for sound fx key [{soundFxKey}] on gameobject [{Instance.gameObject.name}].");
            return false;
        }

        var clipArray = Instance.soundFxToAudioClipMap[soundFxKey];
        if(clipArray == null || clipArray.Length <= 0)
        {
            Debug.LogError($"audio clip array is null/empty for sound fx key [{soundFxKey}] on gameobject [{Instance.gameObject.name}].");
            return false;
        }

        clip = clipArray[Random.Range(0, clipArray.Length)];
        return true;
    }
}
