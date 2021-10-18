using UnityEngine;
using UnityEngine.Assertions;

//Subclasses: RaycastWeapon, ProjectileWeapon, PortalWeapon, BeamWeapon
[RequireComponent(typeof(BaseMechController))]
public class Weapon : MonoBehaviour, IDamage
{
    public enum Type {
        BASIC,
        SPECIAL
    };

    [Header("Properties")]
    [SerializeField] protected Type type = Type.BASIC;
    [SerializeField] protected Transform[] shootingPoints;
    [SerializeField] protected bool rotateShootingPoints = true;
    [SerializeField] protected float cooldownDelay = 0.1f;
    [SerializeField] protected bool cooldownPowerDependence = false;
    [SerializeField] protected float cooldownPowerRatio = 1.0f;
    [SerializeField] protected float MPDepletePerShot = 0.1f;

    [Header("Effects")]
    [SerializeField] protected string muzzleParticlesTag;
    [SerializeField] protected SoundFxKey sound;

    protected int shootingPointIndex = 0;
    protected float cooldownTimer = 0.0f;

    protected BaseMechController mechController;
    protected Power power;
    protected AudioSource audioSource;

    protected Transform GetShootingPoint()
    {
        return shootingPoints[shootingPointIndex];
    }

    protected virtual void Start()
    {
        mechController = GetComponent<BaseMechController>();
        power = GetComponent<Power>();
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(mechController, "Mech Controller is null!");
        Assert.IsNotNull(power, "Power is null!");
        Assert.IsNotNull(audioSource, "Audio Source is null!");
    }

    protected virtual void Update()
    {
        if(mechController.enabled && Time.timeScale > 0.0f)
        {
            if(cooldownTimer <= 0.0f && power.Get() >= MPDepletePerShot)
            {
                if((type == Type.BASIC
                    && ((Input.GetButton("Fire1") && !GameManager.instance.touch)
                    || (mechController.IsTouchAttacking() && GameManager.instance.touch)))
                || (type == Type.SPECIAL && power.GetSpecials() > 0
                    && ((Input.GetButtonDown("Fire2") && !GameManager.instance.touch)
                    || (GameManager.instance.useSpecial > 0.0f && GameManager.instance.touch))))
                {
                    if(rotateShootingPoints)
                    {
                        GetShootingPoint().LookAt(CameraController.singleton.lastAimPoint);
                        Vector3 shootingPointEuler = GetShootingPoint().localRotation.eulerAngles;
                        shootingPointEuler.y = 0;
                        shootingPointEuler.z = 0;
                        GetShootingPoint().localRotation = Quaternion.Euler(shootingPointEuler);
                    }

                    Fire();
                    Effects();

                    power.ChangeBy(-MPDepletePerShot);
                    if(type == Type.SPECIAL)
                    {
                        if(GameManager.instance.touch) GameManager.instance.useSpecial = 0.0f;
                        power.ChangeBy_Special(-1);
                    }

                    shootingPointIndex++;
                    if(shootingPointIndex > shootingPoints.Length - 1)
                        shootingPointIndex = 0;

                    if(cooldownPowerDependence)
                    {
                        cooldownTimer = cooldownDelay / (power.Get() * cooldownPowerRatio);
                        if(cooldownTimer > 1.0f) cooldownTimer = 1.0f;
                    }
                    else
                    {
                        cooldownTimer = cooldownDelay;
                    }
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }

            if(type == Type.SPECIAL
            && GameManager.instance.touch)
                GameManager.instance.useSpecial -= Time.deltaTime;
        }
    }

    protected virtual void Fire() {}

    public virtual Damage GetDamage() => new Damage() { Value = 0 };
    
    protected void Effects()
    {
        ObjectPooler.instance.SpawnFromPool(
            muzzleParticlesTag,
            GetShootingPoint().position,
            GetShootingPoint().rotation
        );
        SoundFXManager.PlayOneShot(sound);//, audioSource);
    }
}
