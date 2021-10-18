using UnityEngine;

public class MechController : BaseMechController
{
    protected override void SetInput()
    {
        HandleMovementInput();
        HandleMouseInput();
    }

    private void HandleMovementInput()
    {
        if(GameManager.instance.touch && movementStick)
        {
            horizontal = movementStick.Horizontal;
            vertical = movementStick.Vertical;

            if(IsTouchBoosting())
                mechBoost.ActivateBoost();
            else
                mechBoost.DeactivateBoost();
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if(Input.GetKeyDown(KeyCode.Space))
                mechBoost.ActivateBoost();
            if(Input.GetKeyUp(KeyCode.Space))
                mechBoost.DeactivateBoost();
        }
    }

    private void HandleMouseInput()
    {
        Vector3 targetPoint = Cursor.transform.position;
        Vector3 relativeDirection = hoverMechAnimation.transform.InverseTransformPoint(targetPoint);
        turnDir = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg * turnSensitivity;
        turnDir = Mathf.Clamp(turnDir, -1, 1);
    }
}
