using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InvoBehaviour : MonoBehaviour
{
    public InvoData _Invo;
    public InvoDataInstance Data;
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
        InvoManager.OnDelock += HandleDelock;
    }

    private void OnDisable()
    {
        InvoManager.OnLock -= HandleLock;
        InvoManager.OnDelock -= HandleDelock;
    }

    public void InvoActivate()
    {
        Data.isActivated = true;
        OnInvoActivate?.Invoke(true);
    }

    public void InvoDeactivate()
    {
        Data.isActivated = false;
        OnInvoActivate?.Invoke(false);
    }
    
    private void Start()
    {
        Data = _Invo.Instance();
        
        OnInvoSpawn?.Invoke(this);
        
        Data.rb = rb;

        stateIdle = new StateIdle(this);
        stateProtection = new StateProtection(this);
        stateAttack = new StateAttack(this);
        stateDisabled = new StateDisabled(this);
        
        ChangeState(stateIdle);
    }

    private void FixedUpdate()
    {
        Data.currentState.Execute();
    }

    public void ChangeState(IState newState)
    {
        StopAllCoroutines();
        if (Data.currentState != null)
            Data.currentState.Exit();

        Data.currentState = newState;
        Data.currentState.Enter();
        st = Data.currentState.ToString();
    }

    public void resetPosition()
    {
        transform.position = Data.target.position + Data.offset;
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

            if (enemy != null && Data.currentState == stateAttack)
            {
                enemy.TakeDamage(Data.damage, attackID);
                ChangeState(stateDisabled);
            }
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            if (Data.currentState == stateAttack)
            {
                ChangeState(stateDisabled);
                StopCoroutine(attackDelay());
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (Data.currentState == stateDisabled)
            {
                ChangeState(stateIdle);
            }
        }

        if (other.CompareTag("DeathZone"))
        {
            resetPosition();
        }
    }
    
    private void HandleLock(Transform transform)
    {
        Data.ennemyTarget = transform;
    }

    private void HandleDelock(Transform t)
    {
        Data.ennemyTarget = null;
    }
    
    #region coroutines
    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(2);
        Data.rb.isKinematic = true;
        Data.isMovingDirection = true;
        if (Data.ennemyTarget == null)
        {
            Data.direction = player.forward;
        }
        else
        {
            Data.direction = (Data.ennemyTarget.position - transform.position).normalized;
        }
        
        
        yield return new WaitForSeconds(1);
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
