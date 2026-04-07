using System;
using UnityEngine;

public class DetectorLogicV2 : MonoBehaviour
{
    [SerializeField] private BasicEnemyV2 basicEnemyV2ScriptReference;
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
            basicEnemyV2ScriptReference.HandleEnemyState(BasicEnemyV2.StateFlags.IDLE);
        }
    }

    private void BehaviourHandler()
    {
        float distanceFromPlayer = Vector3.Distance(
            basicEnemyV2ScriptReference.transform.position,
            playerTransform.position
        );

        if (basicEnemyV2ScriptReference.isAttacking) return;

        if (distanceFromPlayer > attackRadius)
            basicEnemyV2ScriptReference.HandleEnemyState(BasicEnemyV2.StateFlags.CHASING);
        else
            basicEnemyV2ScriptReference.HandleEnemyState(BasicEnemyV2.StateFlags.ATTACKING);
    }
}
