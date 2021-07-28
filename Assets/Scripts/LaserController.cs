using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    //public float LaserPause = .2f;

    private LineRenderer lineRenderer;
    private float LL;
    private float Alpha = 1;
    private bool Active;
    private float LaserLife;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //LL = 1 + LaserPause;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (Active)
        {
            LL -= Time.deltaTime / LaserLife;

            Alpha = Mathf.Lerp(0, 1, LL);

            lineRenderer.material.SetFloat("Alpha", Alpha);

            if(LL < 0)
            {
                Active = false;
                lineRenderer.enabled = false;
            }
        }
    }


    public void Shoot(float HangTime, float DecayTime, float Length)
    {
        lineRenderer.enabled = true;

        Active = true;

        LaserLife = HangTime;
        LL = 1 + (DecayTime * HangTime);

        Vector4 T = new Vector4(Length, 1, 0, 0);

        lineRenderer.material.SetVector("Tiling", T);


    }
}
