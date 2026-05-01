using UnityEngine;
using System.Collections;

public class EnemyChargeAttack : MonoBehaviour
{
    [Header("Config")]
    public float windupTime = 2f;
    public float chargeSpeed = 10f;
    public float maxChargeDistance = 10f;

    [Header("Visual")]
    public GameObject previewZone;

    private EnemyController enemy;
    private Transform player;

    private bool isCharging = false;
    private Vector3 chargeDirection;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    public void TryCharge()
    {
        if (enemy.Player == null || isCharging) return;

        isCharging = true;
        player = enemy.Player;

        StartCoroutine(ChargeRoutine());
    }

    IEnumerator ChargeRoutine()
    {
        // 🎯 LOCK direction at START (important)
        chargeDirection = (player.position - transform.position).normalized;

        // 🟥 SHOW preview in that direction
        if (previewZone != null)
        {
            previewZone.SetActive(true);
            previewZone.transform.forward = chargeDirection;
        }

        enemy.SetState(EnemyController.State.ATTACK);

        // ⏳ WINDUP (no tracking anymore)
        yield return new WaitForSeconds(windupTime);

        // 🟥 HIDE preview
        if (previewZone != null)
            previewZone.SetActive(false);

        // 🚀 CHARGE forward in LOCKED direction
        float traveled = 0f;

        while (traveled < maxChargeDistance)
        {
            float step = chargeSpeed * Time.deltaTime;

            transform.position += chargeDirection * step;
            traveled += step;

            yield return null;
        }

        EndCharge();
    }

    void EndCharge()
    {
        isCharging = false;

        if (enemy.Player != null)
            enemy.SetState(EnemyController.State.CHASE);
        else
            enemy.SetState(EnemyController.State.IDLE);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCharging) return;

        // 🎯 HIT PLAYER → damage but KEEP GOING
        if (collision.collider.CompareTag("Player"))
        {
            var health = collision.collider.GetComponent<PlayerHealthComponent>();

            if (health != null)
                health.TakeDamage(enemy.Stats.damage);

            return;
        }

        // 🧱 HIT WALL / OTHER → STOP IMMEDIATELY
        StopAllCoroutines();
        EndCharge();
    }
}