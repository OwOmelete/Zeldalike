using System.Collections;
using UnityEngine;
using static System.Random;

public class StateDisabled : IState
{

    private InvoBehaviour owner;
    
    public StateDisabled(InvoBehaviour owner)
    {
        this.owner = owner;
    }
    
    public override void Enter()
    {
        owner._InvoInstance.rb.linearVelocity = Vector3.zero;
        owner._InvoInstance.rb.angularVelocity = Vector3.zero;
        owner.gameObject.layer = LayerMask.NameToLayer("InvoDisabled");
        addDispersion();
        int r = Random.Range(0, 100);
        if (r > 80)
        {
            owner.InvoDeactivate();
        }
        else
        {
            if (!owner._InvoInstance.isActivated)
            {
                owner.InvoActivate();
            }
            owner.startReactivationDelay();
        }
        
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        if (!owner._InvoInstance.isActivated)
        {
            owner.InvoActivate();
        }
        owner._InvoInstance.isActivated = true;
        owner.gameObject.layer = LayerMask.NameToLayer("Invo");
    }

    void addDispersion()
    {
        Vector3 dir = Quaternion.Euler(Random.Range(0, 45), Random.Range(0, 360), 0) * Vector3.up;
        owner._InvoInstance.rb.AddForce(dir * 3, ForceMode.Impulse);
    }
}
