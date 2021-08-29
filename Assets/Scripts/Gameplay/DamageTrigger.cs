using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour, IDamage
{
    [SerializeField] private float damagePerCycle;
    [SerializeField] private float cycleDelay;

    [Header("Optional")]
    [Tooltip("Destroys the object the moment someone triggers it. Only one cycle of damage will be applied.")]
    [SerializeField] private bool destroyOnTrigger = false;
    [Tooltip("Particles are only spawned when destroyOnTrigger is true.")]
    [SerializeField] private string particleTag = "";

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
        if(canTakeDamage)
        {
            if(destroyOnTrigger)
            {
                canTakeDamage.TakeDamage(GetDamage());
                ObjectPooler.instance.SpawnFromPool(particleTag, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                takeDamageList.Add(canTakeDamage);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        AbstractTakeDamage canTakeDamage = other.gameObject.GetComponent<AbstractTakeDamage>();
        if(canTakeDamage) takeDamageList.Remove(canTakeDamage);
    }
}
