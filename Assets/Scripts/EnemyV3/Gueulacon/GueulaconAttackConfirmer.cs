using UnityEngine;

public class NewGueulaconAttackConfirmer : MonoBehaviour
{
    private NewGueulaconAttack attack;

    private void Awake()
    {
        attack = GetComponentInParent<NewGueulaconAttack>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!attack.CanHit) return;

        if (!other.CompareTag("Player")) return;

        PlayerHealthComponent health =
            other.GetComponent<PlayerHealthComponent>();

        if (health == null) return;

        Debug.Log("Confirmed melee hit");

        health.TakeDamage(attack.GetDamage());

        attack.StopAttack();
    }
}