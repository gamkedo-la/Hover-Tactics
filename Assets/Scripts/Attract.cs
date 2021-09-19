using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    public float range = 60.0f;
    public string targetTag;

    private GameObject[] targets;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
    }

    void Update()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, targets[i].transform.position);
            if(distance <= range)
            {
                Vector3 position = targets[i].transform.position;
                float y = position.y;
                position =
                    Vector3.Lerp(
                        targets[i].transform.position,
                        transform.position,
                        (1.0f - (distance/range))
                        * (targets[i].name.Contains("Wyvern") ? 0.08f : 0.006f));
                position.y = y;
                targets[i].transform.position = position;
            }
        }
    }
}
