using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    [Header("Raycast")]
    [SerializeField] private float damagePerHit = 0.05f;
    [SerializeField] private string hitImpactTag;
    [SerializeField] private float lineDelay = 0.05f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineThickness = 0.5f;

    private List<LineRenderer> lineRenderers;
    private List<float> lineTimers;

    private float resetRecoil;

    protected override void Start()
    {
        base.Start();

        //Adding Line Renderer for each Shooting Point to render Raycast Shooting
        lineRenderers = new List<LineRenderer>();
        lineTimers = new List<float>();
        for(int i = 0; i < shootingPoints.Length; i++)
        {
            GameObject lineObject = new GameObject();
            lineObject.transform.parent = gameObject.transform;
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineRenderer.endWidth = lineThickness;
            lineRenderers.Add(lineRenderer);
            lineTimers.Add(0.0f);
        }

        if(shootingPoints.Length > 0)
            resetRecoil = shootingPoints[0].GetChild(0).localPosition.z;
    }

    protected override void Update()
    {
        base.Update();

        //Decreasing Alpha (and ultimately, disabling) Line Renderer as the time passes for each Shooting Point
        for(int i = 0; i < shootingPoints.Length; i++)
        {
            Vector3 localShooterPosition = shootingPoints[i].GetChild(0).localPosition;
            localShooterPosition.z = Mathf.Lerp(localShooterPosition.z, resetRecoil, 20.0f * Time.deltaTime);
            shootingPoints[i].GetChild(0).localPosition = localShooterPosition;

            if(lineTimers[i] <= 0.0f)
            {
                lineRenderers[i].enabled = false;
                Color color = lineRenderers[i].startColor;
                color.a = 1.0f;
                lineRenderers[i].startColor = lineRenderers[i].endColor = color;
            }
            else
            {
                Color color = lineRenderers[i].startColor;
                color.a = lineTimers[i] / lineDelay;
                lineRenderers[i].startColor = lineRenderers[i].endColor = color;
                lineTimers[i] -= Time.deltaTime;
            }
        }
    }

    protected override void Fire()
    {
        lineRenderers[shootingPointIndex].SetPosition(0, GetShootingPoint().position);
        lineRenderers[shootingPointIndex].SetPosition(1, GetShootingPoint().position + (GetShootingPoint().forward * 1000.0f));
        lineRenderers[shootingPointIndex].enabled = true;
        lineTimers[shootingPointIndex] = lineDelay;

        Vector3 localShooterPosition = GetShootingPoint().GetChild(0).localPosition;
        localShooterPosition.z -= 2.0f;
        GetShootingPoint().GetChild(0).localPosition = localShooterPosition;

        RaycastHit hitData;
        if (Physics.Raycast(GetShootingPoint().position, GetShootingPoint().forward, out hitData))
        {
            lineRenderers[shootingPointIndex].SetPosition(1, hitData.point);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitData.normal);
            ObjectPooler.instance.SpawnFromPool(hitImpactTag, hitData.point, rot);

            if(hitData.transform.gameObject.layer == 10) return; //NoDamage

            AbstractTakeDamage canTakeDamage = hitData.transform.GetComponent<AbstractTakeDamage>();
            if(canTakeDamage) canTakeDamage.TakeDamage(GetDamage());
        }
    }

    public override Damage GetDamage()
    {
        return new Damage()
        {
            Value = -damagePerHit
        };
    }
}
