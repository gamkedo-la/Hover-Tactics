using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBoostTrusthModifier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected MechBoost mechBoost;
    [SerializeField] protected TrailRenderer trailRenderer;

    [Header("Configuration")]
    [SerializeField] protected Gradient modifiedColor;
    [SerializeField] protected Gradient cooldownGradient;
    private Dictionary<MechBoost.BoostState, Gradient> gradientModifiers;

    private void Start()
    {
        gradientModifiers = new Dictionary<MechBoost.BoostState, Gradient>();
        gradientModifiers.Add(MechBoost.BoostState.Inactive, trailRenderer.colorGradient);
        gradientModifiers.Add(MechBoost.BoostState.Active, modifiedColor);
        gradientModifiers.Add(MechBoost.BoostState.Coolindown, cooldownGradient);
    }

    private void OnEnable()
    {
        if(mechBoost)
        {
            mechBoost.BoostActivateToggled += HandleBoostActivateToggle;
        }
    }

    private void OnDisable()
    {
        if(mechBoost)
        {
            mechBoost.BoostActivateToggled -= HandleBoostActivateToggle;
        }
    }

    protected virtual void HandleBoostActivateToggle(MechBoost.BoostState boostState)
    {
        if(gradientModifiers.ContainsKey(boostState))
        {
            trailRenderer.colorGradient = gradientModifiers[boostState];
        }
        else
        {
            trailRenderer.colorGradient = gradientModifiers[MechBoost.BoostState.Inactive];
        }
    }
}
