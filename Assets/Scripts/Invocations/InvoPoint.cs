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
            0,
            Mathf.Sin(Time.time * loopTime)*distance) ;
    }
}
