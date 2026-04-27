using UnityEngine;

public class DetectorLogicV3 : MonoBehaviour
{
    [SerializeField] private BasicEnemyV3 enemy;
    [SerializeField] private float attackRadius;

    private Transform playerTransform;
    private bool isPlayerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isPlayerInRange = true;

            enemy.SetPlayer(playerTransform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            isPlayerInRange = false;

            enemy.SetPlayer(null);
        }
    }

    private void Update()
    {
        if (!isPlayerInRange || playerTransform == null)
        {
            enemy.SetState(BasicEnemyV3.State.IDLE);
            return;
        }

        float distance = Vector3.Distance(
            enemy.transform.position,
            playerTransform.position
        );

        if (enemy.IsAttacking) return;

        if (distance > attackRadius)
            enemy.SetState(BasicEnemyV3.State.CHASE);
        else
            enemy.SetState(BasicEnemyV3.State.ATTACK);
    }
}