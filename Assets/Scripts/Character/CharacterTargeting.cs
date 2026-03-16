using System;
using Unity.Mathematics;
using UnityEngine;

public class CharacterTargeting
{
    public static Transform FindClosestEnemy(Vector3 pos, int radius, int layerMask)
    {
        Collider[] hits = Physics.OverlapSphere(pos, radius, layerMask);

        Transform closestTarget = null;
        float bestDistance = math.INFINITY;

        foreach (Collider col in hits)
        {
            Vector3 diff = col.transform.position - pos;
            float dist = diff.sqrMagnitude;

            if (dist < bestDistance)
            {
                bestDistance = dist;
                closestTarget = col.transform;
            }
        }

        return closestTarget;
    }
}