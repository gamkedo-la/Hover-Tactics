﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float backwardSpeed;
    [SerializeField] float sideSpeed;
    [Header("Turning")]
    [SerializeField] HoverMechAnimation hoverMechAnimation;
    [SerializeField] float turnSpeed;
    [SerializeField] float turnSensitivity;
    [Space]
    [SerializeField] MechBoost mechBoost;
    [SerializeField] GameObject Cursor;

    //TEMP
    public float MP = 1.0f;

    private Rigidbody rigidbody;

    private float horizontal;
    private float vertical;
    private float turnDir;

    public void StopMovement()
    {
        rigidbody.velocity = Vector3.zero;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        Turn();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleMovementInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(ShouldActivateBoost())
        {
            mechBoost.ActivateBoost();
        }
    }

    private bool ShouldActivateBoost()
    {
        return mechBoost?.IsBoostActive() == false && Input.GetKeyDown(KeyCode.Space);
    }

    private void HandleMouseInput()
    {
        Vector3 targetPoint = Cursor.transform.position;
        Vector3 relativeDirection = hoverMechAnimation.transform.InverseTransformPoint(targetPoint);
        turnDir = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg * turnSensitivity;
        turnDir = Mathf.Clamp(turnDir, -1, 1);
    }

    private void Move()
    {
        Vector3 rightDirection = hoverMechAnimation.transform.right;
        rightDirection.y = 0.0f;
        Vector3 movement =
            (hoverMechAnimation.transform.forward * vertical * (vertical > 0 ? forwardSpeed : backwardSpeed)) +
            (rightDirection * horizontal * sideSpeed);
        
        rigidbody.velocity = (mechBoost == null) ? movement : movement * mechBoost.GetBoostValue();
    }

    private void Turn()
    {
        hoverMechAnimation.SetYRotation(hoverMechAnimation.GetYRotation() + turnDir * turnSpeed * Time.deltaTime);
    }
}
