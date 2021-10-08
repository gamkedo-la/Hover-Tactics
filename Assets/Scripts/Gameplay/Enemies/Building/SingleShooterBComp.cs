using UnityEngine;

class SingleShooterBComp : BuildingComponent
{
    [Header("Aim")]
    [SerializeField] private bool aimAtTarget = true;
    [SerializeField] private float aimLerpFactor = 5.0f;
    [SerializeField] private bool yRotationOnly = true;
    [SerializeField] private bool shootOnlyOnAim = true;
    [SerializeField] private float aimThresholdInDegrees = 20.0f;

    [Header("Timers")]
    [SerializeField] private float delayPerShot = 0.25f;
    [SerializeField] private int shotsPerCycle = 1;
    [SerializeField] private float delayPerCycle = 2.0f;

    [Header("Muzzle")]
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private string muzzleFlashTag;

    [Header("Projectile")]
    [SerializeField] private string projectileTag;

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

    private bool IsLookingAtTarget()
    {
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(GetTarget().transform);
        Quaternion targetRotation = transform.rotation;
        transform.rotation = currentRotation;
        float angle = Quaternion.Angle(currentRotation, targetRotation);
        if(yRotationOnly)
        {
            currentRotation = Quaternion.Euler(0.0f, currentRotation.eulerAngles.y, 0.0f);
            targetRotation = Quaternion.Euler(0.0f, targetRotation.eulerAngles.y, 0.0f);
        }
        return Mathf.Abs(angle) < aimThresholdInDegrees;
    }

    protected override void Action()
    {
        if(aimAtTarget)
        {
            Quaternion currentRotation = transform.rotation;
            transform.LookAt(GetTarget().transform);
            Quaternion targetRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, aimLerpFactor * Time.deltaTime);
            if(yRotationOnly) transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        }

        if(GetTarget().GetComponent<MechController>().enabled
        && (!shootOnlyOnAim || IsLookingAtTarget()))
        {
            if(cycleTimer <= 0.0f)
            {
                if(shotTimer <= 0.0f)
                {
                    ObjectPooler.instance.SpawnFromPool(projectileTag, muzzleTransform.position, muzzleTransform.rotation);
                    ObjectPooler.instance.SpawnFromPool(muzzleFlashTag, muzzleTransform.position, muzzleTransform.rotation);
                    SoundFXManager.PlayOneShot(SoundFxKey.BCOMP_SHOOT, audioSource);
                    cycleShots--;

                    if(delayPerCycle > 0 && cycleShots <= 0)
                    {
                        cycleTimer = delayPerCycle;
                        cycleShots = shotsPerCycle;
                    }
                    else
                    {
                        shotTimer = delayPerShot;
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
    }
}