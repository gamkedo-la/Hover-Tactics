using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] WayPointArray;

    public int GetNextIndex(int index)
    {
        index++;
        if(index < 0)
        {
            return 0;
        }

        return index % WayPointArray.Length;
    }

    public Transform GetWayPointAt(int index)
    {
        if(index < 0)
        {
            return WayPointArray[0];
        }

        return WayPointArray[index % WayPointArray.Length];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);

        for(int i = 0; i < WayPointArray.Length; i++)
        {
            if(i > 0)
            {
                Gizmos.DrawLine(WayPointArray[i-1].position, WayPointArray[i].position);
            }
            Gizmos.DrawSphere(WayPointArray[i].position, 1f);
        }
    }
}
