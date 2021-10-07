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
            ChangeBGMState(myToggle);
        });
    }

    private void ChangeBGMState(Toggle change)
    {
        MusicManager.state = myToggle.isOn;
    }
}
