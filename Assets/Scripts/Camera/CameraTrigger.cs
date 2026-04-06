using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraMode modeToActivate = CameraMode.Fixed;
    public RailMover railMover;
    public Transform fixedPoint = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (modeToActivate == CameraMode.Fixed)
            {
                railMover.fixedPoint = fixedPoint;
            }
            railMover.SetCameraMode(modeToActivate);
        }
    }
}