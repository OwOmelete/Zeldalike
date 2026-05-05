using UnityEngine;
using System.Collections;

public class EnemyKnockBackAttack : MonoBehaviour
{
    public EnemyStatsSO stats;

    private EnemyController enemyControllerReference;
    private PlayerHealthComponent playerHealth;

    private Coroutine attackRoutine;

    void Awake()
    {
        enemyControllerReference = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        if (enemyControllerReference.CurrentState != EnemyController.State.ATTACK) {return;}

        if (attackRoutine == null) {attackRoutine = StartCoroutine(AttackRoutine());}
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(stats.attackCooldown);

        if (enemyControllerReference.Player == null)
        {
            attackRoutine = null;
            yield break;
        }

        playerHealth = enemyControllerReference.Player.GetComponent<PlayerHealthComponent>();

        if (playerHealth != null) {playerHealth.TakeDamage(stats.damage);}

        yield return new WaitForSeconds(0.2f);

        attackRoutine = null;
    }
}