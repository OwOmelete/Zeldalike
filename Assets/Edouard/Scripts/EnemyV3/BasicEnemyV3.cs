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

    [Header("Behaviours")]
    public List<EnemyBehaviourSO> behaviours;

    [Header("References")]
    public GameObject attackZone;
    public Slider healthBar;

    public Transform Player { get; private set; }

    public bool IsAttacking { get; private set; }

    private float currentHealth;
    private State currentState;

    private Dictionary<State, EnemyBehaviourSO> behaviourMap;
    private HashSet<int> receivedAttacks = new HashSet<int>();
    private PlayerHealthComponent playerHealth;

    void Start()
    {
        currentHealth = Stats.maxHealth;

        BuildBehaviourMap();
        SetState(State.IDLE);

        UpdateHealthBar();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            SetState(State.DEATH);
        }

        behaviourMap[currentState]?.Execute(this);
    }

    void BuildBehaviourMap()
    {
        behaviourMap = new Dictionary<State, EnemyBehaviourSO>();

        behaviourMap[State.IDLE] = behaviours[0];
        behaviourMap[State.CHASE] = behaviours[1];
        behaviourMap[State.ATTACK] = behaviours[2];
        behaviourMap[State.DEATH] = behaviours[3];
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

    public void TakeDamage(float damage, int attackID)
    {
        if (receivedAttacks.Contains(attackID)) return;

        receivedAttacks.Add(attackID);

        currentHealth -= damage;

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