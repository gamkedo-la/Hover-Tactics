using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] float forwardSpeed;
    [SerializeField] float backwardSpeed;
    [SerializeField] float sideSpeed;
    [Space]
    [SerializeField] float turnSpeed;
    [SerializeField] float turnSensitivity;
    [Space]
    [SerializeField] GameObject Cursor;

    private Rigidbody rb;

    private float horizontal;
    private float vertical;
    private float turnDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void HandleMovementInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void HandleMouseInput()
    {
        Vector3 targetPoint = Cursor.transform.position;
        Vector3 relativeDirection = transform.InverseTransformPoint(targetPoint);
        turnDir = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg * turnSensitivity;
        turnDir = Mathf.Clamp(turnDir, -1, 1);
    }

    private void Move()
    {
        Vector3 rightDirection = transform.right;
        rightDirection.y = 0.0f;
        Vector3 movement =
            (transform.forward * vertical * (vertical > 0 ? forwardSpeed : backwardSpeed)) +
            (rightDirection * horizontal * sideSpeed);
        rb.velocity = movement;
    }

    private void Turn()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += turnDir * turnSpeed * Time.deltaTime;
        rb.MoveRotation(Quaternion.Euler(rotation));
    }
}
