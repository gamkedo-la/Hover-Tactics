using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    [Header("Raycast")]
    [SerializeField] private string hitImpactTag;
    [SerializeField] private float lineDelay = 0.05f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineThickness = 0.5f;

    private List<LineRenderer> lineRenderers;
    private List<float> lineTimers;

    void Start()
    {
        base.Start();

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
    }

    void Update()
    {
        base.Update();

        for(int i = 0; i < shootingPoints.Length; i++)
        {
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
        lineRenderers[shootingPointIndex].SetPosition(0, shootingPoints[shootingPointIndex].position);
        lineRenderers[shootingPointIndex].SetPosition(1, shootingPoints[shootingPointIndex].position + (shootingPoints[shootingPointIndex].forward * 1000.0f));
        lineRenderers[shootingPointIndex].enabled = true;
        lineTimers[shootingPointIndex] = lineDelay;

        RaycastHit hitData;
        if (Physics.Raycast(shootingPoints[shootingPointIndex].position, shootingPoints[shootingPointIndex].forward, out hitData))
        {
            lineRenderers[shootingPointIndex].SetPosition(1, hitData.point);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitData.normal);
            ObjectPooler.instance.SpawnFromPool(hitImpactTag, hitData.point, rot);
        }
    }
}
