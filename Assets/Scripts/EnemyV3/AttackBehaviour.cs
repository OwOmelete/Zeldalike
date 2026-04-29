using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Enemies/Behaviours/MeleeAttack")]
public class AttackBehaviour : EnemyBehaviourSO
{
    public float attackRange = 2f;
    public float cooldown = 1f;

    private float _lastAttackTime;

    public override void Execute(EnemyController enemy)
    {
        if (enemy.Player == null) return;

        float distance = Vector3.Distance(
            enemy.transform.position,
            enemy.Player.position
        );

        if (distance > attackRange)
        {
            enemy.SetState(EnemyController.State.CHASE);
            return;
        }

        if (Time.time < _lastAttackTime + cooldown) return;

        enemy.StartCoroutine(Attack(enemy));
        _lastAttackTime = Time.time;
    }

    IEnumerator Attack(EnemyController enemy)
    {
        enemy.SetState(EnemyController.State.ATTACK);

        yield return new WaitForSeconds(0.2f);

        var playerHealth = enemy.Player.GetComponent<PlayerHealthComponent>();
        if (playerHealth != null)
            playerHealth.TakeDamage(enemy.Stats.damage);

        yield return new WaitForSeconds(0.3f);

        enemy.SetState(
            enemy.Player != null
                ? EnemyController.State.CHASE
                : EnemyController.State.IDLE
        );
    }
}