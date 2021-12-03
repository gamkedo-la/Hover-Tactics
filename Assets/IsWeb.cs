using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWeb : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(Application.platform != RuntimePlatform.WebGLPlayer);
    }
}
