using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = GameManager.instance.GetActiveHoverMech().transform.position;
    }
}
