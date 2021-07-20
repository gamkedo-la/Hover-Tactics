using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private string explosionTag;
    [SerializeField] private AudioClip explosionSound;

    private Vector3 impactForce;
    private Vector3 force;

    private MeshRenderer meshRenderer;
    private Rigidbody rigidbody;
    private Collider collider;
    private AudioSource audioSource;

    public void SetForce(Vector3 impactForce, Vector3 force)
    {
        this.impactForce = impactForce;
        this.force = force;
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(impactForce != Vector3.zero)
        {
            rigidbody.AddForce(impactForce, ForceMode.Impulse);
            impactForce = Vector3.zero;
        }
        rigidbody.AddForce(force);
    }

    void OnCollisionEnter()
    {
        ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
        audioSource.PlayOneShot(explosionSound);
        meshRenderer.enabled = false;
        collider.enabled = false;

        Invoke("DisableObject", 1.0f);
    }

    void DisableObject()
    {
        meshRenderer.enabled = true;
        collider.enabled = true;
        impactForce = force = Vector3.zero;
        gameObject.SetActive(false);
    }
}
