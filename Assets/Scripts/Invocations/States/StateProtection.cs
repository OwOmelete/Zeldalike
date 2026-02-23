using UnityEngine;

public class StateProtection : IState
{
    
    private InvoBehaviour owner;

    public StateProtection(InvoBehaviour owner)
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        owner._InvoInstance.rb.useGravity = false;
        owner._InvoInstance.acceleration = 60;
    }

    public override void Execute()
    {
        movement(owner,owner._InvoInstance.target.position + owner._InvoInstance.offset - owner.transform.position);
    }

    public override void Exit()
    {
        
    }

    
}
