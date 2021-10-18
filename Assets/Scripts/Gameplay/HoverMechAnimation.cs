using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMechAnimation : MonoBehaviour
{
    [SerializeField] private float lerpFactor = 4.0f;
    [SerializeField] private float forwardRotation = 25.0f;
    [SerializeField] private float backwardRotation = 15.0f;
    [SerializeField] private float sideRotation = 20.0f;

    private BaseMechController controller;
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
        controller = transform.parent.gameObject.GetComponent<BaseMechController>();
    }

    void Update()
    {
        if(controller.enabled)
        {
            Quaternion quaternionRotation = transform.rotation;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = vertical * (vertical > 0 ? forwardRotation : backwardRotation);
            rotation.y = 180.0f + yRotation;
            rotation.z = horizontal * -sideRotation;
            quaternionRotation = Quaternion.Euler(rotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternionRotation, Time.deltaTime * lerpFactor);
        }
    }
}
