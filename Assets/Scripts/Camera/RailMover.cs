using UnityEngine;

public class RailMover : MonoBehaviour {

    public Rail rail;
    public Transform lookAt;
    public bool smothMove = true;
    public float moveSpeed;
    public int maxRotation;
    public int minRotation;

    private Transform thisTransform;
    private Vector3 lastPosition;
    void Start () {

        thisTransform = transform;
    }
 
 
    void Update ()
    {
        if (smothMove)
        {
            lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
            thisTransform.position = lastPosition;
        }
        else
        {
            thisTransform.position = rail.ProjectPositionOnRail(lookAt.position);
        }
        //thisTransform.position = rail.ProjectOnSegment(Vector3.zero, Vector3.forward * 20, lookAt.position);
        thisTransform.LookAt(lookAt.position);
        Vector3 rot = new Vector3(Mathf.Clamp(transform.rotation.eulerAngles.x, -minRotation, maxRotation) ,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);

        transform.rotation = Quaternion.Euler(rot);
    }
}