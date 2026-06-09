using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using System;
using Unity.Entities.UniversalDelegates;
using JetBrains.Annotations;

public class EnnemyTD : MonoBehaviour
{
     [Header("StatsEnnemi")]
     [SerializeField] float speed;
     [SerializeField] public float life;
     [SerializeField] float attack;
     [SerializeField] int dammage;
     [SerializeField] float CouldownAttack;

     [Header("Patrole")]
    [SerializeField] List<Transform> patrole = new List<Transform>();
    
    [Header("Contexte")]
    [SerializeField] bool asEnnemyTarget;
    [SerializeField] bool isInFight;
    [SerializeField] int nextDestination;
    [SerializeField] bool isSlow;
    bool isBurn;
    [SerializeField] List<ObstacleTD> targetFight = new List<ObstacleTD>();
    GameObject targetFightGameobject;
    [Header("Slider")]
    [SerializeField] Slider LifeBar;
    [SerializeField] Image image;
    Color32 colorbase;
    [SerializeField] Color32 colordammaged;
    [SerializeField] Color32 colorSlow;
    [SerializeField] Color32 colorBurn;
    [Header("Reference")]
    [SerializeField] GameManagerTD gameManagerTD;
    [SerializeField] GameObject underBridge;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isInFight=false;
        nextDestination=0;
        colorbase=image.color;
        if (Patrole()!=null) StopCoroutine(Patrole());
        StartCoroutine(Patrole());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float DistanceToTarget(Transform Target)
    {
        float distanceToTarget = (Target.position - transform.position).magnitude;
        return distanceToTarget;
    }
    void Move(Transform Target)
    {
        Vector3 dir = Target.position - transform.position;
        transform.position += dir.normalized*speed*Time.deltaTime;
    }
    public void takeDammage(float dammage)
    {
        life-=dammage;
        if (dammageAnimation()!=null) StopCoroutine(dammageAnimation());
        if (gameObject.activeSelf) StartCoroutine (dammageAnimation());
        
        Die();
    }
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            int i = targetFight.Count;
            targetFight.Add(other.GetComponent<ObstacleTD>());
            if (!targetFight[i].isFighting && !asEnnemyTarget)
            { 
                Debug.Log("isFighting");
            targetFightGameobject = other.gameObject;
            targetFight[i].isFighting = true;
            asEnnemyTarget = true;
            isInFight=true;
            if (BeforeFight(other)!=null) StopCoroutine(BeforeFight(other));
            StartCoroutine(BeforeFight(other));
            }
            else
            {
                targetFight.Remove(targetFight[i]);
            }
        }
    }
    void Die()
    {
        if (life <= 0)
        {
            gameManagerTD.UpdateEnergie(true,100); 
            gameObject.SetActive(false);
            if(targetFight.Count>0) targetFight[0].isFighting = false;
        }
    }
    public void Burn(float burn)
    {
         if (BurnCoroutine(0)!=null) StopCoroutine(BurnCoroutine(0));
        if(gameObject.activeSelf)StartCoroutine(BurnCoroutine(burn));
    }
     IEnumerator BurnCoroutine(float burn)
    {
        isBurn=true;
        image.color = colorBurn;
        float burnDuration = 2f;
        while (burnDuration > 0)
        {
            burnDuration-=0.5f;
            takeDammage(burn);
            yield return new WaitForSeconds(05f);
        }
        image.color = colorBurn;
        isBurn=false;
    }
    public void Slow(float slow)
    {
        if (SlowCoroutine(0)!=null) StopCoroutine(SlowCoroutine(0));
        if(gameObject.activeSelf)StartCoroutine(SlowCoroutine(slow));
    }
     IEnumerator SlowCoroutine(float slow)
    {
        isSlow=true;
        speed-=slow;
        image.color = colorSlow;
        yield return new WaitForSeconds(2f);
        image.color = colorbase;
        speed+=slow;
        isSlow=false;
    }
    IEnumerator dammageAnimation()
    {
        image.color = colordammaged;
        yield return new WaitForSeconds(0.5f);
        if(!isSlow) image.color = colorbase;
        else image.color = colorSlow;
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator EnnemyFight()
    {
        while(targetFight[0].gameObject.activeSelf)
        {
            targetFight[0].TakeDamage(dammage);
            yield return new WaitForSeconds(CouldownAttack);
        }
        targetFight.Remove(targetFight[0]);
        asEnnemyTarget = false;
        isInFight=false;
        if (Patrole()!=null) StopCoroutine(Patrole());
        StartCoroutine(Patrole());
        yield return null;
    }
    IEnumerator ObstacleFight()
    {
       while (life > 1 && targetFight.Count>0 && targetFight[0].life > 1)
        {
            takeDammage(targetFight[0].attack);
            yield return new WaitForSeconds(targetFight[0].CouldownAttack);
        }
        Die();
        isInFight=false;
    }
   
    IEnumerator Patrole()
    {
        while (!isInFight)
        {
            if (DistanceToTarget(patrole[nextDestination]) > 2f)  Move(patrole[nextDestination]);
            else if (nextDestination<patrole.Count-1) nextDestination++;
            else 
            {
                gameManagerTD.UpdateLife(1);
                nextDestination=0;
                gameObject.SetActive(false);
            }

            if(nextDestination==4)transform.SetParent(underBridge.transform);
            yield return null;  
        }
        
    }
     IEnumerator BeforeFight(Collider other)
    {
        while (DistanceToTarget(other.transform) > 20f)
        {
            Move(other.transform);               
            yield return null;  
        }
        StartCoroutine(EnnemyFight());
        StartCoroutine(ObstacleFight());
        
    }
    
}
