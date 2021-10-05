using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechManager : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnPointProperties
    {
        public Transform waypoint;
        public bool isActive;
        public float buildingDependenceDistance;
    }

    [System.Serializable]
    public struct EnemyManageProperties
    {
        public GameObject mech;
        public int amountLimit;
        public float minSpawnDelay;
        public float maxSpawnDelay;
        public SpawnPointProperties[] wayPointSpawnProperty;
        public float spawnTimer;
    };

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
