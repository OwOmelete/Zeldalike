using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        DEATH
    }

    [Header("References")]
    public Animator animator;

    [Header("Behaviours")]
    public EnemyBehaviourSO idleBehaviour;
    public EnemyBehaviourSO chaseBehaviour;
    public EnemyBehaviourSO attackBehaviour;
    public EnemyBehaviourSO deathBehaviour;

    private Dictionary<State, EnemyBehaviourSO> behaviourMap;
    private State currentState;

    public Transform Player { get; private set; }

    void Awake()
    {
        behaviourMap = new Dictionary<State, EnemyBehaviourSO>()
        {
            { State.IDLE, idleBehaviour },
            { State.CHASE, chaseBehaviour },
            { State.ATTACK, attackBehaviour },
            { State.DEATH, deathBehaviour }
        };
    }

    void Start()
    {
        SetState(State.IDLE);
    }

    void Update()
    {
        behaviourMap[currentState]?.Execute(this);
    }

    public void SetState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        UpdateAnimator();
    }

    public void SetPlayer(Transform player)
    {
        Player = player;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetBool("IsChasing", currentState == State.CHASE);
        animator.SetBool("IsDead", currentState == State.DEATH);

        if (currentState == State.ATTACK)
            animator.SetTrigger("Attack");
    }
}
