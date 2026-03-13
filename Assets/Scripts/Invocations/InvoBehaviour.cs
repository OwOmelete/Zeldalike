using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    public InvoData _Invo;
    public InvoDataInstance _InvoInstance;
    public Transform player;
    public string st;
    
    [SerializeField] private Rigidbody rb;

    public StateIdle stateIdle;
    public StateProtection stateProtection;
    public StateAttack stateAttack;
    public StateDisabled stateDisabled;

    public static event Action<InvoBehaviour> OnInvoSpawn;
    
    private void Start()
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
        StopAllCoroutines();
        if (_InvoInstance.currentState != null)
            _InvoInstance.currentState.Exit();

        _InvoInstance.currentState = newState;
        _InvoInstance.currentState.Enter();
        st = _InvoInstance.currentState.ToString();
    }

    public void startAttackDelay()
    {
        StopCoroutine(attackDelay());
        StartCoroutine(attackDelay());
    }
    
    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(2);
        _InvoInstance.rb.isKinematic = true;
        _InvoInstance.isMovingDirection = true;
        
        yield return new WaitForSeconds(10);
        ChangeState(stateIdle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (_InvoInstance.currentState == stateAttack)
            {
                ChangeState(stateIdle);
                StopCoroutine(attackDelay());
            }
        }
    }

}
