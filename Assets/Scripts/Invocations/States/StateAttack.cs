using System.Collections;
using UnityEngine;

public class StateAttack : IState
{
    
    private InvoBehaviour owner;
    
    public StateAttack(InvoBehaviour owner)
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        owner._InvoInstance.rb.useGravity = false;
        owner._InvoInstance.acceleration = 60;
        owner.startAttackDelay();

    }

    public override void Execute()
    {
        movement(owner,owner._InvoInstance.target.position + owner._InvoInstance.offset - owner.transform.position);
    }

    public override void Exit()
    {
        owner._InvoInstance.rb.useGravity = true;
        owner._InvoInstance.isMoving = true;
    }
}
