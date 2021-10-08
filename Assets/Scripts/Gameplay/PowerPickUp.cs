using UnityEngine;

public class PowerPickUp : MonoBehaviour
{
	[SerializeField] private string particleTag;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Health health = other.GetComponent<Health>();
			if (health != null) health.ChangeBy(1.0f);
			
			Power power = other.GetComponent<Power>();
			if (power != null)
			{
				power.ChangeBy(1.0f);
				power.ChangeBy_Special(3);
			}
			SoundFXManager.PlayOneShot(SoundFxKey.PICKUP);
			ObjectPooler.instance.SpawnFromPool(particleTag, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
