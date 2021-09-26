using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]
public class Building : MonoBehaviour
{
    [SerializeField] private GameObject ruins;
    [SerializeField] private SoundFxKey explosionSound;
    [SerializeField] private string explosionTag;

    [System.Serializable]
    public struct BuildingComponentParams
    {
        public string name;
        public GameObject type;
        public int count;
        public Transform[] transforms;
        public float minDestroyHealth;
        public float maxDestroyHealth;
        public float minHeightVariation;
        public float maxHeightVariation;

        [HideInInspector] public List<int> usedTransformIndexes;
    };

    [Space]
    [SerializeField] private BuildingComponentParams[] bComps;

    private List<GameObject> bCompReferences = null;

    private Health health;
    private AudioSource audioSource;
    
    [ContextMenu("Clear BComps")]
    void ClearBComps()
    {
        for(int i = 0; i < bComps.Length; i++)
        {
            if(bComps[i].usedTransformIndexes == null)
                bComps[i].usedTransformIndexes = new List<int>();
            else
                bComps[i].usedTransformIndexes.Clear();
        }

        if(bCompReferences == null)
        {
            bCompReferences = new List<GameObject>();
        }
        else
        {
            foreach(GameObject bObj in bCompReferences)
                DestroyImmediate(bObj);
            bCompReferences.Clear();
        }
    }

    [ContextMenu("Generate All Possible BComps")]
    void GenerateAllPossibleBComps()
    {
        ClearBComps();
        for(int i = 0; i < bComps.Length; i++)
        {
            for(int b = 0; b < bComps[i].transforms.Length; b++)
            {
                GameObject bCompObject = Instantiate(
                    bComps[i].type,
                    bComps[i].transforms[b].position
                        + new Vector3(0.0f, UnityEngine.Random.Range(bComps[i].minHeightVariation, bComps[i].maxHeightVariation), 0.0f),
                    bComps[i].transforms[b].rotation
                );
                bCompObject.transform.parent = transform;
                bCompObject.GetComponent<BuildingComponent>().destroyBelowHealth = UnityEngine.Random.Range(bComps[i].minDestroyHealth, bComps[i].maxDestroyHealth);
                bCompReferences.Add(bCompObject);
            }
        }
    }

    [ContextMenu("Generate Random BComps")]
    void GenerateBComps()
    {
        ClearBComps();
        for(int i = 0; i < bComps.Length; i++)
        {
            int tempCount = bComps[i].count;
            while(tempCount > 0)
            {
                int randomTransformIndex = UnityEngine.Random.Range(0, bComps[i].transforms.Length);

                if(!bComps[i].usedTransformIndexes.Contains(randomTransformIndex))
                {
                    GameObject bCompObject = Instantiate(
                        bComps[i].type,
                        bComps[i].transforms[randomTransformIndex].position
                            + new Vector3(0.0f, UnityEngine.Random.Range(bComps[i].minHeightVariation, bComps[i].maxHeightVariation), 0.0f),
                        bComps[i].transforms[randomTransformIndex].rotation
                    );
                    bCompObject.transform.parent = transform;
                    bCompObject.GetComponent<BuildingComponent>().destroyBelowHealth = UnityEngine.Random.Range(bComps[i].minDestroyHealth, bComps[i].maxDestroyHealth);
                    bCompReferences.Add(bCompObject);

                    bComps[i].usedTransformIndexes.Add(randomTransformIndex);
                    tempCount--;
                }
            }
        }
    }

    void Start()
    {
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        GenerateBComps();
    }

    void Update()
    {
        if(health.IsZero())
        {
            SoundFXManager.PlayOneShot(explosionSound, audioSource);
            ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, Quaternion.identity);
            if(ruins) Instantiate(ruins, transform.position, ruins.transform.rotation);
            Destroy(gameObject);
        }
    }
}
