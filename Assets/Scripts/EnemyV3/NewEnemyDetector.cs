using UnityEngine;

public class NewEnemyDetector : MonoBehaviour
{
    [SerializeField] private NewEnemyController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        controller.SetPlayer(other.transform);

        if (controller.TryGetComponent(out EnemyChase chase))
        {
            controller.SetState(NewEnemyController.States.CHASING);
        }
        else
        {
            controller.SetState(NewEnemyController.States.ATTACKING);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        controller.SetPlayer(null);

        if (controller.TryGetComponent(out NewGueulaconAttack attack))
        {
            attack.StopAttack();
        }

        controller.SetState(NewEnemyController.States.IDLE);
    }
}