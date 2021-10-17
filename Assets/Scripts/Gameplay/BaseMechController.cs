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
    [Header("Sounds")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioSource moveAudioSource;
    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float boostPitch = 1.4f;
    [Header("Touch")]
    [SerializeField] protected FloatingJoystick movementStick = null;
    [SerializeField] protected FloatingJoystick rotationStick = null;
    [Space]

    private Rigidbody rb;
    protected MechBoost mechBoost;
    protected Health health;

    protected float horizontal;
    protected float vertical;
    protected float turnDir;
    private float previousHealth;

    public bool IsTouchBoosting()
    {
        return movementStick && (movementStick.Horizontal >= 1.0f || movementStick.Vertical >= 1.0f);
    }

    public bool IsTouchAttacking()
    {
        return rotationStick && (rotationStick.Horizontal >= 1.0f || rotationStick.Vertical >= 1.0f);
    }

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
        SoundFXManager.PlayOneShot(SoundFxKey.TELEPORT_BOOST);
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        mechBoost = GetComponent<MechBoost>();
        health = GetComponent<Health>();

        if(audioSource == null) audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(rb, "Rigidbody is null!");
        Assert.IsNotNull(health, "Health is null!");

        previousHealth = health.Get();

        audioSource.pitch = basePitch;
    }
    
    protected virtual void Update()
    {
        SetInput();
        Turn();
        ShakeOnDamage();

        audioSource.enabled = SoundFXManager.state;
        if(moveAudioSource) moveAudioSource.enabled = SoundFXManager.state;
    }

    protected virtual void FixedUpdate()
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
            (hoverMechAnimation.transform.forward * vertical * (vertical > 0 ? forwardSpeed : backwardSpeed) * AssistPanel.GetMovement()) +
            (rightDirection * horizontal * sideSpeed * AssistPanel.GetMovement());
        rb.velocity = (mechBoost == null) ? movement : movement * mechBoost.GetBoostValue();

        if(moveAudioSource == null) return;

        moveAudioSource.pitch = Mathf.Lerp(moveAudioSource.pitch, mechBoost.IsBoostActive() ? boostPitch : basePitch, Time.deltaTime * 8.0f);

        if(vertical != 0.0f || horizontal != 0.0f)
        {
            moveAudioSource.volume = Mathf.Lerp(moveAudioSource.volume, 1.0f, Time.deltaTime * 4.0f);
            audioSource.volume = 1.0f - moveAudioSource.volume;
        }
        else
        {
            moveAudioSource.volume = Mathf.Lerp(moveAudioSource.volume, 0.0f, Time.deltaTime * 8.0f);
            audioSource.volume = 1.0f - moveAudioSource.volume;
        }
    }

    private void Turn()
    {
        hoverMechAnimation.SetYRotation(hoverMechAnimation.GetYRotation() + turnDir * turnSpeed * AssistPanel.GetMovement() * Time.deltaTime);
    }

    private void ShakeOnDamage()
    {
        if(previousHealth > health.Get())
        {
            if(gameObject.tag == "Player")
            {
                SoundFXManager.PlayOneShot(SoundFxKey.MECH_DAMAGE);
                CameraShake.Shake(10.0f * (previousHealth - health.Get()), 1, 0.0f);
            }
            else
            {
                if(audioSource != null) SoundFXManager.PlayOneShot(SoundFxKey.MECH_DAMAGE, audioSource);
                CameraShake.Shake(1.0f * (previousHealth - health.Get()), 1, 0.0f);
            }
            previousHealth = health.Get();
        }
    }
}
