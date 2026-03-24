using UnityEngine;

public abstract class IState
{
    
    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
    
    public void movement(InvoBehaviour owner, Vector3 dir)
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


