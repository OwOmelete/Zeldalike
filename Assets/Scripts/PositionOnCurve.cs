using System;
using UnityEngine;

public class PositionOnCurve : MonoBehaviour
{
    public float increment;

    public float curvature;
    public float yAtZero;

    private Vector3 center;

    private Vector3 start;
    public Vector3 end;
    public int control;
    
    float x = 0;


    private void Start()
    {
        start = transform.position;
        
    }

    private void FixedUpdate()
    {
        center = (end - start) / 2 + Vector3.up * control ;
        if (x >= 1) x = 0;
        transform.position = CalculateBezierPoint(x, start, center, end);
        x += increment;
    }
    
    Vector3 CalculateBezierPoint(float t, Vector3 start, Vector3 control, Vector3 end)
    {
        return Mathf.Pow(1 - t, 2) * start +
               2 * (1 - t) * t * control +
               Mathf.Pow(t, 2) * end;
    }

    private float curveCoords(float x)
    {
        return -curvature * (x * x) + yAtZero;
    }
}
