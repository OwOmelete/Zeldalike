using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
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
    [SerializeField] private StateFlags currentStateFlag;

    #endregion


    #region References

    [Header("References")]
    [SerializeField] private GameObject detectorGameObject;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private Image targetUI;

    #endregion


    #region Runtime

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
        if (receivedAttacks.Contains(attackID))
        {
            return;
        }

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
        Debug.Log("Current behaviour is idle");
    }

    private void ChasingBehavior()
    {
        Debug.Log("Current behaviour is chasing");
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
            playerTransform.position, moveSpeed * Time.deltaTime);
        gameObject.transform.LookAt(playerTransform);
    }
    private void AttackingBehavior()
    {
        Debug.Log("Current behaviour is attacking");
    }

    private void DeathBehavior()
    {
        Debug.Log("Current behaviour is death");
        Destroy(gameObject);
    }
    #endregion
}
