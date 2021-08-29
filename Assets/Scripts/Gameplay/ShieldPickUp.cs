using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
	[SerializeField] private string particleTag;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameManager.instance.AddShieldToAll(1.0f);
			ObjectPooler.instance.SpawnFromPool(particleTag, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
