using UnityEngine;

[CreateAssetMenu(fileName = "invoData")]
public class InvoData : ScriptableObject
{
    public Vector3 offset;
    public float acceleration;
    public float maxSpeed;
    public float maxDistance;
    

    public InvoDataInstance Instance()
    {
        return new InvoDataInstance(this);
    }
}

public class InvoDataInstance
{
    public Vector3 offset;
    public Transform target;
    public Transform ennemyTarget;
    public Vector3 direction;
    public Rigidbody rb;
    public float acceleration;
    public float maxSpeed;
    public float maxDistance;
    public IState currentState;
    public bool isMoving = true;
    public bool isMovingDirection = false;
    public float damage;
    public bool isActivated = true;
    
    public InvoDataInstance(InvoData data)
    {
        offset = data.offset;
        acceleration = data.acceleration;
        maxSpeed = data.maxSpeed;
        maxDistance = data.maxDistance;
    }
}