using System;
using Unity.Cinemachine;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public CinemachineCamera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<CinemachineCamera>();
        cam.Target.TrackingTarget = InvoManager.Instance.transform;
    }
}
