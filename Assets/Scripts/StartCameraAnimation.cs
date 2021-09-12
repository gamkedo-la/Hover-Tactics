using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraAnimation : MonoBehaviour
{
    public float lerpFactor = 0.1f;
    public float minDistance = 1.0f;
    public Transform[] transforms;

    private int index = 0;

    void Start()
    {
        
    }

    void Update()
    {
        //no Time.deltaTime factor!
        transform.position = Vector3.Lerp(transform.position, transforms[index].position, lerpFactor);
        if(Vector3.Distance(transform.position, transforms[index].position) <= minDistance)
        {
            index++;
            if(index >= transforms.Length) index = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0; i < transforms.Length - 1; i++)
            Gizmos.DrawLine(transforms[i].position, transforms[i+1].position);
        Gizmos.DrawLine(transforms[transforms.Length-1].position, transforms[0].position);
    }
}
