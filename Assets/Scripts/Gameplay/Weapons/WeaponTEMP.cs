using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class WeaponTEMP : MonoBehaviour
{
    public enum Type {
        BASIC,
        SPECIAL
    };

    [Header("Properties")]
    [SerializeField] private Type type = Type.BASIC;
    [SerializeField] private float speed = -1.0f;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float cooldownDelay = 0.1f;

    [Header("AudioVisual")]
    [SerializeField] private string bulletTag;
    [SerializeField] private string explosionTag;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private float LaserUpTime = 0.1f;
    [SerializeField] private float LaserDecayTime = 0.1f;

    [Header("UI")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image cooldownBar;

    private float cooldownTimer = 0.0f;
    private float raycastLineTimer = 0.0f;
    private MechController controller;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private LaserController laserController;

    void Start()
    {
        controller = GetComponent<MechController>();
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        laserController = GetComponent<LaserController>();

        //assert statements are only included in development build
        Assert.IsNotNull(controller, "Mech Controller is null!");
        Assert.IsNotNull(lineRenderer, "Line Renderer is null!");
        Assert.IsNotNull(audioSource, "Audio Source is null!");
        Assert.IsNotNull(laserController, "Laser Controller is null!");
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
                    Vector3 shootingPointEuler = shootingPoint.localRotation.eulerAngles;
                    shootingPointEuler.y = 0;
                    shootingPointEuler.z = 0;
                    shootingPoint.localRotation = Quaternion.Euler(shootingPointEuler);

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
            lineRenderer.SetPosition(0, shootingPoint.position);
            lineRenderer.SetPosition(1, shootingPoint.position + (shootingPoint.forward * 1000.0f));
            //lineRenderer.enabled = true;
            //raycastLineTimer = raycastLineDelay;

            RaycastHit hitData;
            if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hitData))
            {
                lineRenderer.SetPosition(1, hitData.point);

                laserController.Shoot(LaserUpTime, LaserDecayTime, Vector3.Distance(shootingPoint.position, hitData.point));

                Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitData.normal);

                ObjectPooler.instance.SpawnFromPool(explosionTag, hitData.point, rot);
            }
            else
            {
                laserController.Shoot(LaserUpTime, LaserDecayTime, 1000);
            }
        }
        else
        {
            ObjectPooler.instance.SpawnFromPool(bulletTag, shootingPoint.position, shootingPoint.rotation);
        }

        SoundFXManager.PlayOneShot(SoundFxKey.Shoot, audioSource);
    }
}
