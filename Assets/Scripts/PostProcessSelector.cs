using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessSelector : MonoBehaviour
{
    public VolumeProfile standardProfile;
    public VolumeProfile mobileProfile;
    
    void Start()
    {
        GetComponent<Volume>().profile = IsTouch.touch ? mobileProfile : standardProfile;
        QualitySettings.SetQualityLevel(IsTouch.touch ? 0 : 1);
    }
}
