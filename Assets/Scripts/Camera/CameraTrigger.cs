using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraMode modeToActivate = CameraMode.Fixed;
    public RailMover railMover;
    public Rail rail;
    public Transform fixedPoint = null;
    public Vector3 followOffset;
    public float camSize;

    public bool lockVertical;
    public bool lockHorizontal;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (modeToActivate == CameraMode.Fixed)
            {
                railMover.fixedPoint = fixedPoint;
                railMover.lockedHorizontal = lockHorizontal;
                railMover.lockedVertical = lockVertical;
                railMover.fixedTargetRotation = fixedPoint.localRotation;
            }

            if (modeToActivate == CameraMode.Rail)
            {
                railMover.rail = rail;
            }

            if (modeToActivate == CameraMode.Follow)
            {
                railMover.followOffset = followOffset;
            }

            railMover.cam.orthographicSize = camSize;
            railMover.SetCameraMode(modeToActivate);
        }
    }
}