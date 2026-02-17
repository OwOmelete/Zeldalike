using System;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    public Transform baseTarget;
    

    public Transform target;
    [SerializeField] private Rigidbody rb;
    public float acceleration;
    public float maxSpeed;
    [SerializeField] private float maxDistance;

    private void FixedUpdate()
    {
        Movement();
    }
    
    void Movement()
    {
        /*if ((transform.position - target.position).magnitude > maxDistance)
        {
            rb.AddForce(getDirection().normalized * acceleration);
            return;
        }*/
        
        //^^
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            Vector3 force = Maths.OrthogonalProjection(
                getDirection().normalized * acceleration,
                rb.linearVelocity.normalized * maxSpeed);
            rb.AddForce(force);
        }
        
        else
        {
            rb.AddForce(getDirection().normalized * acceleration);
        }
    }

    void Protection()
    {
        
    }
    
    Vector3 getDirection()
    {
        return target.position - transform.position;
    }
}
