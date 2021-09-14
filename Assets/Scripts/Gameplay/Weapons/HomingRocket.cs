using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    public float rocketSpeed;
    public float rotateSpeed;
    public float targetRange;
    public float secondsToSelfDestruct;

    private Rigidbody rb;
    private Transform target;
    private float selfDestructTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindClosestEnemy();
    }

	private void Update()
	{
        target = FindClosestEnemy();

        selfDestructTimer += Time.deltaTime;
        if(selfDestructTimer >= secondsToSelfDestruct)
		{
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        if(target == gameObject.transform)
		{
            rb.velocity = transform.forward * rocketSpeed;
        }
        else
		{
            Vector3 direction = target.position - rb.position;
            direction.Normalize();

            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);
            rb.angularVelocity = rotationAmount * rotateSpeed;

            rb.velocity = transform.forward * rocketSpeed;
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance <= targetRange)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if(closest == null)
		{
            return gameObject.transform;
		}
        else
		{
            return closest.transform;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        Destroy(gameObject);
        if(collision.gameObject.tag == "Enemy")
		{
            Destroy(collision.gameObject);
		}
	}
}
