using System;
using UnityEngine;

public class DetectorLogicV1 : MonoBehaviour
{
    [SerializeField] private BasicEnemyV1 basicEnemyV1ScriptReference;
    [SerializeField] private float attackRadius;
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
            basicEnemyV1ScriptReference.HandleEnemyState(BasicEnemyV1.StateFlags.IDLE);
        }
    }

    private void BehaviourHandler()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);
        
        if (distanceFromPlayer > attackRadius)
        {
            basicEnemyV1ScriptReference.HandleEnemyState(BasicEnemyV1.StateFlags.CHASING);
        }
        else
        {
            basicEnemyV1ScriptReference.HandleEnemyState(BasicEnemyV1.StateFlags.ATTACKING);
        }
    }
}
