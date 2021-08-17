using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MechBoost : MonoBehaviour
{
    public enum BoostState { Inactive, Active }
    
    [Range(1.0f, 10.0f)]
    [SerializeField] protected float boostMaxValue = 2.5f;
    [Range(0.1f, 2.5f)]
    [SerializeField] protected float boostCostPerSecond = 0.2f;
    [SerializeField] protected bool isTeleport = false;
    [SerializeField] private Power mechPower;

    protected float boostValue = 1.0f;
    protected bool isBoostActivated = false;
    public Action<BoostState> BoostActivateToggled;

    private Coroutine boostRoutine;
    
    private void Start()
    {
        Assert.IsNotNull(mechPower, "Mech Power is null!");
    }

    private void Update()
    {
        if(!Teleport() && IsBoostActive())
        {
            mechPower.ChangeBy(-boostCostPerSecond * Time.deltaTime);
            if(!HasEnoughPower())
            {
                DeactivateBoost();
            }
        }
    }

    private bool Teleport()
    {
        if(isTeleport)
        {
            if(isBoostActivated && mechPower.Get() >= boostCostPerSecond)
            {
                GetComponent<MechController>().Teleport(boostMaxValue);
                mechPower.ChangeBy(-boostCostPerSecond);
            }
            isBoostActivated = false;
            return true;
        }
        return false;
    }

    public void ActivateBoost()
    {
        if(isBoostActivated)
        {
             return;
        }

        if(!HasEnoughPower())
        {
            return;
        }

        if(boostRoutine != null)
        {
            StopCoroutine(boostRoutine);
        }

        isBoostActivated = true;
        boostValue = boostMaxValue;
        BoostActivateToggled?.Invoke(BoostState.Active);

        boostRoutine = StartCoroutine(BoostRoutine());
    }

    public void DeactivateBoost()
    {
        if(boostRoutine != null)
        {
            StopCoroutine(boostRoutine);
        }

        boostValue = 1.0f;
        isBoostActivated = false;
        BoostActivateToggled?.Invoke(BoostState.Inactive);
    }


    protected virtual IEnumerator BoostRoutine()
    {
        while(IsBoostActive())
        {
            if(!HasEnoughPower())
            {
                DeactivateBoost();
            }
            else
            {
                DepletePower(-boostCostPerSecond * Time.deltaTime);
            }

            yield return new WaitForEndOfFrame();
        }
        DeactivateBoost();
    }

    public bool IsBoostActive()
    {
        return isBoostActivated;
    }

    public virtual float GetBoostValue()
    {
        return boostValue;
    }

    protected virtual bool HasEnoughPower()
    {
        return mechPower.Get() > 0;
    }

    protected virtual void DepletePower(float value)
    {
        mechPower.ChangeBy(value);
    }
}
