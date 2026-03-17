using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private HashSet<int> receivedAttacks = new HashSet<int>();
    public Image cible;

    [SerializeField]
    private int MaxHp;
    
    [SerializeField]
    private Slider HpBar;

    private float Hp;

    private void Start()
    {
        Hp = MaxHp;
        UpdateHpBar();
    }

    public void TakeDamage(float damage, int attackID)
    {
        if (receivedAttacks.Contains(attackID))
            return;

        receivedAttacks.Add(attackID);

        Hp -= damage;

        if (Hp <= 0)
        {
            Die();
        }
        else
        {
            UpdateHpBar();
        }
        
        Debug.Log("Enemy took damage: " + damage);
    }

    void UpdateHpBar()
    {
        HpBar.value = Hp / MaxHp;
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}