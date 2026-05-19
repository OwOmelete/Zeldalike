using System;
using System.Collections.Generic;
using UnityEngine;

public class SimonManager : MonoBehaviour, IDamagable
{

    [SerializeField] private SimonDisplay _simonDisplay;
    
    private HashSet<int> receivedAttacks = new HashSet<int>();


    private int currentIndex;
    
    public SimonDisplay.color[] sequence;

    public bool isTrying;
    public bool hasWon;
    
    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID))
            return;

        receivedAttacks.Add(attackID);

        if (data.currentTemperature == InvoDataInstance.temperature.hot)
        {
            hot();
        }

        if (data.currentTemperature == InvoDataInstance.temperature.cold)
        {
            cold();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            hot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            cold();
        }
    }


    private void hot()
    {
        if (sequence[currentIndex] == SimonDisplay.color.red)
        {
            currentIndex += 1;
            isTrying = true;
            if (currentIndex >= sequence.Length)
            {
                Bravo();
            }
        }
        else
        {
            currentIndex = 0;
            isTrying = false;
            Dommage();
        }
    }

    private void cold()
    {
        if (sequence[currentIndex] == SimonDisplay.color.blue)
        {
            currentIndex += 1;
            isTrying = true;
            if (currentIndex >= sequence.Length)
            {
                Bravo();
            }
        }
        else
        {
            currentIndex = 0;
            isTrying = false;
            Dommage();
        }
    }

    private void Bravo()
    {
        hasWon = true;
        Debug.Log("bravo");
    }

    private void Dommage()
    {
        isTrying = false;
        _simonDisplay.ResetSequence();
    }
    
    
}
