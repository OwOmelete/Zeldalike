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
        owner.Data.rb.linearVelocity = Vector3.zero;
        owner.Data.rb.angularVelocity = Vector3.zero;
        owner.gameObject.layer = LayerMask.NameToLayer("InvoDisabled");
        addDispersion();
        int r = Random.Range(0, 100);
        if (r > 80)
        {
            owner.InvoDeactivate();
        }
        else
        {
            if (!owner.Data.isActivated)
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
        if (!owner.Data.isActivated)
        {
            owner.InvoActivate();
        }
        owner.Data.isActivated = true;
        owner.gameObject.layer = LayerMask.NameToLayer("Invo");
    }

    void addDispersion()
    {
        Vector3 dir = Quaternion.Euler(Random.Range(0, 45), Random.Range(0, 360), 0) * Vector3.up;
        owner.Data.rb.AddForce(dir * 5, ForceMode.Impulse);
    }
}
