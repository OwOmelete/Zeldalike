using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvoManager : MonoBehaviour
{
    public List<InvoBehaviour> InvoList = new List<InvoBehaviour>();
    public InvoAttack InvoAttack;
    [SerializeField] private LayerMask layerMask;
    private int currentAttackID = 0;
    private int currentDeactivated = 0;

    private Enemy lastEnemyLocked;
    
    
    
    public static event Action<List<InvoBehaviour>> OnAttackAction;
    public static event Action<List<InvoBehaviour>> OnProtection;
    
    public static event Action<Transform> OnLock;
    public static event Action<Transform> OnDelock;

    public static event Action FireWaveAction;
    
    

    private void OnEnable()
    {
        InvoBehaviour.OnInvoSpawn += AddInvocation;
        InvoBehaviour.OnInvoActivate += removeInvo;
    }

    private void OnDisable()
    {
        InvoBehaviour.OnInvoSpawn -= AddInvocation;
        InvoBehaviour.OnInvoActivate -= removeInvo;
    }

    private void AddInvocation(InvoBehaviour instance)
    {
        InvoList.Add(instance);
    }

    private void removeInvo(bool b)
    {
        if (!b)
        {
            currentDeactivated++;
        }
        else
        {
            currentDeactivated--;
        }
    }
    
    private void OnShield(InputValue value)
    {
        List<InvoBehaviour> l = new();
        foreach (var invo in InvoList)
        {
            if (invo._InvoInstance.isActivated)
            {
                l.Add(invo);
            }
        }
        OnProtection?.Invoke(l);
    }

    private void OnAttack()
    {
        Debug.Log(currentDeactivated);
        List<InvoBehaviour> invos = GetClosest((int)((InvoList.Count-currentDeactivated)/2));

        int attackID = GetNewAttackID();

        foreach (var invo in invos)
        {
            if (invo != null)
                invo.SetAttackID(attackID);
        }

        OnAttackAction?.Invoke(invos);
    }

    private void OnLockEnemy()
    {
        Enemy enemy = CharacterTargeting.FindClosestEnemy(transform.position, 10, layerMask);

        if (!enemy)
        {
                
        }
        else if (enemy == lastEnemyLocked)
        {
            enemy.cible.enabled = false;
            lastEnemyLocked = null;
            OnDelock?.Invoke(enemy.transform);
        }
        else if (enemy != null)
        {
            if(lastEnemyLocked != null) lastEnemyLocked.cible.enabled = false;
                
            enemy.cible.enabled = true;
            lastEnemyLocked = enemy;
            OnLock?.Invoke(enemy.transform);
        }
    }

    private void OnFireWave()
    {
        FireWaveAction?.Invoke();
    }
    
    public int GetNewAttackID()
    {
        currentAttackID++;
        return currentAttackID;
    }
    
    
    
    

    private List<InvoBehaviour> GetClosest(int n)
    {
        List<InvoBehaviour> result = new();
        List<float> distances = new();

        for (int i = 0; i < InvoList.Count; i++)
        {
            if (!InvoList[i]._InvoInstance.isActivated)
                continue;

            float dist = (InvoList[i].transform.position - transform.position).sqrMagnitude;

            if (result.Count < n)
            {
                result.Add(InvoList[i]);
                distances.Add(dist);
            }
            else
            {
                int maxIndex = 0;
                float maxDist = distances[0];

                for (int j = 1; j < distances.Count; j++)
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
