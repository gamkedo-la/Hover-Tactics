using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseMechController : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float backwardSpeed;
    [SerializeField] private float sideSpeed;
    [Header("Turning")]
    [SerializeField] protected HoverMechAnimation hoverMechAnimation;
    [SerializeField] private float turnSpeed;
    [SerializeField] protected float turnSensitivity;
    [Space]

    private Rigidbody rb;
    protected MechBoost mechBoost;

    protected float horizontal;
    protected float vertical;
    protected float turnDir;

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }

    public void Teleport(float value)
    {
        StopMovement();
        Vector3 position = transform.position;
        ObjectPooler.instance.SpawnFromPool("TeleportParticles", position, Quaternion.identity);
        float y = position.y;
        position += hoverMechAnimation.transform.forward * (value * 2.0f);
        position.y = y;
        transform.position = position;
        ObjectPooler.instance.SpawnFromPool("TeleportParticles", position, Quaternion.identity);
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        mechBoost = GetComponent<MechBoost>();

        Assert.IsNotNull(rb, "Rigidbody is null!");
        Assert.IsNotNull(mechBoost, "Mech Boost is null!");
    }
    
    private void Update()
    {
        SetInput();
        Turn();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void SetInput()
    {
        // empty on purpose, child classes need to implement it.
    }

    private void Move()
    {
        Vector3 rightDirection = hoverMechAnimation.transform.right;
        rightDirection.y = 0.0f;
        Vector3 movement =
            (hoverMechAnimation.transform.forward * vertical * (vertical > 0 ? forwardSpeed : backwardSpeed)) +
            (rightDirection * horizontal * sideSpeed);
        
        rb.velocity = (mechBoost == null) ? movement : movement * mechBoost.GetBoostValue();
    }

    private void Turn()
    {
        hoverMechAnimation.SetYRotation(hoverMechAnimation.GetYRotation() + turnDir * turnSpeed * Time.deltaTime);
    }
}
