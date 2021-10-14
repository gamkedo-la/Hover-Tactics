using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthRegeneration : MonoBehaviour
{
    [SerializeField] private float regenerationRate = 0.02f;
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        health.ChangeBy(regenerationRate * Time.deltaTime * AssistPanel.GetHealth());
    }
}
