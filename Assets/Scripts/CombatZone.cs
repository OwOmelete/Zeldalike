using System;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    [SerializeField] private Door[] doors;
    private bool activated = false;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            activated = true;
            foreach (var door in doors)
            {
                door.Close();
            }
        }

    }
}
