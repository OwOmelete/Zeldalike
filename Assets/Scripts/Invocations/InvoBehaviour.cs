using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvoBehaviour : MonoBehaviour
{

    public Renderer renderer1;
    public Renderer renderer2;
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
    public ParticleSystem particleSystemFamillier;

    public static event Action<InvoBehaviour> OnInvoSpawn;
    public static event Action<bool> OnInvoActivate;

    public Coroutine delay;

    private void OnEnable()
    {
        InvoManager.OnLock += HandleLock;
        InvoManager.OnDelock += HandleDelock;
        SceneManager.sceneLoaded += OnSceneLoaded;
        transform.SetParent(Invo.Instance.transform);
    }

   

    private void OnDisable()
    {
        InvoManager.OnLock -= HandleLock;
        InvoManager.OnDelock -= HandleDelock;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        player = InvoManager.Instance.transform;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        
        player = InvoManager.Instance.transform;
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

        particleSystemFamillier.Play();

        if (Data.currentState != null)
            Data.currentState.Exit();

        Data.currentState = newState;
        Data.currentState.Enter();
        st = Data.currentState.ToString();
    }

    public void ChangeTemperature(InvoDataInstance.temperature temp)
    {
        Data.currentTemperature = temp;
    }

    public void resetPosition()
    {
        transform.position = Data.target.position + Data.offset;
    }
    
    public void startAttackDelay()
    {
        if (delay != null)
        {
            StopCoroutine(delay);
        }
        delay = StartCoroutine(attackDelay(Data.attackDelay));
    }
    
    
    public void SetAttackID(int id)
    {
        attackID = id;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Boss"))
        {
            BossManager boss = other.GetComponent<BossManager>();

            if (boss != null && Data.currentState == stateAttack)
                {
                     ChangeState(stateDisabled);
                     boss.TakeDamage(Data.damage, attackID);
                }
        }*/
        if (other.gameObject.CompareTag("Enemy") && Data.currentState == stateAttack)
        {
            
            IDamagable damagable = other.GetComponent<IDamagable>();

            if (damagable != null && Data.currentState == stateAttack)
            {
                damagable.TakeDamage(Data.damage, attackID, Data);
                ChangeState(stateDisabled);
            }
        }

        if (other.CompareTag("Dummy"))
        {
            if (Data.currentState == stateAttack)
            {
                ChangeState(stateDisabled);
            }
        }
        if (other.gameObject.CompareTag("CollectZone"))
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

    private void HandleDelock()
    {
        Data.ennemyTarget = null;
    }
    
    #region coroutines
    IEnumerator attackDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
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
        yield return new WaitForSeconds(1);
        ChangeState(stateIdle);
    }

    #endregion
    
}
