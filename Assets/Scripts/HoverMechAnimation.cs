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

            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = Mathf.Lerp(rotation.x, 45.0f + (vertical * maxMoveRotation), Time.deltaTime * lerpFactor);
            rotation.z = Mathf.Lerp(rotation.z, 45.0f + (horizontal * -maxMoveRotation), Time.deltaTime * lerpFactor);
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
