using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
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
    public Animator animator;

    private State currentState;
    public State CurrentState => currentState;

    public Transform Player { get; private set; }

    void Start()
    {
        SetState(State.IDLE);
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (currentState != State.CHASE || Player == null) return;

        Vector3 target = Player.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            Stats.moveSpeed * Time.deltaTime
        );

        Vector3 dir = target - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        UpdateAnimator();

        if (currentState == State.DEATH)
            HandleDeath();
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

    void HandleDeath()
    {
        Destroy(gameObject, 2f);
    }
}