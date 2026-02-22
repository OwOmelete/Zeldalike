using UnityEngine;

public class StateProtection : IState
{
    
    private InvoBehaviour owner;

    public StateProtection(InvoBehaviour owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner._InvoInstance.rb.useGravity = false;
        owner._InvoInstance.acceleration = 60;
    }

    public void Execute()
    {
        movement();
    }

    public void Exit()
    {
        
    }

    void movement()
    {
        Vector3 dir = owner._InvoInstance.target.position + owner._InvoInstance.offset - owner.transform.position;
        
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
