using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBGM : MonoBehaviour
{
    private Toggle myToggle;
    private AudioSource[] audioSources;
    //private AudioSource[] sfxAudio;

    // Start is called before the first frame update
    void Start()
    {
        myToggle = GetComponent<Toggle>();
        myToggle.onValueChanged.AddListener(delegate
        {
            ChangeSFXState(myToggle);
        });

        //sfxAudio = GameObject.FindGameObjectWithTag("MusicManager").GetComponents<AudioSource>();
        audioSources = FindObjectsOfType<AudioSource>();
    }

    private void ChangeSFXState(Toggle change)
    {
        if (myToggle.isOn)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                if(audioSources[i].tag != "MusicManager")
				{
                    audioSources[i].mute = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i].tag != "MusicManager")
                {
                    audioSources[i].mute = true;
                }
            }
        }
    }
}
