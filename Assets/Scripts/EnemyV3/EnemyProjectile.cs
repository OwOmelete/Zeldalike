using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float launchForce = 10f;
    public float gravity = -20f;
    

    private Vector3 velocity;
    private int damage;

    public void Init(Vector3 targetPosition, int dmg)
    {
        damage = dmg;

        Vector3 start = transform.position;
        Vector3 direction = targetPosition - start;

        Vector3 horizontal = new Vector3(direction.x, 0, direction.z);

        velocity = horizontal.normalized * launchForce;

        velocity.y = launchForce * 0.7f;
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponent<PlayerHealthComponent>();

        if (health != null)
            health.TakeDamage(damage);

        Destroy(gameObject);
    }
}