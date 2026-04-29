using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;
    [SerializeField] private float attackRange = 2f;

    private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.transform;
        enemy.SetPlayer(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = null;
        enemy.SetPlayer(null);
        enemy.SetState(EnemyController.State.IDLE);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(
            enemy.transform.position,
            player.position
        );

        if (dist <= attackRange)
        {
            enemy.SetState(EnemyController.State.ATTACK);
        }
        else
        {
            enemy.SetState(EnemyController.State.CHASE);
        }
    }
}