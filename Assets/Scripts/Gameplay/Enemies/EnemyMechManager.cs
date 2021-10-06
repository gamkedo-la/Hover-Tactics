using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechManager : MonoBehaviour
{
    [SerializeField] private string spawnParticleTag;

    [System.Serializable]
    public struct SpawnPointProperties
    {
        public WayPoints waypoints;
        public int amountLimit;
        [HideInInspector] public List<GameObject> mechs;
    }

    [System.Serializable]
    public struct EnemyManageProperties
    {
        public GameObject mech;
        public float minSpawnDelay;
        public float maxSpawnDelay;
        public SpawnPointProperties[] wayPointSpawnProperty;
        public float spawnTimer;
        [HideInInspector] public List<GameObject> mechs;
    };

    [SerializeField] private EnemyManageProperties[] properties;

    void Start()
    {
        
    }

    void Update()
    {
        for(int e = 0; e < properties.Length; e++)
        {
            if(properties[e].spawnTimer <= 0.0f)
            {
                //Making sure that destroyed mechs are removed from the lists
                if(properties[e].mechs != null)
                {
                    for(int em = 0; em < properties[e].mechs.Count; em++)
                    {
                        if(properties[e].mechs[em] == null) { properties[e].mechs.RemoveAt(em); em--; }
                        for(int empi = 0; empi < properties[e].wayPointSpawnProperty.Length; empi++)
                        {
                            if(properties[e].wayPointSpawnProperty[empi].mechs != null)
                            {
                                for(int empi_em = 0; empi_em < properties[e].wayPointSpawnProperty[empi].mechs.Count; empi_em++)
                                {
                                    if(properties[e].wayPointSpawnProperty[empi].mechs[empi_em] == null) { properties[e].wayPointSpawnProperty[empi].mechs.RemoveAt(empi_em); empi_em--; }
                                }
                            }
                        }
                    }
                }
                
                int spawnIndex = UnityEngine.Random.Range(0, properties[e].wayPointSpawnProperty.Length - 1);
                if(properties[e].wayPointSpawnProperty[spawnIndex].waypoints.gameObject.activeSelf
                && properties[e].wayPointSpawnProperty[spawnIndex].mechs.Count < properties[e].wayPointSpawnProperty[spawnIndex].amountLimit)
                {
                    GameObject newMech = GameObject.Instantiate(
                        properties[e].mech,
                        properties[e].wayPointSpawnProperty[spawnIndex].waypoints.GetWayPointAt(0).transform.position,
                        Quaternion.FromToRotation(
                            properties[e].wayPointSpawnProperty[spawnIndex].waypoints.GetWayPointAt(0).transform.position,
                            properties[e].wayPointSpawnProperty[spawnIndex].waypoints.GetWayPointAt(1).transform.position)
                    );
                    ObjectPooler.instance.SpawnFromPool(spawnParticleTag, newMech.transform.position, Quaternion.identity);

                    EnemyFollowWayPoints enemyFollow1 = newMech.GetComponent<EnemyFollowWayPoints>();
                    EnemyFollowWayPointsRockets enemyFollow2 = newMech.GetComponent<EnemyFollowWayPointsRockets>();
                    if(enemyFollow1 != null) enemyFollow1.wayPoints = properties[e].wayPointSpawnProperty[spawnIndex].waypoints;
                    if(enemyFollow2 != null) enemyFollow2.wayPoints = properties[e].wayPointSpawnProperty[spawnIndex].waypoints;

                    if(properties[e].mechs == null) properties[e].mechs = new List<GameObject>();
                    properties[e].mechs.Add(newMech);

                    if(properties[e].wayPointSpawnProperty[spawnIndex].mechs == null) properties[e].wayPointSpawnProperty[spawnIndex].mechs = new List<GameObject>();
                    properties[e].wayPointSpawnProperty[spawnIndex].mechs.Add(newMech);
                }

                properties[e].spawnTimer = UnityEngine.Random.Range(properties[e].minSpawnDelay, properties[e].maxSpawnDelay);
            }
            else
            {
                properties[e].spawnTimer -= Time.deltaTime;
            }
        }
    }
}
