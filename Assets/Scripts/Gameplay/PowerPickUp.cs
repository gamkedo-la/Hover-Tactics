using UnityEngine;

public class PowerPickUp : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Power power = other.GetComponent<Power>();
			if (power != null) {
				power.ChangeBy(1.0f);
				power.ChangeBy_Special(3);
			}
			Destroy(gameObject);
		}
	}
}
