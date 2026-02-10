using System;
using UnityEngine;

public class InvoPoint : MonoBehaviour
{
    [SerializeField] private float loopTime;
    [SerializeField] private float distance;
    
    private void FixedUpdate()
    {
        transform.position += new Vector3(
            0,
            Mathf.Cos(Time.time * loopTime)*distance,
            0) ;
    }
}
