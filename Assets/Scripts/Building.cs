using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject ruins;
    [SerializeField] private string explosionTag;

    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        if(health.IsZero())
        {
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
            Instantiate(ruins, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
