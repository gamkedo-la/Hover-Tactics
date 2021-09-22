using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
	
	[System.Serializable]
	public class Pool
	{
		public GameObject prefab;
		public int size;
	}
	
	public List<Pool> pools;
	public Dictionary <string, Queue<GameObject>> poolDictionary;
	
	public static ObjectPooler instance;
	void Awake()
	{
		instance = this;
		poolDictionary = new Dictionary<string, Queue<GameObject>>();
		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();
			for(int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}
			poolDictionary.Add(pool.prefab.name, objectPool);
		}
	}
	
	public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
	{
		if(name == "") return null;
		GameObject objectToSpawn = poolDictionary[name].Dequeue();
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;
		Rigidbody rigidbody = objectToSpawn.GetComponent<Rigidbody>();
		if(rigidbody) rigidbody.velocity = rigidbody.angularVelocity = Vector3.zero;
		poolDictionary[name].Enqueue(objectToSpawn);
		objectToSpawn.SetActive(true);
		return objectToSpawn;
	}
}
