using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Transform target;
    private PlayerHealthComponent playerHealth;

    public void Init(Transform target, PlayerHealthComponent playerHealth)
    {
        this.target = target;
        this.playerHealth = playerHealth;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}