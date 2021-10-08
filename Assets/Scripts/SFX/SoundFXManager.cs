using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundFxKey
{
    NONE,

    //UI
    SELECT,

    //CONTROLS
    SWITCH,

    //WEAPONS
    RIFLE,
    LASER_BEAM,
    MELTER,
    PORTAL,
    ROCKET,
    BOOM,

    //BUILDING
    BUILDING_DAMAGE,
    BUILDING_DESTROY,

    //OTHER
    PICKUP,
    SWITCH_ON,
    SWITCH_OFF,

    PROJECTILE_DESTROY,
    BOOST_START,
    MECH_DAMAGE,
    TELEPORT_BOOST,
    TELEPORT,

    BCOMP_SHOW,
    BCOMP_HIDE,
    BCOMP_SHOOT,
    BCOMP_DESTROY,
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

    public static bool state = true;

    private static SoundFXManager instance;

    private Dictionary<SoundFxKey, AudioClip[]> soundFxToAudioClipMap = new Dictionary<SoundFxKey, AudioClip[]>();

    private void Awake()
    {
        instance = this;
        Setup();
    }

    private void Setup()
    {
        if(state) instance.mainAudioSource.enabled = true;
        else instance.mainAudioSource.enabled = false;

        SetupOneShotAudioClips();
    }

    private void SetupOneShotAudioClips()
    {
        foreach(SoundFX sound in sounds)
        {
            soundFxToAudioClipMap.Add(sound.key, sound.clips);
        }
    }

    public static void SetState(bool state)
    {
        SoundFXManager.state = state;

        if(state) instance.mainAudioSource.enabled = true;
        else instance.mainAudioSource.enabled = false;
    }

    public static void PlayOneShot(SoundFxKey soundFxKey)
    {
        PlayOneShot(soundFxKey, instance.mainAudioSource);
    }

    public static void PlayOneShot(SoundFxKey soundFxKey, AudioSource audioSource)
    {
        // use this method to play oneshots in a specific audio source
        // this is useful when we want to play audio in specific places in the world.

        if(!state) return;
        if(soundFxKey == SoundFxKey.NONE) return;

        if(audioSource == null)
        {
            Debug.LogError($"Unable to play sound fx [{soundFxKey}]. AudioSource can not be null.");
            return;
        }

        if(false == TryGetRandomClip(soundFxKey, out AudioClip clip))
        {
            if (instance && instance.logDebug) Debug.LogWarning($"no audio clip found for [{soundFxKey}]");
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    private static bool TryGetRandomClip(SoundFxKey soundFxKey, out AudioClip clip)
    {
        clip = null;
        if(!instance) return false;
        if(soundFxKey == SoundFxKey.NONE) return false;
        
        if(false == instance.soundFxToAudioClipMap.ContainsKey(soundFxKey))
        {
            Debug.LogError($"no audio clip found for sound fx key [{soundFxKey}] on gameobject [{instance.gameObject.name}].");
            return false;
        }

        var clipArray = instance.soundFxToAudioClipMap[soundFxKey];
        if(clipArray == null || clipArray.Length <= 0)
        {
            Debug.LogError($"audio clip array is null/empty for sound fx key [{soundFxKey}] on gameobject [{instance.gameObject.name}].");
            return false;
        }

        clip = clipArray[Random.Range(0, clipArray.Length)];
        return true;
    }
}
