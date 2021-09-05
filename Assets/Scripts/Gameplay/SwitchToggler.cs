using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToggler : MonoBehaviour
{
    [SerializeField] private ToggleByDamage toggler;
    [SerializeField] private GameObject[] activateObjects;
    [SerializeField] private GameObject[] deactivateObjects;

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
        for(int i = 0; i < activateObjects.Length; i++)
            activateObjects[i].SetActive(isActive);
        for(int i = 0; i < deactivateObjects.Length; i++)
            deactivateObjects[i].SetActive(!isActive);
    }
}
