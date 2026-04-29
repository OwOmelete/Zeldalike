using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Enemies/Behaviours/RangedAttack")]
public class RangedAttackBehaviour : EnemyBehaviourSO
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 10f;
    public float cooldown = 1.5f;

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

        enemy.StartCoroutine(Fire(enemy));
        _lastAttackTime = Time.time;
    }

    IEnumerator Fire(EnemyController enemy)
    {
        enemy.SetState(EnemyController.State.ATTACK);

        yield return new WaitForSeconds(0.2f);

        Transform point = firePoint != null ? firePoint : enemy.transform;

        GameObject proj = Object.Instantiate(
            projectilePrefab,
            point.position,
            Quaternion.identity
        );

        var p = proj.GetComponent<EnemyProjectile>();
        var playerHealth = enemy.Player.GetComponent<PlayerHealthComponent>();

        p.Init(enemy.Player, playerHealth);

        yield return new WaitForSeconds(0.3f);

        enemy.SetState(
            enemy.Player != null
                ? EnemyController.State.CHASE
                : EnemyController.State.IDLE
        );
    }
}