using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouch : MonoBehaviour
{
    static public bool touch = false;
    private void Awake() {
        touch = (Application.platform == RuntimePlatform.Android);
    }
}
