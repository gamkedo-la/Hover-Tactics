using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : Weapon
{
    [Header("Beam")]
    [SerializeField] private GameObject beamObject;
    [SerializeField] private float beamStartDelay = 0.25f;
    [SerializeField] private float beamGrowFactor = 5.0f;
    [SerializeField] private float beamDelay = 2.0f;
    [SerializeField] private float beamShrinkFactor = 15.0f;

    private float beamTimer = 0.0f;

    private GameObject beam = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
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
                CameraShake.Shake(0.1f, 1, 0.0f);
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
        StartCoroutine("StartBeam");
    }

    IEnumerator StartBeam()
    {
        yield return new WaitForSeconds(beamStartDelay);
        if(beam == null)
        {
            beam = GameObject.Instantiate(beamObject, GetShootingPoint().position, GetShootingPoint().rotation);
            Vector3 rotation = beam.transform.rotation.eulerAngles;
            rotation.x = 0.0f;
            beam.transform.rotation = Quaternion.Euler(rotation);
        }
        beamTimer = beamDelay;
    }
}
