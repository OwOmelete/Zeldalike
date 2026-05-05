using System;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private GameObject followed;
    [SerializeField] private float lerpAmount;
    private Vector3 pos;

    private void Start()
    {
        followed = InvoManager.Instance.gameObject;
        pos = transform.position;
    }
    
    

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followed.transform.position + pos, lerpAmount);
    }
}
