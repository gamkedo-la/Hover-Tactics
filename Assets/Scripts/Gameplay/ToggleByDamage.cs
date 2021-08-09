using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ToggleByDamage : AbstractTakeDamage
{
    public bool IsActive { set; protected get; }
    public Action<bool> OnToggled;
    [SerializeField] private bool initialToggleState = false;
    [SerializeField] private float toggleWaitTimeInSecs = 0.3f;

    private bool canToggle;
    private Coroutine toggleRoutine;

    private void Start()
    {
        IsActive = initialToggleState;
        canToggle = true;
    }
    
    public override void TakeDamage(Damage damage)
    {
        if(canToggle == false)
        {
            return;
        }

        Toggle();
    }

    protected virtual void Toggle(bool fireEvent = true, bool startToggleWaitTime = true)
    {
        IsActive = !IsActive;

        if(fireEvent)
        {
            OnToggled?.Invoke(IsActive);
        }

        if(startToggleWaitTime)
        {
            if(toggleRoutine != null)
            {
                StopCoroutine(toggleRoutine);
            }
            toggleRoutine = StartCoroutine(ToggleWaitTimeRoutine());
        }
    }

    protected virtual IEnumerator ToggleWaitTimeRoutine()
    {
        canToggle = false;
        yield return new WaitForSeconds(toggleWaitTimeInSecs);
        canToggle = true;
    }
}
