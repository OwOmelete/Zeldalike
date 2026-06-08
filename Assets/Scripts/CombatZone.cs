using System;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    public Door[] doors;
    private bool activated = false;
    public EnnemyRework ennemyRework;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!activated)
            {
                activated = true;
                foreach (var door in doors)
                {
                    door.Close();
                }
            }
            ennemyRework.StartFight(other);
            Debug.Log("PlayerDetected");
        }
    }
}
