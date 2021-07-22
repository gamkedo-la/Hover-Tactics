using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private float hitpoints = 1.0f;
    [SerializeField] private GameObject ruins;
    [SerializeField] private string explosionTag;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(float value)
    {
        hitpoints -= value;
        if(hitpoints <= 0)
        {
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
            Instantiate(ruins, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
