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
        [Range(0.0f, 1.0f)] public float minDestroyHealth;
        [Range(0.0f, 1.0f)] public float maxDestroyHealth;
        public float minHeightVariation;
        public float maxHeightVariation;

        [HideInInspector] public List<int> usedTransformIndexes;
    };

    [Space]
    [SerializeField] private BuildingComponentParams[] bComps;

    private Health health;
    private AudioSource audioSource;

    void GenerateBComps()
    {
        for(int i = 0; i < bComps.Length; i++)
        {
            bComps[i].usedTransformIndexes = new List<int>();

            while(bComps[i].count > 0)
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

                    bComps[i].usedTransformIndexes.Add(randomTransformIndex);
                    bComps[i].count--;
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
