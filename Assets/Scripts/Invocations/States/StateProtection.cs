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
        owner.Data.rb.useGravity = false;
        owner.Data.acceleration = 60;
        owner.transform.position =
            owner.Data.target.position + owner.Data.offset;
    }

    public override void Execute()
    {
        movement(owner,owner.Data.target.position + owner.Data.offset - owner.transform.position);
    }

    public override void Exit()
    {
        owner.Data.rb.useGravity = true;
        owner.Data.acceleration = 15;
    }

    
}
