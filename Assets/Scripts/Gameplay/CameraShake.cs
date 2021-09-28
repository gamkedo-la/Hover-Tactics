using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float intensity = 0.0f;
    private int cycles = 0;
    private float cycleDelay = 0.0f;
    private float intensityMultiplierPerCycle = 1.0f;
    private float timer = 0.0f;

    public static List<CameraShake> instances = null;

    private static void ResetAllShakes()
    {
        foreach(CameraShake camShake in instances)
        {
            camShake.intensity = 0.0f;
            camShake.cycles = 0;
            camShake.cycleDelay = 0.0f;
            camShake.intensityMultiplierPerCycle = 1.0f;
            camShake.timer = 0.0f;
        }
    }

    public static void Shake(float intensity, int cycles, float cycleDelay, float intensityMultiplierPerCycle = 1.0f)
    {
        bool gotOne = false;
        foreach(CameraShake camShake in instances)
        {
            if(camShake.cycles <= 0 && camShake.timer <= 0.0f)
            {
                camShake.intensity = intensity;
                camShake.cycles = cycles;
                camShake.cycleDelay = cycleDelay;
                camShake.intensityMultiplierPerCycle = intensityMultiplierPerCycle;
                gotOne = true;
                break;
            }
        }
        if(!gotOne)
        {
            ResetAllShakes();
            Shake(intensity, cycles, cycleDelay, intensityMultiplierPerCycle);
        }
    }

    private void Update()
    {
        if(cycles > 0)
        {
            if(timer <= 0.0f)
            {
                Vector3 position = transform.position;
                position.x += UnityEngine.Random.Range(-intensity, intensity);
                position.y += UnityEngine.Random.Range(-intensity, intensity);
                position.z += UnityEngine.Random.Range(-intensity, intensity);
                transform.position = position;
                intensity *= intensityMultiplierPerCycle;
                cycles--;
                timer = cycleDelay;
            }
            else
            {
                timer -= Time.unscaledDeltaTime;
            }
        }
    }
}
