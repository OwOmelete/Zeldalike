using UnityEngine;
using System.Collections;

public class EnemyChargeAttack : MonoBehaviour
{
    public float windupTime = 2f;
    public float chargeSpeed = 10f;
    public float maxChargeDistance = 10f;

    public GameObject previewZone;

    private EnemyController enemy;
    private Transform player;

    private bool isCharging = false;
    private Vector3 chargeDirection;
    private Vector3 startPosition;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    public void TryCharge()
    {
        if (enemy.Player == null || isCharging) return;

        player = enemy.Player;
        StartCoroutine(ChargeRoutine());
    }

    IEnumerator ChargeRoutine()
    {
        isCharging = true;

        chargeDirection = (player.position - transform.position).normalized;
        startPosition = transform.position;

        if (previewZone != null)
        {
            previewZone.SetActive(true);
            previewZone.transform.forward = chargeDirection;
        }

        enemy.SetState(EnemyController.State.ATTACK);

        yield return new WaitForSeconds(windupTime);

        if (previewZone != null)
            previewZone.SetActive(false);

        while (Vector3.Distance(startPosition, transform.position) < maxChargeDistance)
        {
            transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
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

        if (collision.collider.CompareTag("Player"))
        {
            var health = collision.collider.GetComponent<PlayerHealthComponent>();
            if (health != null)
                health.TakeDamage(enemy.Stats.damage);

            return;
        }

        StopAllCoroutines();
        EndCharge();
    }
}