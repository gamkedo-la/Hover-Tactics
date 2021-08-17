using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameManager.instance.AddShieldToAll(1.0f);
			Destroy(gameObject);
		}
	}
}
