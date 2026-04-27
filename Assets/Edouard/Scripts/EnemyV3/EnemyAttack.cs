using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Config")]
    public float attackRadius;
    public EnemyStatsSO stats;

    [Header("References")]
    public GameObject attackZone;

    private Transform player;
    private PlayerHealthComponent playerHealth;
    private EnemyController enemy;

    public bool IsAttacking { get; private set; }

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    public void SetTarget(Transform target)
    {
        player = target;

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealthComponent>();
    }

    void Update()
    {
        if (player == null || IsAttacking) return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRadius)
        {
            enemy.SetState(EnemyController.State.ATTACK);
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        IsAttacking = true;

        attackZone.SetActive(true);
        yield return new WaitForSeconds(stats.attackCooldown);

        attackZone.SetActive(false);

        if (playerHealth != null)
            playerHealth.TakeDamage(stats.damage);

        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }
}