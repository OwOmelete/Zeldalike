using UnityEngine;

public class StateDisabled : IState
{

    private InvoBehaviour owner;

    
    public StateDisabled(InvoBehaviour owner)
    {
        this.owner = owner;
    }
    
    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
