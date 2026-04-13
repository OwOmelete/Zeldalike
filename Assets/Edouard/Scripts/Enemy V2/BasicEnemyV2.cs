using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BasicEnemyV2 : MonoBehaviour, IDamagable
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
    [SerializeField] private float attackPreparationCooldown;

    #endregion

    #region Other

    [Header("Other")]
    [SerializeField] private GameObject detectorGameObject;
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private GameObject attackZone;
    public Slider healthBar;
    public Image targetUI;

    private Transform playerTransform;
    private Transform cameraTransform;
    private HashSet<int> receivedAttacks = new HashSet<int>();

    #endregion

    #endregion

    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
        HandleEnemyState(StateFlags.IDLE);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        UpdateHealthBar();
    }

    private void Update()
    {
        // enemyCanvas.transform.LookAt(cameraTransform);

        if (currentHealthPoints <= 0f)
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

    public void HandleEnemyState(StateFlags state)
    {
        if (currentStateFlag == state) return;

        currentStateFlag = state;
        Debug.Log($"Changed state to {state}");
    }

    #region Behaviours

    private void IdleBehavior()
    {
    }

    private void ChasingBehavior()
    {
        Vector3 targetPosition = playerTransform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        transform.LookAt(targetPosition);

        GlobalEvents.EnemyMove();
    }

    private void AttackingBehavior()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void DeathBehavior()
    {
        Destroy(gameObject);
    }

    #endregion

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        Debug.Log("Preparing attack...");
        attackZone.SetActive(true);

        yield return new WaitForSeconds(attackPreparationCooldown);

        attackZone.SetActive(false);

        Debug.Log("Attacking");
        GlobalEvents.EnemyAttack();

        yield return new WaitForSeconds(0.8f);

        isAttacking = false;
    }

    public void TakeDamage(float damage, int attackID)
    {
        if (receivedAttacks.Contains(attackID)) { return; }

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
}
