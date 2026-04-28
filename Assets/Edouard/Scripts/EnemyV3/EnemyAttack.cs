using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Config")]
    public EnemyStatsSO stats;

    [Header("References")]
    public GameObject attackZone;
    public Animator animator;

    private Transform player;
    private PlayerHealthComponent playerHealth;
    private EnemyController enemy;

    private Coroutine attackRoutine;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.transform;
        playerHealth = player.GetComponent<PlayerHealthComponent>();

        TryAttack();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TryAttack();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = null;
        playerHealth = null;
    }

    public void TryAttack()
    {
        if (player == null || attackRoutine != null) return;

        enemy.SetState(EnemyController.State.ATTACK);
        attackRoutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        yield return new WaitForSeconds(stats.attackCooldown);

        EndAttack();
    }

    public void DealDamage()
    {
        attackZone.SetActive(true);

        if (playerHealth != null)
            playerHealth.TakeDamage(stats.damage);

        attackZone.SetActive(false);
    }

    public void EndAttack()
    {
        attackRoutine = null;

        if (player != null)
            enemy.SetState(EnemyController.State.CHASE);
        else
            enemy.SetState(EnemyController.State.IDLE);
    }

    void OnDisable()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
    }
}
