using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private string explosionTag;
    [SerializeField] private AudioClip explosionSound;

    private MeshRenderer meshRenderer;
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    public void SetSpeed(float speed)
    {
        rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter()
    {
        ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
        audioSource.PlayOneShot(explosionSound);
        meshRenderer.enabled = false;

        Invoke("DisableObject", 1.0f);
    }

    void DisableObject()
    {
        meshRenderer.enabled = true;
        gameObject.SetActive(false);
    }
}
