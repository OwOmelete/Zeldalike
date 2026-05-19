using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float regen;
    [SerializeField] private float maxShaderOpacity;
    [SerializeField] private Material _material;
    public static event Action OnDeath; 
    
    public float currentHealth;


    private void Awake()
    {
        _material.SetFloat("_Opacity", 0);
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

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

        if (currentHealth > maxHealth) currentHealth = maxHealth;
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
        _material.SetFloat("_Opacity", (1 - currentHealth / maxHealth) * maxShaderOpacity);
    }
    
    
    
}
