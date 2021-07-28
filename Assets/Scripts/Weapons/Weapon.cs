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
    [SerializeField] private float raycastLineDelay = 0.1f;

    [Header("UI")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image cooldownBar;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private float cooldownTimer = 0.0f;
    private float raycastLineTimer = 0.0f;
    private SpaceshipController controller;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private LaserController laserController;

    void Start()
    {
        controller = GetComponent<SpaceshipController>();
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        laserController = GetComponent<LaserController>();

        if(logDebug)
        {
            if(controller == null) Debug.LogWarning($"component [{nameof(SpaceshipController)}] not found in [{this.gameObject.name}]");
            if(lineRenderer == null) Debug.LogWarning($"component [{nameof(LineRenderer)}] not found in [{this.gameObject.name}]");
            if(audioSource == null) Debug.LogWarning($"component [{nameof(AudioSource)}] not found in [{this.gameObject.name}]");
            if(laserController == null) Debug.LogWarning($"component [{nameof(LaserController)}] not found in [{this.gameObject.name}]");
        }
    }

    void Update()
    {
        if(controller.enabled)
        {
            if(cooldownTimer <= 0.0f)
            {
                if((type == Type.BASIC && Input.GetButton("Fire1"))
                || (type == Type.SPECIAL && Input.GetButtonDown("Fire2")))
                {
                    shootingPoint.LookAt(CameraController.singleton.lastAimPoint);

                    Fire();
                    cooldownTimer = cooldownDelay;
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }

        if(lineRenderer)
        {
            /*
            if(raycastLineTimer <= 0.0f)
            {
                lineRenderer.enabled = false;
                Color color = lineRenderer.startColor;
                color.a = 1.0f;
                lineRenderer.startColor = lineRenderer.endColor = color;
            }
            else
            {
                Color color = lineRenderer.startColor;
                color.a = raycastLineTimer / raycastLineDelay;
                lineRenderer.startColor = lineRenderer.endColor = color;
                raycastLineTimer -= Time.deltaTime;
            }
            */
        }
    }

    public virtual void Fire()
    {
        if(bulletTag == "")
        {
            if(logDebug) Debug.Log("bulletTag is null");

            lineRenderer.SetPosition(0, shootingPoint.position);
            lineRenderer.SetPosition(1, shootingPoint.position + (shootingPoint.forward * 1000.0f));
            //lineRenderer.enabled = true;
            //raycastLineTimer = raycastLineDelay;


            RaycastHit hitData;
            if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hitData))
            {
                lineRenderer.SetPosition(1, hitData.point);

                Building building = hitData.transform.gameObject.GetComponent<Building>();
                if (building)
                {
                    building.Damage(0.1f);
                }

                laserController.Shoot(raycastLineDelay, Vector3.Distance(shootingPoint.position, hitData.point));
            }
            else
            {
                laserController.Shoot(raycastLineDelay, 1000);
            }
        }
        else
        {
            if(logDebug) Debug.Log("bulletTag is not null, spawning new bullet");
            GameObject newBullet = ObjectPooler.instance.SpawnFromPool(bulletTag, shootingPoint.position, shootingPoint.rotation);
            newBullet.GetComponent<Projectile>().SetForce((shootingPoint.forward * (speed/2.0f)) + (shootingPoint.up * (speed/2.0f)), Vector3.zero);
        }

        if(logDebug) Debug.Log("Playing Shoot Sound");
        SoundFXManager.PlayOneShot(SoundFxKey.Shoot, audioSource);
    }
}
