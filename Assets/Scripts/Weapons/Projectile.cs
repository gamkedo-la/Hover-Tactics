using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private string explosionTag;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float destroyDelay = 2.0f;

    private Vector3 impactForce;
    private Vector3 force;
    private float destroyTimer = 0.0f;

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
        destroyTimer = destroyDelay;
    }

    void Update()
    {
        if(impactForce != Vector3.zero)
        {
            rigidbody.AddForce(impactForce, ForceMode.Impulse);
            impactForce = Vector3.zero;
        }
        rigidbody.AddForce(force);

        if(destroyTimer <= 0.0f)
        {
            DisableObject();
        }
        else
        {
            destroyTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Building building = collision.other.GetComponent<Building>();
        if(building)
        {
            building.Damage(damage);
        }

        ContactPoint contact = collision.contacts[0];

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

        ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, rot);
        Debug.Log("Playing Explosion Sound");
        SoundFXManager.PlayOneShot(SoundFxKey.Explosion);
        meshRenderer.enabled = false;
        collider.enabled = false;

        Invoke("DisableObject", 1.0f);
    }

    void DisableObject()
    {
        destroyTimer = destroyDelay;
        meshRenderer.enabled = true;
        collider.enabled = true;
        impactForce = force = Vector3.zero;
        gameObject.SetActive(false);
    }
}
