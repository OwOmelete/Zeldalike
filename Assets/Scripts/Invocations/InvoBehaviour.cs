using System;
using System.Collections;
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
    public StateAttack stateAttack;
    public StateDisabled stateDisabled;

    public static event Action<InvoBehaviour> OnInvoSpawn;
    
    private void Awake()
    {
        _InvoInstance = _Invo.Instance();
        
        OnInvoSpawn?.Invoke(this);
        
        _InvoInstance.rb = rb;

        stateIdle = new StateIdle(this);
        stateProtection = new StateProtection(this);
        stateAttack = new StateAttack(this);
        stateDisabled = new StateDisabled(this);
        
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

    public void startAttackDelay()
    {
        StartCoroutine(attackDelay());
    }
    
    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(2);
        _InvoInstance.isMoving = false;
        _InvoInstance.rb.useGravity = true;
        _InvoInstance.rb.AddForce(player.transform.forward * 50, ForceMode.Impulse);
        yield return new WaitForSeconds(2);
        ChangeState(stateIdle);
    }
    
    

}
