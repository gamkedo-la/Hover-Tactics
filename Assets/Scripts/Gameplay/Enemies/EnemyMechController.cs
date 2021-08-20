using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechController : BaseMechController
{
    [Range(-1,1)]
    public float turnDirOverride;
    public Transform positionTarget;
    public float minDistanceToTarget = 1f;
    [Range(0.5f, 1.0f)]
    public float minAngleToTarget = 0.8f;
    private Vector3 directionToTarget;

    protected override void Start()
    {
        base.Start();
        SetTarget(this.transform); // in case there is no target
    }
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
        this.positionTarget = target;
    }

    public float GetDistanceFromTarget()
    {
        return Mathf.Abs(Vector3.Distance(this.transform.position, positionTarget.position));
    }

    public bool IsFacingTarget()
    {
        var dot = Vector3.Dot(hoverMechAnimation.transform.forward, (positionTarget.position - transform.position).normalized);
        return  dot > minAngleToTarget;
    }

    public bool IsNearTarget()
    {
        var distance = GetDistanceFromTarget();
        var isNear = distance < minDistanceToTarget;
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
        horizontal = Mathf.Abs(directionToTarget.x*0.3f);

        // making sure it doesn't slow down much by clamping the values
        vertical = Mathf.Clamp(Mathf.Abs(directionToTarget.z), 0.8f, 1.0f); 
    }

    private void SetTurnValues()
    {
        Vector3 targetPoint = positionTarget.position;
        Vector3 relativeDirection = hoverMechAnimation.transform.InverseTransformPoint(targetPoint);
        turnDir = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg * turnSensitivity;
        turnDir = Mathf.Clamp(turnDir, -1, 1);

        //turnDir = turnDirOverride;
    }
}
