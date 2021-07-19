using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMechAnimation : MonoBehaviour
{
    [SerializeField] private float lerpFactor = 4.0f;
    [SerializeField] private float maxMoveRotation = 25.0f;

    private SpaceshipController controller;

    void Start()
    {
        controller = transform.parent.gameObject.GetComponent<SpaceshipController>();
    }

    void Update()
    {
        if(controller.enabled)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Quaternion quaternionRotation = transform.rotation;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = vertical * maxMoveRotation;
            rotation.z = horizontal * -maxMoveRotation;
            quaternionRotation = Quaternion.Euler(rotation);

            transform.rotation = Quaternion.Slerp(transform.rotation, quaternionRotation, Time.deltaTime * lerpFactor);
        }
    }
}
