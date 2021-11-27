using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMobile : MonoBehaviour
{
    [Multiline(10), SerializeField] private string mobileText;
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if(IsTouch.touch) text.text = mobileText;
    }
}
