using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFence : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject startFence;
    public GameObject endFence;
    public float heightOffset;

    // Update is called once per frame
    void Update()
    {
        if(lineRenderer != null)
		{
            lineRenderer.SetPosition(0, new Vector3(startFence.transform.position.x, startFence.transform.position.y + heightOffset, startFence.transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(endFence.transform.position.x, endFence.transform.position.y + heightOffset, endFence.transform.position.z));
		}
    }
}
