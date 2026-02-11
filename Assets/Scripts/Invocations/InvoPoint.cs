using System;
using UnityEngine;

public class InvoPoint : MonoBehaviour
{
    [SerializeField] private float loopTime;
    [SerializeField] private float distance;
    
    private void FixedUpdate()
    {
        transform.position += new Vector3(
            Mathf.Cos(Time.time * loopTime)*distance,
            Mathf.Sin(Time.time * loopTime*0.5f)*distance*0.5f,
            Mathf.Sin(Time.time * loopTime)*distance);
    }
}
