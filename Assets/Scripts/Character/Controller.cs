using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
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
    }

    private void FixedUpdate()
    {
        Movement();
        Debug.Log(rb.linearVelocity.magnitude);
    }

    void Movement()
    {
        if (InputDirection == Vector3.zero)
        {
            pm.dynamicFriction = friction;
            return;
        }

        pm.dynamicFriction = 0;
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            Vector3 force = moveCalculation(
                InputDirection.normalized * acceleration,
                rb.linearVelocity.normalized * maxSpeed);
            rb.AddForce(force);
        }
        else
        {
            rb.AddForce(InputDirection.normalized * acceleration);
        }
    }

    
    
    Vector3 moveCalculation(Vector3 v, Vector3 u)
    {
        float dot = Vector3.Dot(v, u);
        Vector3 proj = (dot / Vector3.Dot(u, u)) * u;
        
        if (dot > 0)
        {
            return v - proj;
        }

        else
        {
            return v + proj;
        }
        
        return v;
    }
}
