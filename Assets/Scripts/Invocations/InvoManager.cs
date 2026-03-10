using System;
using System.Collections.Generic;
using UnityEngine;

public class InvoManager : MonoBehaviour
{
    public List<InvoBehaviour> InvoList = new List<InvoBehaviour>();
    public InvoAttack InvoAttack;
    
    
    
    public static event Action<InvoBehaviour[]> OnAttack;
    

    private void OnEnable()
    {
        InvoBehaviour.OnInvoSpawn += AddInvocation;
    }

    private void OnDisable()
    {
        InvoBehaviour.OnInvoSpawn -= AddInvocation;
    }

    private void AddInvocation(InvoBehaviour instance)
    {
        InvoList.Add(instance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnAttack?.Invoke(GetClosest(10));
        }
    }

    private InvoBehaviour[] GetClosest(int n)
    {
        InvoBehaviour[] result = new InvoBehaviour[n];
        
        int found = 0;
        
        float[] distances = new float[n];

        for (int i = 0; i < InvoList.Count; i++)
        {
            GameObject invo = InvoList[i].gameObject;

            float dist = (invo.transform.position - transform.position).sqrMagnitude;

            if (found < n)
            {
                result[found] = InvoList[i];
                distances[found] = dist;
                found++;
            }
            else
            {
                int maxIndex = 0;
                float maxDist = distances[0];

                for (int j = 1; j < found; j++)
                {
                    if (distances[j] > maxDist)
                    {
                        maxDist = distances[j];
                        maxIndex = j;
                    }
                }

                if (dist < maxDist)
                {
                    result[maxIndex] = InvoList[i];
                    distances[maxIndex] = dist;
                }
            }
        }
        return result;
    }
    
    
    
}
