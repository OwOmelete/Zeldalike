using System;
using Unity.VisualScripting;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    public InvoData _Invo;
    public InvoDataInstance _InvoInstance;
    public Transform player;
    
    [SerializeField] private Rigidbody rb;

    public StateIdle stateIdle;
    public StateProtection stateProtection;

    public static event Action<InvoBehaviour> OnInvoSpawn;
    
    private void Awake()
    {
        _InvoInstance = _Invo.Instance();
        
        OnInvoSpawn?.Invoke(this);
        
        _InvoInstance.rb = rb;

        stateIdle = new StateIdle(this);
        stateProtection = new StateProtection(this);
        
        Debug.Log(stateIdle);
        
        ChangeState(stateIdle);
    }

    private void FixedUpdate()
    {
        _InvoInstance.currentState.Execute();
    }

    public void ChangeState(IState newState)
    {
        if (_InvoInstance.currentState != null)
            _InvoInstance.currentState.Exit();

        _InvoInstance.currentState = newState;
        _InvoInstance.currentState.Enter();
    }
    
    

}
