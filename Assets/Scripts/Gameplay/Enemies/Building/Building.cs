using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]
public class Building : MonoBehaviour
{
    [SerializeField] private GameObject ruins;
    [SerializeField] private SoundFxKey explosionSound;
    [SerializeField] private string explosionTag;

    private Health health;
    private AudioSource audioSource;

    void Start()
    {
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(health.IsZero())
        {
            SoundFXManager.PlayOneShot(explosionSound, audioSource);
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
            if(ruins) Instantiate(ruins, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
