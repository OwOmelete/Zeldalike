using UnityEngine;

[CreateAssetMenu(fileName = "invoData")]
public class InvoData : ScriptableObject
{
    public Vector3 offset;
    public float acceleration;
    public float maxSpeed;
    public float maxDistance;
    
    public enum State
    {
        idle,
        protection
    }

    public InvoDataInstance Instance()
    {
        return new InvoDataInstance(this);
    }
}

public class InvoDataInstance
{
    public Vector3 offset;
    public Transform target;
    public Rigidbody rb;
    public float acceleration;
    public float maxSpeed;
    public float maxDistance;
    public InvoData.State currentState;
    
    public InvoDataInstance(InvoData data)
    {
        offset = data.offset;
        acceleration = data.acceleration;
        maxSpeed = data.maxSpeed;
        maxDistance = data.maxDistance;
    }
}