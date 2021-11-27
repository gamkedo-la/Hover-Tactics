using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouch : MonoBehaviour
{
    [SerializeField] private bool _touch = false;
    static public bool touch = false;
    private void Awake() {
        touch = _touch;
    }
}
