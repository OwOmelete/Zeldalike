using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Vector3 CurrentDirection;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    
    private void FixedUpdate()
    {
        Vector3 force = CurrentDirection * speed;
        rb.AddForce(force);
    }

    public void OnMove(InputValue value)
    {
        Vector2 Direction = value.Get<Vector2>();
        CurrentDirection = new Vector3(Direction.x, 0, Direction.y);
    }
}
