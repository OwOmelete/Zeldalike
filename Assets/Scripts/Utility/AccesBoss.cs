using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AccesBoss : MonoBehaviour
{
    public List<SimonManager> simon = new List<SimonManager>();
    public GameObject ice;
    int count;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(("Player")))
        {
            count = 0;
            foreach (SimonManager sim in simon)
            {
                if (sim.hasWon) count++;
            }
            if(count==simon.Count)ice.SetActive((false));
        }
    }
}
