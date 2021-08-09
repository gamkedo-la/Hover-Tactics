using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialWeapon : Weapon
{
    [Header("RadialBoom")]
    [SerializeField] private GameObject boomObject;
    [SerializeField] private float boomMaxScale = 25.0f;
    [SerializeField] private float boomGrowFactor = 5.0f;
    [SerializeField] private float boomDelay = 2.0f;

    private float boomTimer = 0.0f;

    private GameObject boom = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if(boom != null)
        {
            boom.transform.position = GetShootingPoint().transform.position;
            boom.transform.rotation = GetShootingPoint().transform.rotation;
            Vector3 rotation = boom.transform.rotation.eulerAngles;
            rotation.x = 0.0f;
            boom.transform.rotation = Quaternion.Euler(rotation);

            if(boomTimer > 0.0f)
            {
                boom.transform.localScale = Vector3.Lerp(boom.transform.localScale, Vector3.one * boomMaxScale, boomGrowFactor * Time.deltaTime);
                boomTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(boom);
                boom = null;
            }
        }
    }

    protected override void Fire()
    {
        if(boom == null)
        {
            boom = GameObject.Instantiate(boomObject, GetShootingPoint().position, GetShootingPoint().rotation);
            Vector3 rotation = boom.transform.rotation.eulerAngles;
            rotation.x = 0.0f;
            boom.transform.rotation = Quaternion.Euler(rotation);
        }
        boomTimer = boomDelay;
    }
}
