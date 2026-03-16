using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private HashSet<int> receivedAttacks = new HashSet<int>();

    public void TakeDamage(float damage, int attackID)
    {
        if (receivedAttacks.Contains(attackID))
            return;

        receivedAttacks.Add(attackID);
        
        Debug.Log("Enemy took damage: " + damage);
    }
}