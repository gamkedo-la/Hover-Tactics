using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSFX : MonoBehaviour
{
    private Toggle myToggle;
    private AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        myToggle = GetComponent<Toggle>();
        myToggle.onValueChanged.AddListener(delegate
        {
            ChangeSFXState(myToggle);
        });

        audioSources = GameObject.FindGameObjectWithTag("MusicManager").GetComponents<AudioSource>();
    }

    private void ChangeSFXState(Toggle change)
	{
        if(myToggle.isOn)
		{
			for (int i = 0; i < audioSources.Length; i++)
			{
                audioSources[i].mute = false;
            }
		}
		else
		{
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].mute = true;
            }
        }
	}
}
