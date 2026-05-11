using UnityEngine;

public class NewEnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    public enum States
    {
        IDLE,
        ATTACKING,
        DYING
    }

    private States currentState;

    private IAttack attack;

    private void Awake()
    {
        attack = GetComponent<IAttack>();
    }

    private void Start()
    {
        SetState(States.IDLE);
    }

    public void SetState(States newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        StateBehavior();
    }

    private void StateBehavior()
    {
        switch (currentState)
        {
            case States.IDLE:
                break;

            case States.ATTACKING:
                animator.SetTrigger("Attack");

                TryAttack();
                break;

            case States.DYING:
                animator.SetTrigger("Dying");

                TryDie();
                break;
        }
    }

    private void TryAttack()
    {
        attack?.TryAttack();
    }

    private void TryDie()
    {
        Destroy(gameObject, 2f);
    }
}