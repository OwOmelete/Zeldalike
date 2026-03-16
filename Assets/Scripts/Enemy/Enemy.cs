using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private HashSet<int> receivedAttacks = new HashSet<int>();
    public Image cible;

    public void TakeDamage(float damage, int attackID)
    {
        if (receivedAttacks.Contains(attackID))
            return;

        receivedAttacks.Add(attackID);
        
        Debug.Log("Enemy took damage: " + damage);
    }
}