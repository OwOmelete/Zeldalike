using UnityEngine;

public class StateIdle : IState
{
    private InvoBehaviour owner;

    public StateIdle(InvoBehaviour owner)
    {
        this.owner = owner;
    }

    
    public void Enter()
    {
        owner._InvoInstance.offset = owner._Invo.offset;
        owner._InvoInstance.acceleration = owner._Invo.acceleration;
        owner._InvoInstance.rb.useGravity = true;
        owner._InvoInstance.target = owner.player;
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
        Vector3 dir = owner._InvoInstance.target.position + owner._InvoInstance.offset  - owner.transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        
        
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
