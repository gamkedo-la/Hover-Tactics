using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAnimation : MonoBehaviour
{
    public float rotationSpeed;
    public float floatInterval;
    public float floatSpeed;

    private float height = 0f;
    private Vector3 currentPos;
    private float floatSign = 1;

    void Update()
    {
        currentPos = transform.position;
        height += floatSpeed * floatSign;
        Debug.Log(height);
        Debug.Log(currentPos.y + height);
        if(height <= -floatInterval)
		{
            height = -floatInterval;
            floatSign *= -1;
        }
        if (height >= floatInterval)
        {
            height = floatInterval;
            floatSign *= -1;
        }

        //transform.position = new Vector3(currentPos.x, currentPos.y + height, currentPos.z);
        
        transform.Rotate(0f, 0f, rotationSpeed);
    }
}
