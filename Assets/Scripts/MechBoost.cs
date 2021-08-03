using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MechBoost : MonoBehaviour
{
    public enum BoostState { Inactive, Active }
    [SerializeField] protected AnimationCurve boostValueCurve;
    
    [Range(1.0f, 5f)]
    [SerializeField] protected float boostMaxValue = 2.5f;
    [Range(1.0f, 5f)]
    [SerializeField] protected float boostDuration = 2.5f;
    [Range(1.0f, 5f)]
    [SerializeField] protected float boostCooldown = 2.5f;
    protected float boostValue = 1.0f;
    protected bool isBoostActivated = false;
    public Action<BoostState> BoostActivateToggled;

    public void ActivateBoost()
    {
        if(isBoostActivated)
        {
             return;
        }

        boostValue = boostMaxValue;
        isBoostActivated = true;
        BoostActivateToggled?.Invoke(BoostState.Active);
    }

    public void DeactivateBoost()
    {
        boostValue = 1.0f;
        isBoostActivated = false;
        BoostActivateToggled?.Invoke(BoostState.Inactive);
    }

    public bool IsBoostActive()
    {
        return isBoostActivated;
    }

    public virtual float GetBoostValue()
    {
        return boostValue;
    }
}
