using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceActivation : MonoBehaviour
{
    [SerializeField] private Transform activatorTransform;
    [SerializeField] private float distance = 160.0f;
    [SerializeField] private float updateDelay = 0.1f;

    private float updateTimer = 0.0f;

    void Update()
    {
        if(updateTimer <= 0.0f)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(Vector3.Distance(transform.GetChild(i).position, activatorTransform.position) <= distance);
            }
            updateTimer = updateDelay;
        }
        else
        {
            updateTimer -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if(!activatorTransform) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(activatorTransform.position, distance);
    }
}
