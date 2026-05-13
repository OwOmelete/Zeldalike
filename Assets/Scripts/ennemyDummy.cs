using System;
using System.Collections.Generic;
using UnityEngine;

public class ennemyDummy : MonoBehaviour, IDamagable
{
    [SerializeField] private EnnemyHeatSystem HeatSystem;
    
    private HashSet<int> receivedAttacks = new HashSet<int>();


    private void Update()
    {
        if(HeatSystem.isAlive) Destroy(gameObject);
    }

    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID)) return;
        receivedAttacks.Add(attackID);
        

        switch (data.currentTemperature)
        {
            case InvoDataInstance.temperature.cold:
                HeatSystem.reduceHeat(data.coldValue);
                break;
            case InvoDataInstance.temperature.hot:
                HeatSystem.increaseHeat(data.hotValue);
                break;
        }
    }
}
