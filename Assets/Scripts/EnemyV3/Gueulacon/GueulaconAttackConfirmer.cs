using UnityEngine;

public class GueulaconAttackConfirmer : MonoBehaviour
{
    private EnemyKnockBackAttack attack;

    private void Awake()
    {
        attack = GetComponentInParent<EnemyKnockBackAttack>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!attack.canHit) return;

        if (!other.CompareTag("Player")) return;

        PlayerHealthComponent health =
            other.GetComponent<PlayerHealthComponent>();

        if (health != null)
        {
            Debug.Log("Confirmed melee hit");

            health.TakeDamage(attack.stats.damage);

            attack.canHit = false;
        }
    }
}