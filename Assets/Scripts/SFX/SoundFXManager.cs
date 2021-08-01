using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundFxKey
{
    None,
    Select,
    Explosion,
    Shoot,
    RifleFire,
};

[System.Serializable]
public class SoundFX
{
    public SoundFxKey key;
    public AudioClip[] clips;
}

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private SoundFX[] sounds;

    [Header("Debug")]
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
        foreach(SoundFX sound in sounds)
        {
            soundFxToAudioClipMap.Add(sound.key, sound.clips);
        }
    }

    public static void PlayOneShot(SoundFxKey soundFxKey)
    {  
        if(soundFxKey == SoundFxKey.None) return;
        PlayOneShot(soundFxKey, Instance.mainAudioSource);
    }

    public static void PlayOneShot(SoundFxKey soundFxKey, AudioSource audioSource)
    {
        // use this method to play oneshots in a specific audio source
        // this is useful when we want to play audio in specific places in the world.

        if(soundFxKey == SoundFxKey.None) return;

        if(audioSource == null)
        {
            Debug.LogError($"Unable to play sound fx [{soundFxKey}]. AudioSource can not be null.");
            return;
        }

        if(false == TryGetRandomClip(soundFxKey, out AudioClip clip))
        {
            if (Instance.logDebug) Debug.LogWarning($"no audio clip found for [{soundFxKey}]");
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
