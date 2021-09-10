using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//Building Components are supposed to be childs of Buildings
//or any game object that has Health and AudioSource components with them

//Subclasses of this class have 'BComp' suffix in the end of their class names

public class BuildingComponent : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    public float destroyBelowHealth = 0.25f;

    [Header("Show Properties")]
    [SerializeField] private Vector3 hideOffset = Vector3.zero;
    [SerializeField] private bool startHidden = true;
    [SerializeField] private bool autoHide = true;
    [SerializeField] private float showDelay = 3.0f;

    [Header("Show Detection")]
    [SerializeField] private float detectionDistance = 20.0f;
    [SerializeField] private bool showOnlyOnDamage = false;

    [Header("Show Lerps")]
    [SerializeField] private float showLerpFactor = 20.0f;
    [SerializeField] private float hideLerpFactor = 10.0f;

    [Header("Show Effects")]
    [SerializeField] private SoundFxKey showUpSound;
    [SerializeField] private string showUpParticlesTag;

    [Header("Destroy Effects")]
    [SerializeField] private SoundFxKey explosionSound;
    [SerializeField] private string explosionTag;

    protected Health health;
    protected AudioSource audioSource;
    private Vector3 initialPosition;
    protected GameObject[] targetObjects;

    private float prevHealth;
    private float showTimer;
    private int targetIndex;

    protected GameObject GetTarget()
    {
        return targetObjects[targetIndex];
    }

    protected virtual void Start()
    {
        health = transform.parent.GetComponent<Health>();
        audioSource = transform.parent.GetComponent<AudioSource>();
        initialPosition = transform.position;

        Assert.IsNotNull(health, "Health is null!");
        Assert.IsNotNull(audioSource, "Audio Source is null!");

        if(startHidden)
        {
            showTimer = 0.0f;
            transform.position = initialPosition + hideOffset;
        }
        else
        {
            showTimer = showDelay;
        }

        targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        prevHealth = health.Get();
    }

    private void Update()
    {
        if(health.Get() != prevHealth)
        {
            showTimer = showDelay;
        }
        else if(!showOnlyOnDamage)
        {
            for(int i = 0; i < targetObjects.Length; i++)
            {
                if(Vector3.Distance(transform.parent.position, targetObjects[i].transform.position) <= detectionDistance)
                {
                    targetIndex = i;
                    showTimer = showDelay;
                    break;
                }
            }
        }

        if(showTimer > 0.0f)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, showLerpFactor * Time.deltaTime);
            Action();
            
            if(autoHide) showTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition + hideOffset, hideLerpFactor * Time.deltaTime);
        }

        if(health.Get() <= destroyBelowHealth)
        {
            SoundFXManager.PlayOneShot(explosionSound, audioSource);
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        prevHealth = health.Get();
    }

    protected virtual void Action() {}

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, detectionDistance);
    }
}
