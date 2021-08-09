using UnityEngine;
using UnityEngine.Assertions;

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
    [SerializeField] GameObject Cursor;

    private Rigidbody rb;
    private MechBoost mechBoost;

    private float horizontal;
    private float vertical;
    private float turnDir;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mechBoost = GetComponent<MechBoost>();

        Assert.IsNotNull(rb, "Rigidbody is null!");
        Assert.IsNotNull(mechBoost, "Mech Boost is null!");
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

        if(Input.GetKeyDown(KeyCode.Space))
            mechBoost.ActivateBoost();
        if(Input.GetKeyUp(KeyCode.Space))
            mechBoost.DeactivateBoost();
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
        
        rb.velocity = (mechBoost == null) ? movement : movement * mechBoost.GetBoostValue();
    }

    private void Turn()
    {
        hoverMechAnimation.SetYRotation(hoverMechAnimation.GetYRotation() + turnDir * turnSpeed * Time.deltaTime);
    }
}
