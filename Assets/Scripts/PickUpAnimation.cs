using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAnimation : MonoBehaviour
{
    public float rotationSpeed;
    public float floatInterval;
    public float floatSpeed;

    private float height = 0f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position = initialPosition + (Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatInterval);
        transform.Rotate(0f, 0f, rotationSpeed);
    }
}
