using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float regen;
    [SerializeField] private float maxShaderOpacity;
    [SerializeField] private Material _material;
    public static event Action OnDeath; 
    
    private float currentHealth;

    private void FixedUpdate()
    {
        if (currentHealth < maxHealth)
        {
            if (currentHealth + regen > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += regen;
            }
        }
    }

    private void Update()
    {
        updateShader();
    }


    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
    
    void updateShader()
    {
        _material.SetFloat("Opacity", (maxHealth / currentHealth) * maxShaderOpacity);
    }
    
    
    
}
