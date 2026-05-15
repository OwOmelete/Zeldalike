using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class EnnemyRework :  MonoBehaviour, IDamagable
{
        [Header("StatsEnnemi")]
    [SerializeField] public float speed;
    [SerializeField] float patrolSpeed;
    [SerializeField] float dammage;
    [SerializeField] float corruptLife;
    [SerializeField] float CurentHeat;
    [SerializeField] public float attackSpeed;

        [Header("Target")]
    public List<Transform> patrol = new List<Transform>();
    [SerializeField] public int nextDestination;
    [SerializeField] public Transform player;

        [Header("Action")]
    public Action OnDeath;
    public Action OnRange;

       [Header("Contexte")]
    [SerializeField] public bool fightStart;
    [SerializeField] public bool IsAttacking;
    [SerializeField] public bool IsPatrol;
    [SerializeField] Vector3 distanceToPlayer;
    public float distanceToPlayerActual;


    [Header("Previsualization")]
    [SerializeField] public GameObject zoneAttack;

    [SerializeField] EnnemyHeatSystem HeatSystem;
    private HashSet<int> receivedAttacks = new HashSet<int>();

     void Start()
    {
        fightStart = false;
    // Pour eviter de mettre le test de range dans l'update on va faire un scipt secondaire qui invoque les action avec un sphereCollider en trigger
        OnRange+=PrepareAttack;
        NewDestination();
    }
    public virtual void Update()
    {
        
        if (fightStart && !IsAttacking)
        {
            
            BeforeMove();
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
    public virtual void BeforeMove()
    {
        StartCoroutine(DistanceToPlayer());
        if(distanceToPlayerActual> 4)
            {
                 Move(player);
            }
    }
    public virtual void Move(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.position += dir*speed*Time.deltaTime;
    }
    public void NewDestination()
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
     public virtual void updateZonneAttack()
{
    Vector3 dir = (distanceToPlayer).normalized;

    float distance = 4f; 

    zoneAttack.transform.position = transform.position + dir * distance;
}
  public virtual IEnumerator DistanceToPlayer()
{
    distanceToPlayer = player.position - transform.position;
    distanceToPlayerActual = distanceToPlayer.magnitude ;
    yield return new WaitForSeconds(0.2f);
}
   public virtual IEnumerator Attack1()
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
