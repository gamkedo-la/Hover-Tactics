using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(EnemyMechController))]
public class EnemyFollowWayPoints : MonoBehaviour
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

    [Header("Raycast")]
    [SerializeField] private float damagePerHit = 0.05f;
    [SerializeField] private string hitImpactTag;
    [SerializeField] private float lineDelay = 0.05f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineThickness = 0.5f;

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

    private List<LineRenderer> lineRenderers;
    private List<float> lineTimers;

    private float resetRecoil;

    private void Start()
    {
        enemyMechController = GetComponent<EnemyMechController>();
        targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "Audio Source is null!");

        //Adding Line Renderer for each Shooting Point to render Raycast Shooting
        lineRenderers = new List<LineRenderer>();
        lineTimers = new List<float>();
        for(int i = 0; i < shootingPoints.Length; i++)
        {
            GameObject lineObject = new GameObject();
            lineObject.transform.parent = gameObject.transform;
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineRenderer.endWidth = lineThickness;
            lineRenderers.Add(lineRenderer);
            lineTimers.Add(0.0f);
        }

        if(shootingPoints.Length > 0)
            resetRecoil = shootingPoints[0].parent.localPosition.z;
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

        //Decreasing Alpha (and ultimately, disabling) Line Renderer as the time passes for each Shooting Point
        for(int i = 0; i < shootingPoints.Length; i++)
        {
            Vector3 localShooterPosition = shootingPoints[i].parent.localPosition;
            localShooterPosition.z = Mathf.Lerp(localShooterPosition.z, resetRecoil, 20.0f * Time.deltaTime);
            shootingPoints[i].parent.localPosition = localShooterPosition;

            if(lineTimers[i] <= 0.0f)
            {
                lineRenderers[i].enabled = false;
                Color color = lineRenderers[i].startColor;
                color.a = 1.0f;
                lineRenderers[i].startColor = lineRenderers[i].endColor = color;
            }
            else
            {
                Color color = lineRenderers[i].startColor;
                color.a = lineTimers[i] / lineDelay;
                lineRenderers[i].startColor = lineRenderers[i].endColor = color;
                lineTimers[i] -= Time.deltaTime;
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

    Damage GetDamage() => new Damage() { Value = -damagePerHit };

    void Fire()
    {
        lineRenderers[shootingPointIndex].SetPosition(0, GetShootingPoint().position);
        lineRenderers[shootingPointIndex].SetPosition(1, GetShootingPoint().position + (GetShootingPoint().forward * 1000.0f));
        lineRenderers[shootingPointIndex].enabled = true;
        lineTimers[shootingPointIndex] = lineDelay;

        Vector3 localShooterPosition = GetShootingPoint().parent.localPosition;
        localShooterPosition.z -= 0.1f;
        GetShootingPoint().parent.localPosition = localShooterPosition;

        RaycastHit hitData;
        if (Physics.Raycast(GetShootingPoint().position, GetShootingPoint().forward, out hitData))
        {
            lineRenderers[shootingPointIndex].SetPosition(1, hitData.point);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitData.normal);
            ObjectPooler.instance.SpawnFromPool(hitImpactTag, hitData.point, rot);

            if(hitData.transform.gameObject.layer == 10) return; //NoDamage

            AbstractTakeDamage canTakeDamage = hitData.transform.GetComponent<AbstractTakeDamage>();
            if(canTakeDamage) canTakeDamage.TakeDamage(GetDamage());
        }
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
