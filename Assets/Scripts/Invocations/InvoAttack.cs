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
        setAttack(invoList);
        //StartCoroutine(attacksDelay(invoList));
    }


    private void setAttack(InvoBehaviour[] invos)
    {
        float rowIndex = 0;
        float currentColIndex = 0;
        for (int i = 0; i < invos.Length; i++)
        {
            invos[i]._InvoInstance.offset =
                invos[i].player.forward * 5 + Vector3.up * 2 +
                invos[i].player.right * (currentColIndex - (rowIndex + 1) / 2) - invos[i].player.forward * rowIndex;
            invos[i].ChangeState(invos[i].stateAttack);
            invos[i]._InvoInstance.direction = invos[i].player.forward;
            if (currentColIndex >= rowIndex)
            {
                rowIndex++;
                currentColIndex = 0;
            }
            else
            {
                currentColIndex++;
            }
        }
    }
    

    /*IEnumerator attacksDelay(InvoBehaviour[] invos)
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
    }*/
    
    
}
