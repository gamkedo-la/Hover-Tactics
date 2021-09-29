using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(EnemyMechController))]
public class EnemyFollowWayPointsRockets : MonoBehaviour
{
    [Header("Waypoints")]
    public WayPoints wayPoints;
    public float minDistanceToTarget = 1f;
    public string targetTag = "Player";
    public float targetDetectDistance = 30.0f;

    [Header("Properties")]
    [SerializeField] private Transform shootingCore;
    [SerializeField] private Transform[] shootingPoints;
    [SerializeField] private float shootingCoreRotationFactor = 2.0f;
    [SerializeField] private float cooldownDelay = 0.1f;

    [Header("Projectile")]
    [SerializeField] private string projectileTag;

    [Header("Effects")]
    [SerializeField] protected string muzzleParticlesTag;
    [SerializeField] protected SoundFxKey sound;

    private int currentWayPointIndex = 0;
    private EnemyMechController enemyMechController;
    private GameObject[] targetObjects;
    private int targetStatus = -1;

    protected int shootingPointIndex = 0;
    protected float cooldownTimer = 0.0f;

    protected AudioSource audioSource;

    private void Start()
    {
        enemyMechController = GetComponent<EnemyMechController>();
        targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "Audio Source is null!");
    }

    private Transform GetShootingPoint()
    {
        return shootingPoints[shootingPointIndex];
    }

    private void Update()
    {
        bool gotTarget = false;
        for(int i = 0; i < targetObjects.Length; i++)
        {
            if(targetObjects[i].GetComponent<MechController>().enabled
            && Vector3.Distance(
                targetObjects[i].transform.position,
                wayPoints.GetWayPointAt(currentWayPointIndex).position)
            <= targetDetectDistance)
            {
                enemyMechController.SetTarget(targetObjects[i].transform);
                targetStatus = 1;
                gotTarget = true;
                break;
            }
        }
        if(targetStatus == 1)
        {
            if(!gotTarget)
            {
                targetStatus = -1;
            }
            else
            {
                Quaternion currentRotation = shootingCore.transform.rotation;
                shootingCore.LookAt(enemyMechController.GetTarget());
                Quaternion targetRotation = shootingCore.transform.rotation;
                shootingCore.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, shootingCoreRotationFactor * Time.deltaTime);
                Attack();
            }
        }

        if(targetStatus != 1)
        {
            if(targetStatus == -1)
            {
                enemyMechController.SetTarget(wayPoints.GetWayPointAt(currentWayPointIndex));
                targetStatus = 0;
            }
            else if(enemyMechController.IsNearTarget())
            {
                currentWayPointIndex = wayPoints.GetNextIndex(currentWayPointIndex);
                enemyMechController.SetTarget(wayPoints.GetWayPointAt(currentWayPointIndex));
            }
        }
    }
    
    void Attack()
    {
        if(cooldownTimer <= 0.0f)
        {
            Fire();
            Effects();

            shootingPointIndex++;
            if(shootingPointIndex > shootingPoints.Length - 1)
                shootingPointIndex = 0;

            cooldownTimer = cooldownDelay;
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void Fire()
    {
        ObjectPooler.instance.SpawnFromPool(projectileTag, GetShootingPoint().position, GetShootingPoint().rotation);
    }

    void Effects()
    {
        ObjectPooler.instance.SpawnFromPool(
            muzzleParticlesTag,
            GetShootingPoint().position,
            GetShootingPoint().rotation
        );
        SoundFXManager.PlayOneShot(sound, audioSource);
    }

}
