using System;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyHeatSystem : MonoBehaviour
{
    public Slider heatSlider;
    public Slider corruptionSlider;
    public Slider weakPointSlider;

    public bool training;

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

    // La température naturelle est le centre de la jauge
    private float naturalHeat => maxHeat * 0.5f;

    // Zone interdite autour du centre pour les weak points (en fraction de la jauge)
    [Range(0.0f, 0.5f)]
    public float centerExclusionRadius = 0.1f;

    public bool isAlive = true;


    public event Action weakpointBroke;
    
    
    private void Start()
    {
        currentHeat = naturalHeat;
        currentIndex = 0;
    }

    private void Update()
    {
        heatSlider.value = currentHeat/maxHeat;
        corruptionSlider.value = currentCorruption/maxCorruption;

        if (weakpointList.Length > 0)
        {
            currentWeakPointPos = Mathf.Lerp(weakPointSlider.value, weakpointList[currentIndex], 0.1f);
            weakPointSlider.value = currentWeakPointPos;

        }
        
        
        /*if (Input.GetKeyDown(KeyCode.H))
        {
            increaseHeat(5);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            reduceHeat(5);
        }*/
    }

    private void FixedUpdate()
    {
        // La barre tend toujours vers sa température naturelle (le centre)
        if (currentHeat < naturalHeat)
        {
            currentHeat = Mathf.Min(currentHeat + autoHeatIncrement, naturalHeat);
        }
        else if (currentHeat > naturalHeat)
        {
            currentHeat = Mathf.Max(currentHeat - autoHeatIncrement, naturalHeat);
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

        weakpointBroke?.Invoke();
        if (currentIndex + 1 >= weakpointList.Length)
        {
            if (training)
            {
                currentIndex = 0;
                weakPointTime = 0;
            }
            else
            {
                //Die
                Debug.Log("gagné :D");
                isAlive = false;
            }
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

    /// <summary>
    /// Retourne vrai si la position (normalisée 0-1) est trop proche du centre
    /// et donc invalide pour un weak point.
    /// </summary>
    public bool IsWeakPointPositionValid(float normalizedPos)
    {
        return Mathf.Abs(normalizedPos - 0.5f) > centerExclusionRadius;
    }

    /// <summary>
    /// Expose la température naturelle (centre de jauge) pour les systèmes externes.
    /// </summary>
    public float GetNaturalHeat() => naturalHeat;
    
}