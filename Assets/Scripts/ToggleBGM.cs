using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBGM : MonoBehaviour
{
    private Toggle myToggle;
    
    void Start()
    {
        myToggle = GetComponent<Toggle>();
        myToggle.onValueChanged.AddListener(delegate
        {
            ChangeSFXState(myToggle);
        });
    }

    private void ChangeSFXState(Toggle change)
    {
        MusicManager.state = myToggle.isOn;
    }
}
