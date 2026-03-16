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
    private int attackID;
    
    [SerializeField] private Rigidbody rb;

    public StateIdle stateIdle;
    public StateProtection stateProtection;
    public StateAttack stateAttack;
    public StateDisabled stateDisabled;

    public static event Action<InvoBehaviour> OnInvoSpawn;
    public static event Action<bool> OnInvoActivate;

    private void OnEnable()
    {
        InvoManager.OnLock += HandleLock;
    }

    private void OnDisable()
    {
        InvoManager.OnLock -= HandleLock;
    }

    public void InvoActivate()
    {
        _InvoInstance.isActivated = true;
        OnInvoActivate?.Invoke(true);
    }

    public void InvoDeactivate()
    {
        _InvoInstance.isActivated = false;
        OnInvoActivate?.Invoke(false);
    }
    
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
    
    
    public void SetAttackID(int id)
    {
        attackID = id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(_InvoInstance.damage, attackID);
                ChangeState(stateDisabled);
            }
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            if (_InvoInstance.currentState == stateAttack)
            {
                ChangeState(stateDisabled);
                StopCoroutine(attackDelay());
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (_InvoInstance.currentState == stateDisabled)
            {
                ChangeState(stateIdle);
            }
        }

    }
    
    private void HandleLock(Transform transform)
    {
        _InvoInstance.ennemyTarget = transform;
    }
    
    #region coroutines
    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(2);
        _InvoInstance.rb.isKinematic = true;
        _InvoInstance.isMovingDirection = true;
        _InvoInstance.direction = (_InvoInstance.ennemyTarget.position - transform.position).normalized;
        
        yield return new WaitForSeconds(3);
        ChangeState(stateDisabled);
    }

    public void startReactivationDelay()
    {
        StartCoroutine(reactivationDelay());
    }
    
    IEnumerator reactivationDelay()
    {
        yield return new WaitForSeconds(2);
        ChangeState(stateIdle);
    }

    #endregion
    
}
