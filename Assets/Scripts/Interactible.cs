using System;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

public class Interactible : MonoBehaviour, IDamagable
{
    public static event Action<int> OnBreak;
    
    [SerializeField] private int hitToBreak;

    [SerializeField] private GameObject[] objectsToEnable;

    private int currentHits;
    

    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (data.currentState is StateAttack)
        {
            currentHits += 1;
            if (currentHits >= hitToBreak)
            {
                Break();
            }
        }
    }
    
    private void Break()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        OnBreak?.Invoke(0);
        gameObject.SetActive(false);
    }
}
