using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class EnnemyRework :  MonoBehaviour, IDamagable
{
        [Header("StatsEnnemi")]
    [SerializeField] float speed;
    [SerializeField] float patrolSpeed;
    [SerializeField] float dammage;
    [SerializeField] float corruptLife;
    [SerializeField] float CurentHeat;
    [SerializeField] float attackSpeed;

        [Header("Target")]
    public List<Transform> patrol = new List<Transform>();
    [SerializeField] int nextDestination;
    [SerializeField] Transform player;

        [Header("Action")]
    public Action OnDeath;
    public Action OnRange;

       [Header("Contexte")]
    [SerializeField] bool fightStart;
    [SerializeField] bool IsAttacking;
    [SerializeField] bool IsPatrol;
    [SerializeField] Vector3 distanceToPlayer;


    [Header("Previsualization")]
    [SerializeField] GameObject zoneAttack;

    [SerializeField] EnnemyHeatSystem HeatSystem;
    private HashSet<int> receivedAttacks = new HashSet<int>();

    void Start()
    {
        fightStart = false;
    // Pour eviter de mettre le test de range dans l'update on va faire un scipt secondaire qui invoque les action avec un sphereCollider en trigger
        OnRange+=PrepareAttack;
        NewDestination();
    }
    void Update()
    {
        
        if (fightStart && !IsAttacking)
        {
            StartCoroutine(DistanceToPlayer());
            if(distanceToPlayer.magnitude> 4)
            {
                 Move(player);
            }
        }
        else if (!fightStart && !IsPatrol)
        {
            Vector3 distance = patrol[nextDestination].position - transform.position;
            if (Math.Abs(distance.magnitude) < 1)
            {
                NewDestination();
            }
            Move(patrol[nextDestination]);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            fightStart=true;
            GetComponent<SphereCollider>().enabled=false;
        }
    }
    void Move(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.position += dir*speed*Time.deltaTime;
    }
    void NewDestination()
    {
        System.Random rnd = new System.Random();
        nextDestination  = rnd.Next(0, patrol.Count);
    }
       void PrepareAttack()
    {
        Debug.Log("va attaquer");
        IsAttacking = true;
        StartCoroutine(Attack1());
    }
    void updateZonneAttack()
{
    Vector3 dir = (distanceToPlayer).normalized;

    float distance = 4f; 

    zoneAttack.transform.position = transform.position + dir * distance;
}
   IEnumerator DistanceToPlayer()
{
    distanceToPlayer = player.position - transform.position;
    yield return new WaitForSeconds(0.2f);
}
    IEnumerator Attack1()
{
    zoneAttack.SetActive(true); 
    updateZonneAttack();
    yield return new WaitForEndOfFrame();

    yield return new WaitForSeconds(attackSpeed*0.5f);

    IsAttacking = false;
    speed = 15;
    Move(player);
    

    yield return new WaitForSeconds(0.2f);
    IsAttacking = true;
    speed = 5;
    yield return new WaitForSeconds(attackSpeed*0.2f);
    var col = zoneAttack.GetComponent<Collider>();
    col.enabled = true;

    yield return new WaitForSeconds(0.1f);
    zoneAttack.GetComponent<DetectionAttack>().canAttack = true;
    col.enabled = false;
    IsAttacking = false;
    zoneAttack.SetActive(false);
}

    
    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (receivedAttacks.Contains(attackID)) return;
        receivedAttacks.Add(attackID);

        switch (data.currentTemperature)
        {
            case InvoDataInstance.temperature.cold:
                HeatSystem.reduceHeat(data.coldValue);
                break;
            case InvoDataInstance.temperature.hot:
                HeatSystem.increaseHeat(data.hotValue);
                break;
        }
    }
}
