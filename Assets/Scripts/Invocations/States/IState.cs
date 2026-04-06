using UnityEngine;

public abstract class IState
{
    
    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
    
    public void movement(InvoBehaviour owner, Vector3 dir)
    {
        if (!owner.Data.isMoving) return;

        float distance = dir.magnitude;

        float forceAmount = Mathf.Clamp(
            distance * owner.Data.acceleration,
            0f,
            owner.Data.maxSpeed
        );
        
        if (Vector3.Dot(owner.Data.rb.linearVelocity, dir) < 0f)
        {
            forceAmount *= 3;
        }

        Vector3 desiredVelocity = dir.normalized * forceAmount;
        Vector3 steering = desiredVelocity - owner.Data.rb.linearVelocity;

        

        owner.Data.rb.AddForce(steering, ForceMode.Acceleration);
        
       
    }

    public void b(InvoBehaviour owner, Vector3 dir)
    {
        if (!owner.Data.isMoving) return;
        
        if (owner.Data.rb.linearVelocity.magnitude > owner.Data.maxSpeed)
        {
            Vector3 force = Maths.OrthogonalProjection(
                dir.normalized * owner.Data.acceleration,
                owner.Data.rb.linearVelocity.normalized * owner.Data.maxSpeed);
            owner.Data.rb.AddForce(force);
        }
        
        else
        {
            owner.Data.rb.AddForce(dir.normalized * owner.Data.acceleration);
        }
    }
}


