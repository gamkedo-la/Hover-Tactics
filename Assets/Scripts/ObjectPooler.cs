using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
	
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}
	
	public List<Pool> pools;
	public Dictionary <string, Queue<GameObject>> poolDictionary;
	
	public static ObjectPooler instance;
	void Awake()
	{
		//Singleton code
		instance = this;
		///////////////
		
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
			
			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	//The above code not in start because of the other code asking for objects!
	void Start () {

	}
	
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		GameObject objectToSpawn = poolDictionary[tag].Dequeue();
		
		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		Rigidbody rigidbody = objectToSpawn.GetComponent<Rigidbody>();
		if(rigidbody)
		{
			rigidbody.velocity = rigidbody.angularVelocity = Vector3.zero;
		}
	
		poolDictionary[tag].Enqueue(objectToSpawn);
		
		return objectToSpawn;
	}
}
