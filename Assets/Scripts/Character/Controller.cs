using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PhysicsMaterial pm;
    [SerializeField] private float friction;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    private Vector3 InputDirection;

    private void OnMove(InputValue value)
    {
        InputDirection = value.Get<Vector2>();
        InputDirection = new Vector3(InputDirection.x, 0, InputDirection.y);
        InputDirection = cam.rotation * InputDirection;
        InputDirection = new Vector3(InputDirection.x, 0, InputDirection.z);
    }

    private void OnAttack(InputValue value)
    {
        rb.AddForce(rb.linearVelocity.normalized * 100);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (InputDirection == Vector3.zero)
        {
            pm.dynamicFriction = friction;
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(InputDirection.normalized), 0.1f)  ;
        
        pm.dynamicFriction = 0;
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            Vector3 force = Maths.OrthogonalProjection(
                InputDirection.normalized * acceleration,
                rb.linearVelocity.normalized * maxSpeed);
            float m = force.magnitude;
            Vector3 finalForce = new Vector3(force.x, 0, force.z).normalized;
            
            rb.AddForce(finalForce*m);
        }
        else
        {
            Vector3 force = InputDirection.normalized * acceleration;
            
            float m = force.magnitude;
            Vector3 finalForce = new Vector3(force.x, 0, force.z).normalized;

            
            rb.AddForce(finalForce*m);
        }
    }
}
