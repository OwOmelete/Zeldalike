using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyV3 : MonoBehaviour, IDamagable
{
    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        DEATH
    }

    [Header("Config")]
    public EnemyStatsSO Stats;

    [Header("References")]
    public GameObject attackZone;
    public Slider healthBar;
    public EnnemyHeatSystem HeatSystem;
    public Transform firePoint;

    public Transform Player { get; private set; }

    public bool IsAttacking { get; set; }

    private float currentHealth;
    private State currentState;

    private Dictionary<State, EnemyBehaviourSO> behaviourMap;
    
    [Header("Behaviours")]
    public EnemyBehaviourSO idleBehaviour;
    public EnemyBehaviourSO chaseBehaviour;
    public EnemyBehaviourSO attackBehaviour;
    public EnemyBehaviourSO deathBehaviour;    private HashSet<int> receivedAttacks = new HashSet<int>();
    private PlayerHealthComponent playerHealth;

    void Awake()
    {
        BuildBehaviourMap();
    }
    
    void Start()
    {
        currentHealth = Stats.maxHealth;

        SetState(State.IDLE);

        UpdateHealthBar();
    }

    void Update()
    {
        if (behaviourMap == null)
        {
            Debug.LogError($"{name} behaviourMap is NULL");
            return;
        }

        if (!behaviourMap.ContainsKey(currentState))
        {
            Debug.LogError($"{name} missing state in map: {currentState}");
            return;
        }

        if (behaviourMap[currentState] == null)
        {
            Debug.LogError($"{name} has NULL behaviour for state: {currentState}");
            return;
        }

        behaviourMap[currentState].Execute(this);
        if (currentHealth <= 0)
        {
            SetState(State.DEATH);
        }

        behaviourMap[currentState]?.Execute(this);
    }

    void BuildBehaviourMap()
    {
        if (idleBehaviour == null)
            Debug.LogError($"{name} idleBehaviour is NULL");

        if (chaseBehaviour == null)
            Debug.LogError($"{name} chaseBehaviour is NULL");

        if (attackBehaviour == null)
            Debug.LogError($"{name} attackBehaviour is NULL");

        if (deathBehaviour == null)
            Debug.LogError($"{name} deathBehaviour is NULL");

        behaviourMap = new Dictionary<State, EnemyBehaviourSO>();

        behaviourMap[State.IDLE] = idleBehaviour;
        behaviourMap[State.CHASE] = chaseBehaviour;
        behaviourMap[State.ATTACK] = attackBehaviour;
        behaviourMap[State.DEATH] = deathBehaviour;
    }
    public void SetState(State newState)
    {
        currentState = newState;
    }

    public void SetPlayer(Transform player)
    {
        Player = player;

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealthComponent>();
    }

    public IEnumerator AttackRoutine()
    {
        IsAttacking = true;

        attackZone.SetActive(true);
        yield return new WaitForSeconds(Stats.attackCooldown);

        attackZone.SetActive(false);

        if (playerHealth != null)
            playerHealth.TakeDamage(Stats.damage);

        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }

    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID)) return;

        receivedAttacks.Add(attackID);

        currentHealth -= damage;

        switch (data.currentTemperature)
        {
            case InvoDataInstance.temperature.veryCold:
                HeatSystem.reduceHeat(data.veryColdValue);
                break;
            case InvoDataInstance.temperature.cold:
                HeatSystem.reduceHeat(data.coldValue);
                break;
            case InvoDataInstance.temperature.hot:
                HeatSystem.increaseHeat(data.hotValue);
                break;
            case InvoDataInstance.temperature.veryHot:
                HeatSystem.increaseHeat(data.veryHotValue);
                break;
        }

        if (currentHealth <= 0)
        {
            SetState(State.DEATH);
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.value = currentHealth / Stats.maxHealth;
    }
}