using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public enum Type {
        BASIC,
        SPECIAL
    };

    [Header("Properties")]
    [SerializeField] private Type type = Type.BASIC;
    [SerializeField] private float speed = -1.0f; //-1 = raycast, 0> = projectile
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float cooldownDelay = 0.1f;

    [Header("AudioVisual")]
    [SerializeField] private string bulletTag;
    [SerializeField] private AudioClip shootingSound;

    [Header("UI")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image cooldownBar;

    private float cooldownTimer = 0.0f;
    private SpaceshipController controller;
    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<SpaceshipController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(controller.enabled)
        {
            if(cooldownTimer <= 0.0f)
            {
                if((type == Type.BASIC && Input.GetButtonDown("Fire1"))
                || (type == Type.SPECIAL && Input.GetButtonDown("Fire2")))
                {
                    Fire();
                    cooldownTimer = cooldownDelay;
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }
    }

    public virtual void Fire()
    {
        GameObject newBullet = ObjectPooler.instance.SpawnFromPool(bulletTag, shootingPoint.position, shootingPoint.rotation);
        newBullet.GetComponent<Projectile>().SetForce((shootingPoint.forward * (speed/2.0f)) + (shootingPoint.up * (speed/2.0f)), Vector3.zero);
        audioSource.PlayOneShot(shootingSound);
    }
}
