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
    [SerializeField] List<ObstacleTD> targetFight = new List<ObstacleTD>();
    GameObject targetFightGameobject;
    [Header("Slider")]
    [SerializeField] Slider LifeBar;
    [SerializeField] Image image;
    Color32 colorbase;
    [SerializeField] Color32 colordammaged;


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
        StartCoroutine (dammageAnimation());
        
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
        }
    }
    void Die()
    {
        if (life <= 0)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            targetFight[0].isFighting = false;
        }
    }
    IEnumerator dammageAnimation()
    {
        image.color = colordammaged;
        yield return new WaitForSeconds(0.5f);
        image.color = colorbase;
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator EnnemyFight()
    {
        while (targetFight[0].life > 1)
        {
             targetFight[0].life -= attack;
             yield return new WaitForSeconds(CouldownAttack);
        }
        targetFightGameobject.SetActive(false);
        //Destroy(targetFightGameobject);
        asEnnemyTarget = false;
        isInFight=false;
        if (Patrole()!=null) StopCoroutine(Patrole());
        StartCoroutine(Patrole());
    }
    IEnumerator ObstacleFight()
    {
       while (life > 1 && targetFight[0].life > 1)
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
            else nextDestination++;
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
