using System;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private GameObject followed;
    [SerializeField] private float lerpAmount;
    private Vector3 pos;
    
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followed.transform.position, lerpAmount);
    }
}
