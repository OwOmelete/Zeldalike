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
        if (owner.Data.direction != Vector3.zero && owner.Data.isMovingDirection)
        {
            //owner.transform.position += owner._InvoInstance.direction;
            SafeMove(owner.Data.direction);
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

    
    private void SafeMove(Vector3 direction)
    {
        Vector3 move = direction * (25 * Time.fixedDeltaTime);
        float distance = move.magnitude;

        Rigidbody rb = owner.Data.rb;

        if (rb.SweepTest(direction, out RaycastHit hit, distance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Vector3 safePosition =
                    hit.point - direction * 0.05f;

                rb.MovePosition(safePosition);

                // Stop attaque au contact
                owner.ChangeState(owner.stateDisabled);
                return;
            }
            
        }

        rb.MovePosition(rb.position + move);
    }


  
    public override void Exit()
    {
        owner.Data.rb.MovePosition(owner.transform.position);
        owner.Data.direction = Vector3.zero;
        owner.Data.isMovingDirection = false;
        owner.Data.rb.isKinematic = false;
        owner.Data.isMoving = true;
        owner.Data.acceleration = owner._Invo.acceleration;
    }
}
