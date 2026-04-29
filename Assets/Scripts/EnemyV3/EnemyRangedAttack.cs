using UnityEngine;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float cooldown = 1.5f;

    private float _lastAttackTime;
    private Coroutine _attackRoutine;

    private EnemyController enemy;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    public void TryFire()
    {
        if (enemy.Player == null) return;
        if (_attackRoutine != null) return;

        if (Time.time < _lastAttackTime + cooldown) return;

        _attackRoutine = StartCoroutine(Fire());
        _lastAttackTime = Time.time;
    }

    IEnumerator Fire()
    {
        // optional: trigger animation
        enemy.SetState(EnemyController.State.ATTACK);

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        var p = proj.GetComponent<EnemyProjectile>();
        var playerHealth = enemy.Player.GetComponent<PlayerHealthComponent>();

        p.Init(enemy.Player, playerHealth);

        yield return new WaitForSeconds(0.3f);

        _attackRoutine = null;
    }
}