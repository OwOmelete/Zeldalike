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
        enemy = GetComponentInParent<EnemyController>();

        if (enemy == null)
            Debug.LogError("EnemyController not found on " + gameObject.name);
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
        enemy.SetState(EnemyController.State.ATTACK);

        Vector3 targetPosition = enemy.Player.position;

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        var p = proj.GetComponent<EnemyProjectile>();

        if (p == null)
        {
            Debug.LogError("Projectile missing EnemyProjectile script");
            yield break;
        }

        p.Init(targetPosition, enemy.Stats.damage);

        yield return new WaitForSeconds(0.3f);

        _attackRoutine = null;

        if (enemy.Player != null)
            enemy.SetState(EnemyController.State.CHASE);
        else
            enemy.SetState(EnemyController.State.IDLE);
    }
}