using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private float damagePerCycle;
    [SerializeField] private float cycleDelay;

    private List<Health> healthList;
    private float cycleTimer = 0.0f;

    void Start()
    {
        healthList = new List<Health>();
    }

    void Update()
    {
        if(cycleTimer <= 0.0f)
        {
            if(healthList.Count > 0)
            {
                foreach(Health health in healthList)
                    health.ChangeBy(-damagePerCycle);
                cycleTimer = cycleDelay;
            }
        }
        else
        {
            cycleTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if(health) healthList.Add(health);
    }

    void OnTriggerExit(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if(health) healthList.Remove(health);
    }
}
