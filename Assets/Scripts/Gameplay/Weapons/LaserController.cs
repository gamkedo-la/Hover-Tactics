using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    //public float laserPause = .2f;
    private LineRenderer lineRenderer;
    private float laserTimer;
    private float alpha = 1;
    private bool active;
    private float laserDelay;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //laserTimer = 1 + laserPause;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (active)
        {
            laserTimer -= Time.deltaTime / laserDelay;
            alpha = Mathf.Lerp(0, 1, laserTimer);
            lineRenderer.material.SetFloat("Alpha", alpha);
            if(laserTimer < 0)
            {
                active = false;
                lineRenderer.enabled = false;
            }
        }
    }

    public void Shoot(float HangTime, float DecayTime, float Length)
    {
        lineRenderer.enabled = true;
        active = true;
        laserDelay = HangTime;
        laserTimer = 1 + (DecayTime * HangTime);
        Vector4 vector = new Vector4(Length, 1, 0, 0);
        lineRenderer.material.SetVector("Tiling", vector);
    }
}
