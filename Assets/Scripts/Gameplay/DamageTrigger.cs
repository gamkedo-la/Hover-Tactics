using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour, IDamage
{
    [SerializeField] private float damagePerCycle;
    [SerializeField] private float cycleDelay;

    private List<AbstractTakeDamage> takeDamageList;
    private float cycleTimer = 0.0f;
    public Damage GetDamage()
    {
        return new Damage()
        {
            Value = -damagePerCycle
        };
    }

    void Start()
    {
        takeDamageList = new List<AbstractTakeDamage>();
    }

    void Update()
    {
        if(cycleTimer <= 0.0f)
        {
            if(takeDamageList.Count > 0)
            {
                foreach(AbstractTakeDamage takeDamage in takeDamageList)
                {
                    takeDamage.TakeDamage(GetDamage());
                }
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
        AbstractTakeDamage canTakeDamage = other.gameObject.GetComponent<AbstractTakeDamage>();
        if(canTakeDamage) takeDamageList.Add(canTakeDamage);
    }

    void OnTriggerExit(Collider other)
    {
        AbstractTakeDamage canTakeDamage = other.gameObject.GetComponent<AbstractTakeDamage>();
        if(canTakeDamage) takeDamageList.Remove(canTakeDamage);
    }
}
