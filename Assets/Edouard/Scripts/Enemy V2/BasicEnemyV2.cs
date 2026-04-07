using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BasicEnemyV2 : MonoBehaviour
{
    #region Unity Variables

    #region Stats

    [Header("Stats")]
    [SerializeField] private float maxHealthPoints;
    [SerializeField] private float currentHealthPoints;
    [SerializeField] private int damagePoints;
    [SerializeField] private int moveSpeed;

    #endregion


    #region StateFlags

    public enum StateFlags
    {
        IDLE,
        CHASING,
        ATTACKING,
        DEATH
    }

    [Header("StateFlags")]
    [SerializeField] public StateFlags currentStateFlag;

    public bool isAttacking;
    
    #endregion


    #region Timers

    [Header("Timers")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackPreparationCooldown;

    #endregion


    #region Other

    [Header("Other")]
    [SerializeField] private GameObject detectorGameObject;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private Image targetUI;

    private Transform playerTransform;
    private GameObject mainCamera;
    private HashSet<int> receivedAttacks = new HashSet<int>();

    #endregion

    #endregion
    
    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
        HandleEnemyState(StateFlags.IDLE);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        UpdateHealthBar();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        enemyCanvas.transform.LookAt(mainCamera.transform);
        if (currentHealthPoints == 0)
        {
            HandleEnemyState(StateFlags.DEATH);
        }
        else
        {
            UpdateHealthBar();
        }
        switch (currentStateFlag)
        {
            case StateFlags.IDLE:
                IdleBehavior();
                break;
            case StateFlags.CHASING:
                ChasingBehavior();
                break;
            case StateFlags.ATTACKING:
                AttackingBehavior();
                break;
            case StateFlags.DEATH:
                DeathBehavior();
                break;
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealthPoints / maxHealthPoints;
    }

    public void TakeDamage(float damage, int attackID)
    {
        if (receivedAttacks.Contains(attackID)) {return;}

        receivedAttacks.Add(attackID);

        currentHealthPoints -= damage;

        if (currentHealthPoints <= 0)
        {
            HandleEnemyState(StateFlags.DEATH);
        }
        else
        {
            UpdateHealthBar();
        }
        Debug.Log($"Enemy took {damage} damage");
    }

    public void HandleEnemyState(StateFlags state)
    {
        if (currentStateFlag == state) return;
        
        currentStateFlag = state;
        Debug.Log($"Changed state to {state}");
    }

    #region Behaviours
    private void IdleBehavior()
    {
        //Debug.Log("Current behaviour is idle");
    }

    private void ChasingBehavior()
    {
        //Debug.Log("Current behaviour is chasing");
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
            playerTransform.position, moveSpeed * Time.deltaTime);
        gameObject.transform.LookAt(playerTransform);
    }
    private void AttackingBehavior()
    {
        if (!isAttacking)
            StartCoroutine(AttackRoutine());
    }

    private void DeathBehavior()
    {
        //Debug.Log("Current behaviour is death");
        Destroy(gameObject);
    }
    #endregion

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        
        Debug.Log("Preparing attack...");
        yield return new WaitForSeconds(attackPreparationCooldown);

        Debug.Log("Attacking");

        yield return new WaitForSeconds(attackCooldown);
        Debug.Log("Attack cooldown ended");
        
        isAttacking = false;
    }
    
}
