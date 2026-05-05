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
    private Transform player;

    private bool isCharging = false;
    private Vector3 chargeDirection;

    void Awake()
    {
        enemyControllerReference = GetComponent<EnemyController>();
    }

    public void TryAttack()
    {
        Debug.Log("Trying Attack");
        TryCharge();
    }

    void TryCharge()
    {
        if (enemyControllerReference.Player == null || isCharging) {return;}

        isCharging = true;
        player = enemyControllerReference.Player;

        StartCoroutine(ChargeRoutine());
    }

    IEnumerator ChargeRoutine()
    {
        chargeDirection = (player.position - transform.position).normalized;

        Vector3 lookDir = chargeDirection;
        lookDir.y = 0;

        if (lookDir != Vector3.zero) {transform.rotation = Quaternion.LookRotation(lookDir);}

        if (previewZone != null)
        {
            previewZone.SetActive(true);
            previewZone.transform.forward = chargeDirection;
        }

        enemyControllerReference.SetState(EnemyController.State.ATTACK);

        yield return new WaitForSeconds(windupTime);

        if (previewZone != null) {previewZone.SetActive(false);}

        float traveled = 0f;

        while (traveled < maxChargeDistance)
        {
            float step = chargeSpeed * Time.deltaTime;

            transform.position += chargeDirection * step;
            traveled += step;

            yield return null;
        }

        EndCharge();
        TryCharge();
    }

    void EndCharge()
    {
        isCharging = false;

        if (enemyControllerReference.Player != null)
        {enemyControllerReference.SetState(EnemyController.State.CHASE);}
        
        else {enemyControllerReference.SetState(EnemyController.State.IDLE);}
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            var health = collision.collider.GetComponent<PlayerHealthComponent>();

            if (health != null)
            {
                Debug.Log("giving damage"); 
                health.TakeDamage(enemyControllerReference.Stats.damage);}

            return;
        }

        StopAllCoroutines();
        EndCharge();
    }
}