using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        enemy.SetPlayer(other.transform);
        enemy.SetState(EnemyController.State.CHASE);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        enemy.SetPlayer(null);
        enemy.SetState(EnemyController.State.IDLE);
    }
}