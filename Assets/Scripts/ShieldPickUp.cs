using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Shield shieldScript = other.GetComponent<Shield>();
			Transform shieldTransform = other.transform.Find("Shield");
			if(shieldScript != null && shieldTransform != null)
			{
				shieldTransform.gameObject.SetActive(true);
				shieldScript.enabled = true;
				Destroy(gameObject);
			}
		}
	}
}
