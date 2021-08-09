using UnityEngine;
using UnityEngine.Assertions;

//Subclasses: RaycastWeapon, ProjectileWeapon, PortalWeapon, BeamWeapon
public class Weapon : MonoBehaviour
{
    public enum Type {
        BASIC,
        SPECIAL
    };

    [Header("Properties")]
    [SerializeField] protected Type type = Type.BASIC;
    [SerializeField] protected Transform[] shootingPoints;
    [SerializeField] protected float cooldownDelay = 0.1f;
    [SerializeField] protected float MPDepletePerShot = 0.1f;

    [Header("Effects")]
    [SerializeField] protected string muzzleParticlesTag;
    [SerializeField] protected SoundFxKey sound;

    protected int shootingPointIndex = 0;
    protected float cooldownTimer = 0.0f;

    protected MechController mechController;
    protected Power power;
    protected AudioSource audioSource;

    protected Transform GetShootingPoint()
    {
        return shootingPoints[shootingPointIndex];
    }

    protected virtual void Start()
    {
        mechController = GetComponent<MechController>();
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
                || (type == Type.SPECIAL && Input.GetButtonDown("Fire2")))
                {
                    GetShootingPoint().LookAt(CameraController.singleton.lastAimPoint);
                    Vector3 shootingPointEuler = GetShootingPoint().localRotation.eulerAngles;
                    shootingPointEuler.y = 0;
                    shootingPointEuler.z = 0;
                    GetShootingPoint().localRotation = Quaternion.Euler(shootingPointEuler);

                    Fire();
                    Effects();

                    power.ChangeBy(-MPDepletePerShot);

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
