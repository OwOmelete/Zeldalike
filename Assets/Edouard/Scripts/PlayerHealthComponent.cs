using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    private bool isHealthRegenRunning;

    private Coroutine regenCoroutine;
    private Coroutine regenDelayCoroutine;
    
    [SerializeField] private Material IceScreenShader;   

    private void Start()
    {
        currentHealth = maxHealth;
        isHealthRegenRunning = false;
    }

    private void Update()
    {
        var color = IceScreenShader.color;
        color.a = currentHealth / maxHealth;
        IceScreenShader.color = color;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0f)
            currentHealth = 0f;

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
            isHealthRegenRunning = false;
        }

        if (regenDelayCoroutine != null)
        {
            StopCoroutine(regenDelayCoroutine);
        }

        regenDelayCoroutine = StartCoroutine(RegenDelay());
    }

    private IEnumerator RegenDelay()
    {
        yield return new WaitForSeconds(10f);

        if (currentHealth < maxHealth && !isHealthRegenRunning)
        {
            regenCoroutine = StartCoroutine(Regen());
            isHealthRegenRunning = true;
        }
    }

    private IEnumerator Regen()
    {
        while (currentHealth < maxHealth)
        {
            currentHealth++;
            yield return new WaitForSeconds(0.3f);
        }

        isHealthRegenRunning = false;
        regenCoroutine = null;
    }
}
