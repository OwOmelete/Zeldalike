using System;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private int necessaryAmount;
    [SerializeField] private int currentAmount;
    
    void CheckAmount()
    {
        if (currentAmount >= necessaryAmount)
        {
            Debug.Log("Necessary amount achieved");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Automates")
        {currentAmount++;}
        CheckAmount();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Automates")
        {currentAmount--;}
    }
}
