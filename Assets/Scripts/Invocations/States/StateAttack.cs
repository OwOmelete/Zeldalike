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
        owner.Data.acceleration = 60;
        owner.startAttackDelay();
        owner.Data.direction = Vector3.zero;
        owner.Data.isMovingDirection = false;
        if (owner.Data.ennemyTarget == null)
        {
            owner.transform.position = owner.Data.target.position +
                                        owner.Data.target.rotation
                                         * owner.Data.offset;
        }
        else
        {
            owner.transform.position = owner.Data.target.position + 
                                       ((Quaternion.FromToRotation(Vector3.forward, owner.Data.ennemyTarget.position - owner.Data.target.position)
                                         * owner.Data.offset));
        }
        

    }

    public override void Execute()
    {
        Debug.Log(owner.Data.ennemyTarget);
        if (owner.Data.direction != Vector3.zero && owner.Data.isMovingDirection)
        {
            //owner.transform.position += owner._InvoInstance.direction;
            owner.Data.rb.MovePosition(owner.transform.position + owner.Data.direction);
        }
        else if (owner.Data.ennemyTarget == null)
        {
            movement(owner,owner.Data.target.position + (owner.Data.target.rotation * owner.Data.offset) - owner.transform.position);
        }
        else
        {
            movement(owner,owner.Data.target.position + 
                (Quaternion.FromToRotation(Vector3.forward, owner.Data.ennemyTarget.position - owner.Data.target.position)
                 * owner.Data.offset) - owner.transform.position);
        }
    }

    public override void Exit()
    {
        owner.Data.rb.MovePosition(owner.transform.position);
        owner.Data.direction = Vector3.zero;
        owner.Data.isMovingDirection = false;
        owner.Data.rb.isKinematic = false;
        owner.Data.isMoving = true;
    }
}
