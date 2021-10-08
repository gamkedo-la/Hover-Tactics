using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]
public class EnemyMechController : BaseMechController
{
    [Range(0.0f, 5.0f)]
    public float nearDistanceThreshold = 1f;
    [Range(-1.0f, 1.0f)]
    public float facingTargetTolerance = 0.8f;

    [Header("Destroy Effects")]
    [SerializeField] private string explosionTag;

    private Vector3 directionToTarget;
    private Transform _actualTarget;
    private Transform positionTarget => _actualTarget == null ? this.transform : this._actualTarget;
    private float sidewaysMovementFilter = 0.3f;
    private float minimumFowardMovement = 0.8f;

    protected override void SetInput()
    {
        if(positionTarget == null)
        {
            return;
        }

        if(IsNearTarget())
        {   
            StopMoving();
            StopTurning();
        }
        else
        {
            SetTurnValues();
            SetMovementValues();
        }
    }

    public void SetTarget(Transform target)
    {
        this._actualTarget = target;
    }

    public Transform GetTarget()
    {
        return this._actualTarget;
    }

    public float GetDistanceFromTarget()
    {
        return Mathf.Abs(Vector3.Distance(this.transform.position, positionTarget.position));
    }

    public bool IsFacingTarget()
    {
        var dot = Vector3.Dot(hoverMechAnimation.transform.forward, (positionTarget.position - transform.position).normalized);
        return  dot > facingTargetTolerance;
    }

    public bool IsNearTarget()
    {
        var distance = GetDistanceFromTarget();
        var isNear = distance < nearDistanceThreshold;
        return isNear;
    }

    private void StopMoving()
    {
        horizontal = vertical = 0f;
    }

    private void StopTurning()
    {
        turnDir = 0f;
    }

    private void SetMovementValues()
    {
        directionToTarget = (positionTarget.position - this.transform.position).normalized;

        // limiting the side movement to allow for a more natural movement
        // less side ways, thus more towards the target
        horizontal = Mathf.Abs(directionToTarget.x * sidewaysMovementFilter);

        // making sure it doesn't slow down much by clamping the values
        vertical = Mathf.Clamp(Mathf.Abs(directionToTarget.z), minimumFowardMovement, 1.0f); 
    }

    private void SetTurnValues()
    {
        Vector3 relativeDirection = hoverMechAnimation.transform.InverseTransformPoint(positionTarget.position);
        turnDir = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg * turnSensitivity;
        turnDir = Mathf.Clamp(turnDir, -1, 1);
    }

    protected override void Start()
    {
        base.Start();

        health = GetComponent<Health>();
    }

    protected override void Update()
    {
        base.Update();

        if(health.IsZero())
        {
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
