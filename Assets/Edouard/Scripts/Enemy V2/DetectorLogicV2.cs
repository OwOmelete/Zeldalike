using System;
using UnityEngine;

public class DetectorLogicV2 : MonoBehaviour
{
    [SerializeField] private BasicEnemy BasicEnemyScriptReference;
    [SerializeField] private int attackRadius;
    private Transform playerTransform;
    private bool isPlayerInRange;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && playerTransform != null)
        {
            BehaviourHandler();
        }
        else
        {
            BasicEnemyScriptReference.HandleEnemyState(BasicEnemy.StateFlags.IDLE);
        }
    }

    private void BehaviourHandler()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);
        
        if (distanceFromPlayer > attackRadius)
        {
            BasicEnemyScriptReference.HandleEnemyState(BasicEnemy.StateFlags.CHASING);
        }
        else
        {
            BasicEnemyScriptReference.HandleEnemyState(BasicEnemy.StateFlags.ATTACKING);
        }
    }
}
