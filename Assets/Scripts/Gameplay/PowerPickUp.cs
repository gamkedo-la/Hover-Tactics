using UnityEngine;

public class PowerPickUp : MonoBehaviour
{
	[SerializeField] private string particleTag;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Power power = other.GetComponent<Power>();
			if (power != null) {
				power.ChangeBy(1.0f);
				power.ChangeBy_Special(3);
			}
			ObjectPooler.instance.SpawnFromPool(particleTag, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
