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

        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float distance = toTargetXZ.magnitude;

        float speed = launchForce;

        if (distance < 0.1f)
        {
            velocity = transform.forward * speed;
            velocity.y = speed * 0.5f;
            return;
        }

        float time = distance / speed;

        if (time <= 0.01f)
        {
            velocity = transform.forward * speed;
            velocity.y = speed * 0.5f;
            return;
        }

        float yVelocity = (toTarget.y + 0.5f * gravityAbs * time * time) / time;

        Vector3 velocityXZ = toTargetXZ.normalized * speed;

        velocity = velocityXZ;
        velocity.y = yVelocity;
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