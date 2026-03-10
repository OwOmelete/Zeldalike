using System;
using Unity.Mathematics;
using UnityEngine;

public class Playertargetingsystem : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private float detectRadius = 8f;
    private Transform currentTarget;

    private void Update()
    {
        if (active)
        {
            FindClosestEnemy();
        }
    }

    void FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius);

        Transform closestTarget = null;
        float bestDistance = math.INFINITY;
        
        foreach (Collider col in hits)
        {
            if (col.gameObject.tag == "Enemy")
            {
                Vector3 diff = col.transform.position - transform.position;
                float dist = diff.sqrMagnitude;
                
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closestTarget = col.transform;
                }
            }
            currentTarget = closestTarget;
            gameObject.transform.LookAt(currentTarget);
        }
    }
    
}
