using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    public EnemyStatsSO Stats;
    public Slider healthBar;
    public EnnemyHeatSystem HeatSystem;

    private float currentHealth;
    private HashSet<int> receivedAttacks = new HashSet<int>();

    void Start()
    {
        currentHealth = Stats.maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID)) return;
        receivedAttacks.Add(attackID);

        currentHealth -= damage;

        switch (data.currentTemperature)
        {
            case InvoDataInstance.temperature.veryCold:
                HeatSystem.reduceHeat(data.veryColdValue);
                break;
            case InvoDataInstance.temperature.cold:
                HeatSystem.reduceHeat(data.coldValue);
                break;
            case InvoDataInstance.temperature.hot:
                HeatSystem.increaseHeat(data.hotValue);
                break;
            case InvoDataInstance.temperature.veryHot:
                HeatSystem.increaseHeat(data.veryHotValue);
                break;
        }

        if (currentHealth <= 0)
        {
            GetComponent<EnemyController>().SetState(EnemyController.State.DEATH);
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.value = currentHealth / Stats.maxHealth;
    }
}
