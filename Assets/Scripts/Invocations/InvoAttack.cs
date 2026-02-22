using System;
using System.Collections;
using UnityEngine;

public class InvoAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;


    private void OnEnable()
    {
        InvoManager.OnAttack += ATK;
    }

    private void ATK(InvoBehaviour[] invoList)
    {

        StartCoroutine(attacksDelay(invoList));
    }

    IEnumerator attacksDelay(InvoBehaviour[] invos)
    {
        for (int i = 0; i < invos.Length; i++)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(attack(invos[i]._InvoInstance));
        }
    }
    
    IEnumerator attack(InvoDataInstance invoInstance)
    {
        Vector3 targetAtk = player.transform.forward.normalized * 1;

        Vector3 baseOffset = invoInstance.offset;
        //invoInstance.currentState = InvoData.State.attack;
        invoInstance.rb.useGravity = false;
        invoInstance.acceleration = 15;
        invoInstance.target = player.transform;
        invoInstance.offset = player.transform.forward.normalized * 2;

        yield return new WaitForSeconds(3);

        
        invoInstance.acceleration = 40;
        invoInstance.maxSpeed = 100;
        invoInstance.offset = targetAtk;
        
        yield return new WaitForSeconds(3);
        
        //invoInstance.currentState = InvoData.State.idle;
        invoInstance.rb.useGravity = true;
        invoInstance.offset = baseOffset;
        invoInstance.target = player.transform;
        invoInstance.acceleration = 15;
        invoInstance.maxSpeed = 5;
    }
    
    
}
