using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : Weapon
{
    [Header("Beam")]
    [SerializeField] private GameObject beamObject;
    [SerializeField] private float beamGrowFactor = 5.0f;
    [SerializeField] private float beamDelay = 2.0f;
    [SerializeField] private float beamShrinkFactor = 15.0f;

    private float beamTimer = 0.0f;

    private GameObject beam = null;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();

        if(beam != null)
        {
            beam.transform.position = GetShootingPoint().transform.position;
            beam.transform.rotation = GetShootingPoint().transform.rotation;
            Vector3 rotation = beam.transform.rotation.eulerAngles;
            rotation.x = 0.0f;
            beam.transform.rotation = Quaternion.Euler(rotation);

            if(beamTimer > 0.0f)
            {
                beam.transform.localScale = Vector3.Lerp(beam.transform.localScale, Vector3.one, beamGrowFactor * Time.deltaTime);
                beamTimer -= Time.deltaTime;
            }
            else if(beam.transform.localScale.x > 0.01f)
            {
                beam.transform.localScale = Vector3.Lerp(beam.transform.localScale, new Vector3(0.0f, 0.0f, 1.0f), beamShrinkFactor * Time.deltaTime);
            }
            else
            {
                Destroy(beam);
                beam = null;
            }
        }
    }

    protected override void Fire()
    {
        beam = GameObject.Instantiate(beamObject, GetShootingPoint().position, GetShootingPoint().rotation);
        Vector3 rotation = beam.transform.rotation.eulerAngles;
        rotation.x = 0.0f;
        beam.transform.rotation = Quaternion.Euler(rotation);
        beamTimer = beamDelay;
    }
}
