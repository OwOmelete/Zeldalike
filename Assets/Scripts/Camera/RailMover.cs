using UnityEngine;

public enum CameraMode
{
    Rail,
    Follow,
    Fixed
}

public class RailMover : MonoBehaviour
{
    public float CameraTransitionSpeed = 5f;
    
    [Header("Rail Settings")]
    public Rail rail;
    public Transform lookAt;
    public bool smoothMove = true;
    public float moveSpeed = 5f;
    public int maxRotation = 45;
    public int minRotation = 45;

    [Header("Follow Settings")]
    public Transform player;
    public Vector3 followOffset = new Vector3(0, 3, -5);
    public float followSpeed = 5f;
    public float railBlendDistance = 5f;

    [Header("Fixed Camera Settings")]
    public Transform fixedPoint;

    public bool lockedVertical;
    public bool lockedHorizontal;

    [Header("Mode")]
    public CameraMode currentMode = CameraMode.Rail;

    private Transform thisTransform;
    private Vector3 lastRailPosition;
    private Quaternion targetRotation;

    public Quaternion fixedTargetRotation;

    public Camera cam;
    
    void Start()
    {
        thisTransform = transform;
        lastRailPosition = transform.position;
        targetRotation = transform.rotation;
        fixedTargetRotation = fixedPoint.localRotation;
    }

    void Update()
    {
        switch (currentMode)
        {
            case CameraMode.Rail:
                UpdateRail();
                break;

            case CameraMode.Follow:
                UpdateFollow();
                break;

            case CameraMode.Fixed:
                UpdateFixed();
                break;
        }
        
        Vector3 lookTarget = GetLookTarget();
        targetRotation = Quaternion.LookRotation(lookTarget - thisTransform.position);
        
        Vector3 euler = targetRotation.eulerAngles;
        
        euler.x = Mathf.Clamp(euler.x > 180 ? euler.x - 360 : euler.x, -minRotation, maxRotation);
        
        targetRotation = Quaternion.Euler(euler);
        
        if (lockedHorizontal && currentMode == CameraMode.Fixed)
        {
            euler.y = fixedTargetRotation.y;
        }

        if (lockedVertical && currentMode == CameraMode.Fixed)
        {
            euler.x = fixedTargetRotation.x;
        }
        
        thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }

    #region Updates

    void UpdateRail()
    {
        Vector3 railPos = rail.ProjectPositionOnRail(lookAt.position);

        if (smoothMove)
        {
            lastRailPosition = Vector3.Lerp(lastRailPosition, railPos, Time.deltaTime * moveSpeed);
            thisTransform.position = lastRailPosition;
        }
        else
        {
            thisTransform.position = railPos;
        }
    }

    void UpdateFollow()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + followOffset;
        Vector3 railPos = rail.ProjectPositionOnRail(lookAt.position);
        float distanceToRail = Vector3.Distance(targetPos, railPos);

        if (distanceToRail < railBlendDistance)
        {
            float t = 1f - (distanceToRail / railBlendDistance);
            Vector3 blendedPos = Vector3.Lerp(targetPos, railPos, t * CameraTransitionSpeed);
            thisTransform.position = Vector3.Lerp(thisTransform.position, blendedPos, Time.deltaTime * followSpeed);
        }
        else
        {
            thisTransform.position = Vector3.Lerp(thisTransform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }

    void UpdateFixed()
    {
        if (fixedPoint == null) return;

        thisTransform.position = Vector3.Lerp(thisTransform.position, fixedPoint.position, Time.deltaTime * CameraTransitionSpeed);
    }

    #endregion

    Vector3 GetLookTarget()
    {
        
        switch (currentMode)
        {
            case CameraMode.Rail:
                lockedHorizontal = false;
                lockedVertical = false;
                return lookAt.position;
            case CameraMode.Follow:
                lockedHorizontal = false;
                lockedVertical = false;
                return player != null ? player.position : thisTransform.position;
            case CameraMode.Fixed:
                return lookAt.position;
                

            default:
                return thisTransform.position;
        }
    }

    public void SetCameraMode(CameraMode newMode)
    {
        if (newMode == CameraMode.Rail)
        {
            lastRailPosition = transform.position;
        }

        if (newMode != CameraMode.Fixed)
        {
            lockedHorizontal = false;
            lockedVertical = false;
        }

        currentMode = newMode;
    }
}