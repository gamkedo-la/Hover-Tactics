using System.Collections;
using UnityEngine;

class MultiShooterBComp : BuildingComponent
{
    [Header("Timers")]
    [SerializeField] private float delayPerShot = 0.25f;
    [SerializeField] private int shotsPerCycle = 1;
    [SerializeField] private float delayPerCycle = 2.0f;

    [Header("Pattern")]
    [SerializeField] private bool doDelayBetweenShooters = false;
    [SerializeField] private float delayPerShooter = 0.25f;
    [SerializeField] private float yRotatePerShooter = 0.0f;
    [SerializeField] private float yRotatePerShot = 0.0f;
    [SerializeField] private float yRotatePerCycle = 0.0f;

    [Header("Muzzle")]
    [SerializeField] private Transform[] muzzleTransforms;
    [SerializeField] private string muzzleFlashTag;

    [Header("Projectile")]
    [SerializeField] private string projectileTag;
    [SerializeField] private SoundFxKey shotSound;

    private float shotTimer = 0.0f;
    private float cycleTimer = 0.0f;
    private int cycleShots = 0;

    protected override void Start()
    {
        base.Start();
        shotTimer = delayPerShot;
        cycleTimer = 0.0f;
        cycleShots = 0;
    }

    protected override void Action()
    {
        if(cycleTimer <= 0.0f)
        {
            if(shotTimer <= 0.0f)
            {
                if(doDelayBetweenShooters)
                {
                    StartCoroutine("ShootButDelayBetweenEachShooter");
                }
                else
                {
                    for(int i = 0; i < muzzleTransforms.Length; i++)
                    {
                        ObjectPooler.instance.SpawnFromPool(projectileTag, muzzleTransforms[i].position, muzzleTransforms[i].rotation);
                        ObjectPooler.instance.SpawnFromPool(muzzleFlashTag, muzzleTransforms[i].position, muzzleTransforms[i].rotation);
                    }
                    SoundFXManager.PlayOneShot(SoundFxKey.BCOMP_SHOOT, audioSource);
                }
                cycleShots--;

                if(delayPerCycle > 0 && cycleShots <= 0)
                {
                    cycleTimer = delayPerCycle;
                    cycleShots = shotsPerCycle;

                    Vector3 rotation = transform.rotation.eulerAngles;
                    rotation.y += yRotatePerCycle;
                    transform.rotation = Quaternion.Euler(rotation);
                }
                else
                {
                    shotTimer = delayPerShot;

                    Vector3 rotation = transform.rotation.eulerAngles;
                    rotation.y += yRotatePerShot;
                    transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                shotTimer -= Time.deltaTime;
            }
        }
        else
        {
            cycleTimer -= Time.deltaTime;
        }
    }

    private IEnumerator ShootButDelayBetweenEachShooter()
    {
        for(int i = 0; i < muzzleTransforms.Length; i++)
        {
            ObjectPooler.instance.SpawnFromPool(projectileTag, muzzleTransforms[i].position, muzzleTransforms[i].rotation);
            ObjectPooler.instance.SpawnFromPool(muzzleFlashTag, muzzleTransforms[i].position, muzzleTransforms[i].rotation);
            SoundFXManager.PlayOneShot(shotSound, audioSource);
            yield return new WaitForSeconds(delayPerShooter);

            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y += yRotatePerShooter;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}