using UnityEngine;
using System.Collections;

public class FonceurAttack : MonoBehaviour, IAttack
{
    [Header("Config")]
    public float windupTime = 2f;
    public float chargeSpeed = 10f;
    public float maxChargeDistance = 10f;

    [Header("Visual")]
    public GameObject previewZone;

    private EnemyController enemyControllerReference;

    private Rigidbody rb;

    private Transform player;

    private bool isCharging = false;

    private Vector3 chargeDirection;

    private Coroutine chargeRoutine;

    void Awake()
    {
        enemyControllerReference = GetComponent<EnemyController>();

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("FonceurAttack requires a Rigidbody");
        }
    }

    public void TryAttack()
    {
        if (!isCharging)
        {
            TryCharge();
        }
    }

    void TryCharge()
    {
        if (enemyControllerReference.Player == null || isCharging)
        {
            return;
        }

        isCharging = true;

        player = enemyControllerReference.Player;

        chargeRoutine = StartCoroutine(ChargeRoutine());
    }

    IEnumerator ChargeRoutine()
    {
        chargeDirection = player.position - transform.position;

        chargeDirection.y = 0f;

        chargeDirection.Normalize();

        if (chargeDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(chargeDirection);
        }

        if (previewZone != null)
        {
            previewZone.SetActive(true);
            previewZone.transform.forward = chargeDirection;
        }

        enemyControllerReference.SetState(EnemyController.State.ATTACK);

        yield return new WaitForSeconds(windupTime);

        if (previewZone != null)
        {
            previewZone.SetActive(false);
        }

        float traveled = 0f;

        while (traveled < maxChargeDistance)
        {
            float step = chargeSpeed * Time.deltaTime;

            rb.MovePosition(rb.position + chargeDirection * step);

            traveled += step;

            yield return null;
        }

        EndCharge();

        TryCharge();
    }

    void EndCharge()
    {
        isCharging = false;

        if (chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
            chargeRoutine = null;
        }

        if (enemyControllerReference.Player != null)
        {
            enemyControllerReference.SetState(EnemyController.State.CHASE);
        }
        else
        {
            enemyControllerReference.SetState(EnemyController.State.IDLE);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCharging) return;

        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealthComponent health =
                collision.collider.GetComponent<PlayerHealthComponent>();

            if (health != null)
            {
                Debug.Log("Giving damage");

                health.TakeDamage(enemyControllerReference.Stats.damage);
            }

            EndCharge();

            return;
        }

        EndCharge();
    }
}