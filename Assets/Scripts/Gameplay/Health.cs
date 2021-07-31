using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1.0f;
    private float currentHealth = 1.0f;

    public void ChangeBy(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0.0f, maxHealth);
    }

    public bool IsZero()
    {
        return currentHealth <= 0.0f;
    }

    public void SetToFull()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
}
