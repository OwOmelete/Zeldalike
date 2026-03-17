using System.Collections;
using UnityEngine;

public class StateAttack : IState
{
    
    public InvoBehaviour owner;
    
    public StateAttack(InvoBehaviour owner)
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        owner._InvoInstance.acceleration = 60;
        owner.startAttackDelay();
        owner._InvoInstance.direction = Vector3.zero;
        owner._InvoInstance.isMovingDirection = false;
        if (owner._InvoInstance.ennemyTarget == null)
        {
            owner.transform.position = owner._InvoInstance.target.position +
                                        owner._InvoInstance.target.rotation
                                         * owner._InvoInstance.offset;
        }
        else
        {
            owner.transform.position = owner._InvoInstance.target.position + 
                                       ((Quaternion.FromToRotation(Vector3.forward, owner._InvoInstance.ennemyTarget.position - owner._InvoInstance.target.position)
                                         * owner._InvoInstance.offset));
        }
        

    }

    public override void Execute()
    {
        Debug.Log(owner._InvoInstance.ennemyTarget);
        if (owner._InvoInstance.direction != Vector3.zero && owner._InvoInstance.isMovingDirection)
        {
            //owner.transform.position += owner._InvoInstance.direction;
            owner._InvoInstance.rb.MovePosition(owner.transform.position + owner._InvoInstance.direction);
        }
        else if (owner._InvoInstance.ennemyTarget == null)
        {
            movement(owner,owner._InvoInstance.target.position + (owner._InvoInstance.target.rotation * owner._InvoInstance.offset) - owner.transform.position);
        }
        else
        {
            movement(owner,owner._InvoInstance.target.position + 
                (Quaternion.FromToRotation(Vector3.forward, owner._InvoInstance.ennemyTarget.position - owner._InvoInstance.target.position)
                 * owner._InvoInstance.offset) - owner.transform.position);
        }
    }

    public override void Exit()
    {
        owner._InvoInstance.rb.MovePosition(owner.transform.position);
        owner._InvoInstance.direction = Vector3.zero;
        owner._InvoInstance.isMovingDirection = false;
        owner._InvoInstance.rb.isKinematic = false;
        owner._InvoInstance.isMoving = true;
    }
}
