using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;
    [SerializeField] private EnemyAttack attack;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        enemy.SetPlayer(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        enemy.SetPlayer(null);
        enemy.SetState(EnemyController.State.IDLE);
    }
}
