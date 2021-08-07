using UnityEngine;
using UnityEngine.Assertions;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject ruins;
    [SerializeField] private string explosionTag;

    private Health health;

    void Start()
    {
        health = GetComponent<Health>();

        Assert.IsNotNull(health, "Health is null!");
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
