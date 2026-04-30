using UnityEngine;

public class EnemyDetectorTurret : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;
    [SerializeField] private EnemyRangedAttack rangedAttack;

    private Transform player;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.transform;
        enemy.SetPlayer(player);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = null;
        enemy.SetPlayer(null);
        enemy.SetState(EnemyController.State.IDLE);
    }

    void Update()
    {
        if (player == null) return;

        enemy.SetState(EnemyController.State.ATTACK);
        rangedAttack.TryFire(); 
    }
}
