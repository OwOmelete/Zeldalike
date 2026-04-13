using System;
using System.Collections;
using UnityEngine;

public class PlayerHealthComponent : MonoBehaviour
{
    private static float currentHealth;
    private float maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth != maxHealth)
        {
            StartCoroutine(RegenHealth());
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public static void PlayerTakeDamage(float amount)
    {
        currentHealth -= amount;
        
    }
    
    private IEnumerator RegenHealth()
    {
        yield return new WaitForSeconds(10);
        
        while (currentHealth != maxHealth)
        {
            currentHealth++;
        }
    }
}
