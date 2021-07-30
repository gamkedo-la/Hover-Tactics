using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBars : MonoBehaviour
{
    [SerializeField] private SpriteMask HPMask;
    [SerializeField] private SpriteMask MPMask;
    [SerializeField] private SpriteMask shieldMask;

    void Start()
    {
    }

    void Update()
    {
        GameObject mech = GameManager.instance.GetActiveHoverMech();
        Vector3 position = transform.position;
        position.x = mech.transform.position.x;
        position.z = mech.transform.position.z;
        transform.position = position;
    }

    public void UpdateHP(float percent)
    {
        HPMask.alphaCutoff = percent;
    }

    public void UpdateMP(float percent)
    {
        MPMask.alphaCutoff = percent;
    }

    public void UpdateShield(float percent)
    {
        shieldMask.alphaCutoff = percent;
    }
}
