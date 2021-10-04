using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
	[SerializeField] private string particleTag;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Health health = other.GetComponent<Health>();
			if (health != null) health.ChangeBy(1.0f);
			
			GameManager.instance.AddShieldToAll(1.0f);
			ObjectPooler.instance.SpawnFromPool(particleTag, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
