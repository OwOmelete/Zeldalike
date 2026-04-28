using System;
using System.Collections;
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

    [SerializeField]private Target target;
    
    private List<Transform> enemiesInRange = new List<Transform>();
    
    private Transform lastEnemyLocked;

    public MeshRenderer wave;
    
    
    
    public static event Action<List<InvoBehaviour>> OnAttackAction;
    public static event Action<List<InvoBehaviour>> OnProtection;
    
    public static event Action<Transform> OnLock;
    public static event Action OnDelock;

    public static event Action FireWaveAction;

    private void OnEnable()
    {
        InvoBehaviour.OnInvoSpawn += AddInvocation;
        InvoBehaviour.OnInvoActivate += removeInvo;
        DeathZone.OnFall += Fall;
        ButtonChainManager.OnPattern += HandlePattern;
    }

    private void OnDisable()
    {
        InvoBehaviour.OnInvoSpawn -= AddInvocation;
        InvoBehaviour.OnInvoActivate -= removeInvo;
        DeathZone.OnFall -= Fall;
        ButtonChainManager.OnPattern -= HandlePattern;
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
    
    private void Fall(Transform t)
    {
        transform.position = t.position;
        foreach (var invo in InvoList)
        {
            invo.resetPosition();
        }
    }
    
    /*private void OnShield(InputValue value)
    {
        List<InvoBehaviour> l = new();
        foreach (var invo in InvoList)
        {
            if (invo.Data.isActivated)
            {
                l.Add(invo);
            }
        }
        OnProtection?.Invoke(l);
    }*/

    private void HandlePattern(string name)
    {
        if (name == "lance")
        {
            LancePattern();
        }
    }
    
    private void LancePattern()
    {
        List<InvoBehaviour> invos = GetClosest((int)((InvoList.Count-currentDeactivated)/2));

        int attackID = GetNewAttackID();

        foreach (var invo in invos)
        {
            if (invo != null)
                invo.SetAttackID(attackID);
        }

        OnAttackAction?.Invoke(invos);
    }

    /*private void OnLockEnemy()
    {
        Enemy enemy = CharacterTargeting.FindClosestEnemy(transform.position, 30, layerMask);

        if (!enemy)
        {
                
        }
        else if (enemy == lastEnemyLocked)
        {
            enemy.cible.enabled = false;
            lastEnemyLocked = null;
            OnDelock?.Invoke();
        }
        else if (enemy != null)
        {
            if(lastEnemyLocked != null) lastEnemyLocked.cible.enabled = false;
                
            enemy.cible.enabled = true;
            lastEnemyLocked = enemy;
            OnLock?.Invoke(enemy.transform);
        }
    }
    */
    
    private void OnFireWave()
    {
        StartCoroutine(waveTimer());
        FireWaveAction?.Invoke();
    }

    IEnumerator waveTimer()
    {
        wave.enabled = true;
        yield return new WaitForSeconds(0.4f);
        wave.enabled = false;
    }
    
    public int GetNewAttackID()
    {
        currentAttackID++;
        return currentAttackID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyZone"))
        {
            Transform enemy = other.gameObject.transform;

            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
                
            }
            
            UpdateTarget();
        }
    }

    
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("EnemyZone")) return;

        Transform enemy = other.gameObject.transform;

        
        
        enemiesInRange.Remove(enemy);

        UpdateTarget();
    }
    
    private void UpdateTarget()
    {
        if (enemiesInRange.Count > 0)
        {
            TargetLock(enemiesInRange[0]);
        }
        else
        {
            target.sprite.enabled = false;
            lastEnemyLocked = null;
            OnDelock?.Invoke();
        
        }
    }

    private void TargetLock(Transform enemy)
    {
        target.target = enemy;
        target.sprite.enabled = true;
        lastEnemyLocked = enemy;
        OnLock?.Invoke(enemy.transform);
    }
    
    private List<InvoBehaviour> GetClosest(int n)
    {
        List<InvoBehaviour> result = new();
        List<float> distances = new();

        for (int i = 0; i < InvoList.Count; i++)
        {
            if (!InvoList[i].Data.isActivated)
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
