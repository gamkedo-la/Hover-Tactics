using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Color gizmoColor = new Color(1, 1, 0, 0.75F);

    public int GetNextIndex(int index)
    {
        index++;
        if(index < 0) return 0;
        return index % transform.childCount;
    }

    public Transform GetWayPointAt(int index)
    {
        if(index < 0) return transform.GetChild(0);
        return transform.GetChild(index % transform.childCount);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        for(int i = 0; i < transform.childCount; i++)
        {
            if(i > 0)
                Gizmos.DrawLine(transform.GetChild(i-1).position, transform.GetChild(i).position);
            Gizmos.DrawSphere(transform.GetChild(i).position, 1f);
        }
        if(transform.childCount > 2)
            Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(transform.childCount - 1).position);
    }
}
