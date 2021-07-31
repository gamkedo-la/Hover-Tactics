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
    protected AudioSource audioSource;

    protected Transform GetShootingPoint()
    {
        return shootingPoints[shootingPointIndex];
    }

    protected void Start()
    {
        mechController = GetComponent<MechController>();
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(mechController, "Mech Controller is null!");
        Assert.IsNotNull(audioSource, "Audio Source is null!");
    }

    protected void Update()
    {
        if(mechController.enabled)
        {
            if(cooldownTimer <= 0.0f

            //TEMP
            && mechController.MP >= MPDepletePerShot)
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

                    //TEMP
                    mechController.MP -= MPDepletePerShot;

                    GameManager.instance.GetPlayerBars().UpdateMP(mechController.MP);

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

        //TEMP v
        if(mechController.MP < 1.0f) mechController.MP += Time.deltaTime / 5.0f;
        else mechController.MP = 1.0f;
        GameManager.instance.GetPlayerBars().UpdateMP(mechController.MP);
        //TEMP ^
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
