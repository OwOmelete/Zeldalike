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
        owner.Data.offset = owner._Invo.offset;
        owner.Data.acceleration = owner._Invo.acceleration;
        owner.Data.rb.useGravity = true;
        owner.Data.target = owner.player;
        owner.Data.rb.isKinematic = false;
    }

    public override void Execute()
    {
        Vector3 dir = owner.Data.target.position + (owner.Data.target.rotation * owner.Data.offset)  - owner.transform.position;
        dir = new Vector3(dir.x, 0, dir.z);

        if (dir.magnitude > owner._Invo.maxDistance)
            owner.transform.position = owner.Data.target.position + (owner.Data.target.rotation * owner.Data.offset);
        
        movement(owner, dir);
    }

    public override void Exit()
    {
        
    }
}
