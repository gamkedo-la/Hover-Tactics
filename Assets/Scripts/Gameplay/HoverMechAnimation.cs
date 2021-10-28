using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMechAnimation : MonoBehaviour
{
    [SerializeField] private BaseMechController controller;
    [SerializeField] private float lerpFactor = 4.0f;
    [SerializeField] private float rotationLimit = 25.0f;

    private float yRotation = 180.0f;

    float horizontal = 0.0f;
    float vertical = 0.0f;

    public float GetYRotation()
    {
        return yRotation;
    }

    public void SetYRotation(float rot)
    {
        yRotation = rot;
    }

    public void SetHorizontalAndVertical(float h, float v)
    {
        horizontal = h;
        vertical = v;
    }

    void Start()
    {
    }

    void Update()
    {
        if(controller.enabled)
        {
            Quaternion qRot = transform.localRotation;
            Vector3 rot = transform.localRotation.eulerAngles;
            if(!GameManager.instance.twinShooterMovementMode)
            {
                rot.x = vertical * rotationLimit;
                rot.z = horizontal * -rotationLimit;
            }
            rot.y = 180.0f + yRotation;
            qRot = Quaternion.Euler(rot);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, qRot, Time.deltaTime * lerpFactor * AssistPanel.GetMovement());

            if(GameManager.instance.twinShooterMovementMode)
            {
                if(controller.GetForwardTransform() != null)
                {
                    qRot = controller.GetForwardTransform().rotation;
                    rot = controller.GetForwardTransform().rotation.eulerAngles;
                    rot.x = vertical * rotationLimit;
                    rot.z = horizontal * -rotationLimit;
                    qRot = Quaternion.Euler(rot);
                    transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, qRot, Time.deltaTime * lerpFactor * AssistPanel.GetMovement());
                }
            }
        }
    }
}
