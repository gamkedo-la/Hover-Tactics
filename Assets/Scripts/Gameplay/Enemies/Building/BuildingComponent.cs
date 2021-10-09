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
    [HideInInspector] public float destroyBelowHealth = 0.25f;

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
    [SerializeField] private string explosionTag;
    [SerializeField] private float explosionScale = 1.0f;

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
            if(showTimer <= 0.0f) SoundFXManager.PlayOneShot(SoundFxKey.BCOMP_SHOW, audioSource);
            showTimer = showDelay;
        }
        else if(!showOnlyOnDamage)
        {
            for(int i = 0; i < targetObjects.Length; i++)
            {
                if(targetObjects[i].GetComponent<MechController>().enabled
                && Vector3.Distance(transform.parent.position, targetObjects[i].transform.position) <= detectionDistance)
                {
                    if(showTimer <= 0.0f) SoundFXManager.PlayOneShot(SoundFxKey.BCOMP_SHOW, audioSource);
                    targetIndex = i;
                    showTimer = showDelay;
                    break;
                }
            }
        }

        if(showTimer > 0.0f)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, showLerpFactor * Time.deltaTime);
            if(Vector3.Distance(transform.position, initialPosition) <= 2.0f) Action();
            
            if(autoHide) showTimer -= Time.deltaTime;
        }
        else
        {
            if(showTimer <= 0.0f && showTimer > -2.0f)
            {
                SoundFXManager.PlayOneShot(SoundFxKey.BCOMP_HIDE, audioSource);
                showTimer = -10.0f;
            }

            transform.position = Vector3.Lerp(transform.position, initialPosition + hideOffset, hideLerpFactor * Time.deltaTime);
        }

        if(health.Get() <= destroyBelowHealth)
        {
            SoundFXManager.PlayOneShot(SoundFxKey.BCOMP_DESTROY, audioSource);
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, transform.rotation).transform.localScale = Vector3.one * explosionScale;
            Destroy(gameObject);
        }

        prevHealth = health.Get();
    }

    protected virtual void Action() {}
}
