using UnityEngine;
using System.Collections;

public class EnemyKnockBackAttack : MonoBehaviour, IAttack
{
    public EnemyStatsSO stats;

    [HideInInspector]
    public bool canHit;

    private EnemyController enemyControllerReference;

    private Coroutine attackRoutine;

    void Awake()
    {
        enemyControllerReference = GetComponentInParent<EnemyController>();
    }

    public void TryAttack()
    {
        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        while (enemyControllerReference.Player != null)
        {
            enemyControllerReference.SetState(EnemyController.State.ATTACK);

            yield return new WaitForSeconds(stats.attackCooldown);

            canHit = true;

            Debug.Log("Hit window active");

            yield return new WaitForSeconds(0.2f);

            canHit = false;

            Debug.Log("Hit window ended");

            yield return new WaitForSeconds(0.1f);
        }

        attackRoutine = null;
    }
}