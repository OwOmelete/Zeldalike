/*using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/RangedAttack")]
public class RangedAttackBehaviour : EnemyBehaviourSO
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float cooldown = 1.5f;

    private float lastAttackTime;

    public override void Execute(BasicEnemyV3 enemy)
    {
        if (enemy.Player == null) return;
        if (enemy.IsAttacking) return;

        if (Time.time < lastAttackTime + cooldown) return;

        enemy.StartCoroutine(Fire(enemy));
        lastAttackTime = Time.time;
    }

    private IEnumerator Fire(BasicEnemyV3 enemy)
    {
        enemy.IsAttacking = true;

        GameObject proj = Object.Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        EnemyProjectile p = proj.GetComponent<EnemyProjectile>();

        var playerHealth = enemy.Player.GetComponent<PlayerHealthComponent>();

        p.Init(enemy.Player, playerHealth);

        yield return new WaitForSeconds(0.3f);

        enemy.IsAttacking = false;
    }
}*/