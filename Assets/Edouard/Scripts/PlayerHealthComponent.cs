using System.Collections;
using UnityEngine;

public class PlayerHealthComponent : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private float regenRate = 5f;
    [SerializeField] private float regenDelay = 10f;

    [SerializeField] private Material iceScreenMaterial;

    private static readonly int OpacityID = Shader.PropertyToID("_Opacity");

    private Coroutine regenCoroutine;
    private Coroutine regenDelayCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;

        iceScreenMaterial = Instantiate(iceScreenMaterial);
    }

    private void Update()
    {
        if (iceScreenMaterial != null)
        {
            float normalized = currentHealth / maxHealth;

            float opacity = 1f - normalized;

            iceScreenMaterial.SetFloat(OpacityID, opacity);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0f);

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }

        if (regenDelayCoroutine != null)
        {
            StopCoroutine(regenDelayCoroutine);
        }

        regenDelayCoroutine = StartCoroutine(RegenDelay());
    }

    private IEnumerator RegenDelay()
    {
        yield return new WaitForSeconds(regenDelay);
        regenCoroutine = StartCoroutine(Regen());
    }

    private IEnumerator Regen()
    {
        while (currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            yield return null;
        }

        regenCoroutine = null;
    }
}