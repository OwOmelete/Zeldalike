using UnityEngine;

public class StateIdle : IState
{
    private InvoBehaviour owner;

    public StateIdle(InvoBehaviour owner)
    {
        this.owner = owner;
    }

    
    public override void Enter()
    {
        owner._InvoInstance.offset = owner._Invo.offset;
        owner._InvoInstance.acceleration = owner._Invo.acceleration;
        owner._InvoInstance.rb.useGravity = true;
        owner._InvoInstance.target = owner.player;
        owner._InvoInstance.rb.isKinematic = false;
    }

    public override void Execute()
    {
        Vector3 dir = owner._InvoInstance.target.position + owner._InvoInstance.offset  - owner.transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        
        movement(owner, dir);
    }

    public override void Exit()
    {
        
    }
}
