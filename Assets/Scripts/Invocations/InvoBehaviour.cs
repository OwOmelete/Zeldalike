using System;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;

    private void FixedUpdate()
    {
        Movement();
    }
    
    void Movement()
    {
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

    Vector3 getDirection()
    {
        return target.position - transform.position;
    }
}
