using UnityEngine;
using System.Collections;

public class NewGueulaconAttack : MonoBehaviour, IAttack
{
    [Header("Attack Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("References")]
    [SerializeField] private MeshRenderer attackMesh;

    private Coroutine attackRoutine;

    public bool CanHit { get; private set; }

    public void TryAttack()
    {
        if (attackRoutine != null) return;

        attackRoutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            attackMesh.enabled = true;

            yield return new WaitForSeconds(attackCooldown);

            CanHit = true;

            Debug.Log("Hit window active");

            attackMesh.enabled = false;

            yield return new WaitForSeconds(0.2f);

            CanHit = false;

            Debug.Log("Hit window ended");

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        CanHit = false;

        if (attackMesh != null)
        {
            attackMesh.enabled = false;
        }
    }

    public float GetDamage()
    {
        return damage;
    }
}