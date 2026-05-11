using System;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyHeatSystem : MonoBehaviour
{
    public Slider heatSlider;
    public Slider corruptionSlider;
    public Slider weakPointSlider;

    public float autoHeatIncrement;
    public float corruptionIncrement;
    
    public float maxHeat;
    public float maxCorruption;


    public float weakPointSize = 20;
    public float weakPointSpeed;
    public float timeToHitWeakPoint;
    
    private float currentHeat;
    private float currentCorruption = 0;

    private float currentWeakPointPos;
    private bool isInWeakPoint;

    [Range(0.0f, 1.0f)]
    public float[] weakpointList;
    private int currentIndex;

    private float weakPointTime;

    
    
    private void Start()
    {
        currentHeat = 0;
        currentIndex = 0;
    }

    private void Update()
    {
        heatSlider.value = currentHeat/maxHeat;
        corruptionSlider.value = currentCorruption/maxCorruption;
        currentWeakPointPos = Mathf.Lerp(weakPointSlider.value, weakpointList[currentIndex], 0.1f);
        weakPointSlider.value = currentWeakPointPos;

        if (Input.GetKeyDown(KeyCode.H))
        {
            increaseHeat(5);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            reduceHeat(5);
        }
    }

    private void FixedUpdate()
    {
        if (currentHeat > 0)
        {
            if (currentHeat - autoHeatIncrement < 0)
            {
                currentHeat = 0;
            }
            else
            {
                currentHeat -= autoHeatIncrement;
            }
        }
        /*if (currentHeat > (maxHeat-weakPointSize)*currentWeakPointPos && currentHeat < (maxHeat-weakPointSize)*currentWeakPointPos+weakPointSize)
        {
            currentCorruption += corruptionIncrement;
        }*/

        isInWeakPoint = (currentHeat > (maxHeat - weakPointSize) * currentWeakPointPos &&
                         currentHeat < (maxHeat - weakPointSize) * currentWeakPointPos + weakPointSize);
        

        if (isInWeakPoint)
        {
            if (weakPointTime == 0)
            {
                weakPointTime = Time.time;
            }

            if (Time.time - weakPointTime >= timeToHitWeakPoint)
            {
                ChangeIndex();
            }
            
        }
        else
        {
            weakPointTime = 0;
        }

        //Debug.Log((maxHeat-weakPointSize)*currentWeakPointPos+weakPointSize);
    }

    void ChangeIndex()
    {
        if (currentIndex + 1 >= weakpointList.Length)
        {
            //Die
            Debug.Log("gagné :D");
        }
        else
        {
            currentIndex += 1;
            weakPointTime = 0;
        }
    }
    
    public void reduceHeat(float value)
    {
        currentHeat -= value;
        if (currentHeat < 0) currentHeat = 0;
        
    }

    public void increaseHeat(float value)
    {
        currentHeat += value;
        if (currentHeat > maxHeat) currentHeat = maxHeat;
    }

    /*public float GetColdPercentage()
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
    }*/

    public float GetCorruptionPercentage()
    {
        return currentCorruption / maxCorruption;
    }
    
}
