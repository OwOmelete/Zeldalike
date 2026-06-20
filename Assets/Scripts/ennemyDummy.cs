using System;
using System.Collections.Generic;
using UnityEngine;

public class ennemyDummy : MonoBehaviour, IDamagable
{
    [SerializeField] private EnnemyHeatSystem HeatSystem;
    [SerializeField] private Animation anim;
    
    private HashSet<int> receivedAttacks = new HashSet<int>();


    private void OnEnable()
    {
        HeatSystem.weakpointBroke += Hit;
    }

    private void OnDisable()
    {
        HeatSystem.weakpointBroke -= Hit;
    }

    private void Update()
    {
        if(!HeatSystem.isAlive) Destroy(gameObject);
    }

    private void Hit()
    {
        if(anim==null) return;
        anim.Play();
    }
    
    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID)) return;
        receivedAttacks.Add(attackID);


        if (data.currentState is StateAttack)
        {
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
}
