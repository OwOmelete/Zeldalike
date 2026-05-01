using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;
    [SerializeField] private EnemyChargeAttack charge;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        enemy.SetPlayer(other.transform);
        charge.TryCharge();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        enemy.SetPlayer(null);
    }
}