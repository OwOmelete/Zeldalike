using System;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyHeatSystem : MonoBehaviour
{
    public Slider heatSlider;
    public Slider corruptionSlider;
    public float hotThreshold;
    public float coldThreshold;

    public float autoHeatIncrement;
    public float corruptionIncrement;
    
    public float maxHeat;
    public float maxCorruption;

    
    private float currentHeat;
    private float currentCorruption = 0;


    private void Start()
    {
        currentHeat = maxHeat;
    }

    private void Update()
    {
        heatSlider.value = currentHeat/maxHeat;
        corruptionSlider.value = currentCorruption/maxCorruption;
    }

    private void FixedUpdate()
    {
        if (currentHeat < maxHeat)
        {
            if (currentHeat + autoHeatIncrement > maxHeat)
            {
                currentHeat = maxHeat;
            }
            else
            {
                currentHeat += autoHeatIncrement;
            }
        }
        if (currentHeat < coldThreshold && currentHeat > hotThreshold)
        {
            currentCorruption += corruptionIncrement;
        }
    }

    public void reduceHeat(float value)
    {
        currentHeat += value;
        Mathf.Clamp(currentHeat, 0, maxHeat);
    }

    public void increaseHeat(float value)
    {
        currentHeat -= value;
        Mathf.Clamp(currentHeat, 0, maxHeat);
    }

    public float GetColdPercentage()
    {
        if (currentHeat > coldThreshold)
        {
            float cold = maxHeat - coldThreshold;
            
            return (currentHeat - cold) / cold;
        }
        else
        {
            return 0;
        }
    }

    public float GetHotPercentage()
    {
        if (currentHeat < hotThreshold)
        {
            return currentHeat / hotThreshold;
        }
        else
        {
            return 0;
        }
    }

    public float GetCorruptionPercentage()
    {
        return currentCorruption / maxCorruption;
    }
    
}
