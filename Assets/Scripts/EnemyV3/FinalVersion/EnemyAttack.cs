using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public EnemyStatsSO stats;

    private EnemyController enemy;
    private PlayerHealthComponent playerHealth;

    private Coroutine attackRoutine;

    void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        if (enemy.CurrentState != EnemyController.State.ATTACK)
            return;

        if (attackRoutine == null)
            attackRoutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(stats.attackCooldown);

        if (enemy.Player == null)
        {
            attackRoutine = null;
            yield break;
        }

        playerHealth = enemy.Player.GetComponent<PlayerHealthComponent>();

        if (playerHealth != null)
            playerHealth.TakeDamage(stats.damage);

        yield return new WaitForSeconds(0.2f);

        attackRoutine = null;
    }
}