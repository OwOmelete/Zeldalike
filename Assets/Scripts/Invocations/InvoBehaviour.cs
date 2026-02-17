using System;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;
    
    public Transform target;
    [SerializeField] private Rigidbody rb;
    public Vector3 baseOffset;
    public float acceleration;
    public float maxSpeed;
    [SerializeField] private float maxDistance;

    public State currentState;
    
    public enum State
    {
        idle,
        protection
    }

    private void Start()
    {
        currentState = State.idle;
        offset = baseOffset;
    }

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
        if (currentState == State.idle)
        {
            return player.position + offset + Maths.idleOffset(3, 0.03f) - transform.position;
        }
        return player.position + offset - transform.position;
    }
}
