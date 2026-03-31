using System;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    #region Stats

    [Header("Stats")]
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int currentHealthPoints;
    
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
    [SerializeField] private  StateFlags currentStateFlag;

    #endregion
    
    [Header("Other")]
    [SerializeField] private GameObject detectorGameObject;

    private Transform playerTransform;
    
    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
        HandleEnemyState(StateFlags.IDLE);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (currentHealthPoints == 0)
        {
            HandleEnemyState(StateFlags.DEATH);
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

    public void HandleEnemyState(StateFlags state)
    {
        if (currentStateFlag == state) return;
        
        currentStateFlag = state;
        Debug.Log($"Changed state to {state}");
    }

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
    
}
