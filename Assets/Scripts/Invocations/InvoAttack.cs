using System;
using System.Collections;
using UnityEngine;

public class InvoAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public InvoDataInstance invo;


    private void OnEnable()
    {
        InvoManager.OnAttack += ATK;
    }

    private void ATK(InvoDataInstance invoInstance)
    {
        invo = invoInstance;
        StartCoroutine(attack());
    }

    IEnumerator attack()
    {
        Vector3 targetAtk = player.transform.forward.normalized * 10;

        invo.currentState = InvoData.State.attack;
        invo.rb.useGravity = false;
        invo.acceleration = 15;
        invo.target = player.transform;
        invo.offset = player.transform.forward.normalized * 2;

        yield return new WaitForSeconds(5);

        
        invo.acceleration = 40;
        invo.maxSpeed = 100;
        invo.offset = targetAtk;
        
        yield return new WaitForSeconds(2);
        
        invo.currentState = InvoData.State.idle;
        invo.rb.useGravity = true;
        invo.target = player.transform;
        invo.acceleration = 15;
        invo.maxSpeed = 5;
    }
    
    
}
