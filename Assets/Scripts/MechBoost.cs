using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBoost : MonoBehaviour
{
    [SerializeField] protected AnimationCurve boostValueCurve;
    
    [Range(1.0f, 5f)]
    [SerializeField] protected float boostMaxValue = 2.5f;
    [Range(1.0f, 5f)]
    [SerializeField] protected float boostDuration = 2.5f;
    [Range(1.0f, 5f)]
    [SerializeField] protected float boostCooldown = 2.5f;
    [SerializeField] protected float boostValue = 1.0f;
    protected bool isBoostActivated = false;
    public void ActivateBoost()
    {
        if(isBoostActivated)
        {
             return;
        }

        StartCoroutine(BoostRoutine());
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
        isBoostActivated = false;

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

        yield return new WaitForSeconds(boostCooldown);
        isBoostActivated = false;
    }
}
