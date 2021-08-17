using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFenceToggler : MonoBehaviour
{
    [SerializeField] LaserFence[] laserFences;
    [SerializeField] ToggleByDamage toggler;

    private void OnEnable()
    {
        toggler.OnToggled += OnToggleHandler;
    }

    private void OnDisable()
    {
        toggler.OnToggled -= OnToggleHandler;
    }


    protected virtual void OnToggleHandler(bool isActive)
    {
        foreach(LaserFence fence in laserFences)
        {
            fence.ToggleFence(isActive);
        }
    }
}
