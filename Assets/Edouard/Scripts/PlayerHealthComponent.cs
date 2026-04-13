using System;
using System.Collections;
using UnityEngine;

public class PlayerHealthComponent : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    private bool isHealthRegenRunning;
    private bool isBeenHitRecently;

    private void Start()
    {
        currentHealth = maxHealth;
        isHealthRegenRunning = false;
        isBeenHitRecently = false;
    }

    private void Update()
    {
        if (isBeenHitRecently)
        {
            if (currentHealth < maxHealth)
            {
                isHealthRegenRunning = true;
                StartCoroutine(Regen());
            }
        }
        else
        {
            StopCoroutine(Regen());
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
    }
}
