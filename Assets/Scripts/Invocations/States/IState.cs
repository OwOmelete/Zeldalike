using UnityEngine;

public abstract class IState
{
    
    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
    
    public void movement(InvoBehaviour owner, Vector3 dir)
    {
        if (owner._InvoInstance.rb.linearVelocity.magnitude > owner._InvoInstance.maxSpeed)
        {
            Vector3 force = Maths.OrthogonalProjection(
                dir.normalized * owner._InvoInstance.acceleration,
                owner._InvoInstance.rb.linearVelocity.normalized * owner._InvoInstance.maxSpeed);
            owner._InvoInstance.rb.AddForce(force);
        }
        
        else
        {
            owner._InvoInstance.rb.AddForce(dir.normalized * owner._InvoInstance.acceleration);
        }
    }
}


