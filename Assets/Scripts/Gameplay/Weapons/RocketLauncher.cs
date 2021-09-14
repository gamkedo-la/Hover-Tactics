using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocket;
    public float rocketCoolDownSeconds;
    public Transform rocketSpawnPoint;

    private float rocketTimer = 0;

    // Update is called once per frame
    void Update()
    {
        rocketTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && rocketTimer >= rocketCoolDownSeconds)
        {
            rocketCoolDownSeconds = 0f;
            Instantiate(rocket, new Vector3(rocketSpawnPoint.position.x, transform.position.y, rocketSpawnPoint.position.z), Quaternion.identity);
        }
    }
}
