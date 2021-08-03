using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MechBoost : MonoBehaviour
{
    public enum BoostState { Inactive, Active, Coolindown }
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

        //StartCoroutine(BoostRoutine());
    }

    public void DeactivateBoost()
    {
        boostValue = 1.0f;
        isBoostActivated = false;
    }

    public bool IsBoostActive()
    {
        return isBoostActivated;
    }

    public virtual float GetBoostValue()
    {
        return boostValue;
    }

    private IEnumerator BoostRoutine()
    {
        isBoostActivated = true;
        BoostActivateToggled?.Invoke(BoostState.Active);

        var elapsed = 0.0f;
        while(elapsed <= boostDuration)
        {
            var percentage = elapsed/boostDuration;
            boostValue = Mathf.Lerp(1.0f, boostMaxValue, boostValueCurve.Evaluate(1.0f));
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }

        boostValue = Mathf.Lerp(1.0f, boostMaxValue, boostValueCurve.Evaluate(1.0f));

        yield return new WaitForEndOfFrame();
        boostValue = 1.0f;
        BoostActivateToggled?.Invoke(BoostState.Coolindown);

        yield return new WaitForSeconds(boostCooldown);
        isBoostActivated = false;
        BoostActivateToggled?.Invoke(BoostState.Inactive);
    }
}
