using UnityEngine;

public class NewEnemyController : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private Animator animator;

    private IAttack attack;

    private EnemyChase chase;

    public enum States
    {
        IDLE,
        CHASING,
        ATTACKING,
        DYING
    }

    private States currentState;

    public Transform Player { get; private set; }

    private void Awake()
    {
        attack = GetComponent<IAttack>();

        chase = GetComponent<EnemyChase>();
    }

    private void Start()
    {
        SetState(States.IDLE);
    }

    public void SetPlayer(Transform player)
    {
        Player = player;
    }

    public void SetState(States newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case States.IDLE:

                chase?.StopChase();

                break;

            case States.CHASING:

                //animator.SetBool("IsChasing", true);

                chase?.StartChase(Player);

                break;

            case States.ATTACKING:

                //animator.SetTrigger("Attack");

                attack?.TryAttack();

                break;

            case States.DYING:

                //animator.SetTrigger("Dying");

                HandleDeath();

                break;
        }
    }

    private void HandleDeath()
    {
        if (attack is NewGueulaconAttack gueulaconAttack)
        {
            gueulaconAttack.StopAttack();
        }

        chase?.StopChase();

        Destroy(gameObject, 2f);
    }
}