using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, IDamage
{
    [Header("Forces")]
    [SerializeField] private float forwardForce = 10.0f;
    [SerializeField] private float upwardForce = 2.5f;
    [Header("Properties")]
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private string onDestroyObjectTag = "";
    [Header("Effects")]
    [SerializeField] private string explosionTag;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float destroyDelay = 2.0f;

    private Vector3 impactForce;
    private float destroyTimer = 0.0f;

    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private Collider projectileCollider;
    private AudioSource audioSource;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(meshRenderer, "Mesh Renderer is null!");
        Assert.IsNotNull(rb, "Rigidbody is null!");
        Assert.IsNotNull(projectileCollider, "Projectile Collider is null!");
        Assert.IsNotNull(audioSource, "Audio Source is null!");
    }

    void OnEnable()
    {
        destroyTimer = destroyDelay;
        this.impactForce = (transform.forward * forwardForce) + (transform.up * upwardForce);
    }

    void Update()
    {
        if(impactForce != Vector3.zero)
        {
            rb.AddForce(impactForce, ForceMode.Impulse);
            impactForce = Vector3.zero;
        }

        if(destroyTimer <= 0.0f)
            DisableObject();
        else
            destroyTimer -= Time.deltaTime;
    }

    public Damage GetDamage()
    {
        return new Damage()
        {
            Value = -damage,
        };
    }

    void OnCollisionEnter(Collision coll)
    {
        ObjectPooler.instance.SpawnFromPool(onDestroyObjectTag, transform.position, Quaternion.identity);

        DestroyEffects(coll);
        meshRenderer.enabled = false;
        projectileCollider.enabled = false;
        Invoke("DisableObject", 1.0f);

        AbstractTakeDamage canTakeDamage = coll.transform.GetComponent<AbstractTakeDamage>();
        if(canTakeDamage) canTakeDamage.TakeDamage(GetDamage());
    }

    void DestroyEffects(Collision coll)
    {
        ContactPoint contact = coll.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, rot);
        SoundFXManager.PlayOneShot(SoundFxKey.SMALL_BUILDING_EXPLOSION);
    }

    void DisableObject()
    {
        destroyTimer = destroyDelay;
        meshRenderer.enabled = true;
        projectileCollider.enabled = true;
        impactForce = Vector3.zero;
        gameObject.SetActive(false);
    }
}
