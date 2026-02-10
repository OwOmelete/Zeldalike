using System;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform followed;
    [SerializeField] private float lerpAmount;
    private Vector3 pos;
    
    private void Start()
    {
        pos = followed.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, pos, lerpAmount);
    }
}
