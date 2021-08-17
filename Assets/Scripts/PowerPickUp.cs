using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickUp : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Power powerScript = other.GetComponent<Power>();
			//Transform powerTransform = other.transform.Find("Power");
			//if (powerScript != null && powerTransform != null)
			if (powerScript != null)
			{
				//powerTransform.gameObject.SetActive(true);
				powerScript.enabled = true;
				Destroy(gameObject);
			}
		}
	}
}
