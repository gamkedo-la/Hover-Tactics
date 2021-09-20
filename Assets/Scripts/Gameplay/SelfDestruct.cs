using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private string targetTag;
    [SerializeField] private float range = 20.0f;
    [SerializeField] private int totalCycles = 5;
    [SerializeField] private float cycleOnDelay = 0.5f;
    [SerializeField] private float cycleOffDelay = 0.5f;

    private GameObject[] targets;
    private int cyclesDone = 0;
    private float cycleTimer = 0.0f;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
    }

    void Update()
    {
        bool inRange = false;
        for(int i = 0; i < targets.Length; i++)
        {
            inRange = true;
            if(targets[i].GetComponent<MechController>().enabled
            && Vector3.Distance(targets[i].transform.position, transform.position) <= range)
            {
                if(cycleTimer <= 0.0f)
                {
                    if(!indicator.activeSelf)
                    {
                        indicator.SetActive(true);
                        cycleTimer = cycleOnDelay;
                    }
                    else
                    {
                        indicator.SetActive(false);
                        cycleTimer = cycleOffDelay;

                        if(cyclesDone >= totalCycles)
                        {
                            targets[i].GetComponent<Health>().ChangeBy(-0.6f);
                            transform.GetComponent<Health>().ChangeBy(-1.0f);
                            break;
                        }
                        else
                        {
                            cyclesDone++;
                        }
                    }
                }
                else
                {
                    cycleTimer -= Time.deltaTime;
                    break;
                }
            }
        }
        if(!inRange)
        {
            indicator.SetActive(false);
            cycleTimer = 0.0f;
            cyclesDone = 0;
        }
    }
}
