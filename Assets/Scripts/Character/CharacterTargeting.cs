using System;
using Unity.Mathematics;
using UnityEngine;

public class CharacterTargeting
{
    public static Enemy FindClosestEnemy(Vector3 pos, int radius, int layerMask)
    {
        Collider[] hits = Physics.OverlapSphere(pos, radius, layerMask);

        if (hits.Length == 0)
        {
            return null;
        }

        Enemy closestTarget = null;
        GameObject go = null;
        float bestDistance = math.INFINITY;

        foreach (Collider col in hits)
        {
            Vector3 diff = col.transform.position - pos;
            float dist = diff.sqrMagnitude;

            if (dist < bestDistance)
            {
                bestDistance = dist;
                go = col.gameObject;
            }
        }

        closestTarget = go.GetComponent<Enemy>();
        
        return closestTarget;
    }
}