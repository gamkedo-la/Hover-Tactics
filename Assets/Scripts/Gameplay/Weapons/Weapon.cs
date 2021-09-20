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
        if(mechController.enabled)
        {
            if(cooldownTimer <= 0.0f && power.Get() >= MPDepletePerShot)
            {
                if((type == Type.BASIC && Input.GetButton("Fire1"))
                || (type == Type.SPECIAL && power.GetSpecials() > 0 && Input.GetButtonDown("Fire2")))
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
                    if(type == Type.SPECIAL) power.ChangeBy_Special(-1);

                    shootingPointIndex++;
                    if(shootingPointIndex > shootingPoints.Length - 1)
                        shootingPointIndex = 0;

                    cooldownTimer = cooldownDelay;
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
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
        SoundFXManager.PlayOneShot(sound, audioSource);
    }
}
