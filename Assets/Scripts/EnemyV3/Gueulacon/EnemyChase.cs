using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float stopDistance = 2f;

    private Transform player;

    private bool isChasing;

    private NewEnemyController controller;

    private void Awake()
    {
        controller = GetComponent<NewEnemyController>();
    }

    public void StartChase(Transform target)
    {
        player = target;

        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;

        player = null;
    }

    private void Update()
    {
        if (!isChasing) return;

        if (player == null) return;

        Vector3 direction = player.position - transform.position;

        direction.y = 0;

        float distance = direction.magnitude;

        if (distance <= stopDistance)
        {
            controller.SetState(NewEnemyController.States.ATTACKING);

            return;
        }

        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}