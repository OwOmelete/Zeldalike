using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float launchForce = 10f;
    public float gravity = -20f;

    private Vector3 velocity;
    private int damage;

    private float destructionTime = 5f;

    public void Init(Vector3 targetPosition, int dmg)
    {
        damage = dmg;

        Vector3 start = transform.position;
        Vector3 toTarget = targetPosition - start;

        float gravityAbs = Mathf.Abs(gravity);

        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float distance = toTargetXZ.magnitude;

        float arcHeight = 2f;

        float timeUp = Mathf.Sqrt(2 * arcHeight / gravityAbs);

        float timeDown = Mathf.Sqrt(2 * Mathf.Max(0.01f, arcHeight - toTarget.y) / gravityAbs);

        float totalTime = timeUp + timeDown;

        Vector3 velocityXZ = toTargetXZ / totalTime;

        float velocityY = gravityAbs * timeUp;

        velocity = velocityXZ;
        velocity.y = velocityY;
    }

    void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var health = collision.gameObject.GetComponent<PlayerHealthComponent>();

        if (health != null)
            health.TakeDamage(damage);

        Destroy(gameObject);
    }
}