using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBars : MonoBehaviour
{
    [SerializeField] private SpriteMask HPMask;
    [SerializeField] private SpriteMask MPMask;

    void Start()
    {
        
    }

    void Update()
    {
        GameObject mech = GameManager.instance.GetActiveHoverMech();

        if(GameManager.instance.mechChanged)
        {
            //update hp and mp
            Debug.Log("Mech Changed and Bars Updated.");
        }

        Vector3 position = transform.position;
        position.x = mech.transform.position.x;
        position.z = mech.transform.position.z;
        transform.position = position;

        //TEMP
        UpdateHP(0.75f);
        UpdateMP(0.5f);
    }

    public void UpdateHP(float percent)
    {
        HPMask.alphaCutoff = percent;
    }

    public void UpdateMP(float percent)
    {
        MPMask.alphaCutoff = percent;
    }

}
