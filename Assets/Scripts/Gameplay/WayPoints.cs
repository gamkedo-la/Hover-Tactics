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
}
