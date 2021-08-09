using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnOffIndicator : MonoBehaviour
{
    [SerializeField] private ToggleByDamage toggler;
    [SerializeField] private Transform OnIndicator;
    [SerializeField] private Transform OffIndicator;

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
        OnIndicator.gameObject.SetActive(isActive);
        OffIndicator.gameObject.SetActive(!isActive);
    }
}
