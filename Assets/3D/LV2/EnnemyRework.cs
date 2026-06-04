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
    [SerializeField] public bool isIdle;
    [SerializeField] Vector3 distanceToPlayer;

    public float distanceToPlayerActual;


    [Header("Previsualization")]
    [SerializeField] public GameObject zoneAttack;

    public EnnemyHeatSystem HeatSystem;
    private HashSet<int> receivedAttacks = new HashSet<int>();
    public Animator animator;
    public Camera MainCamera;
    public GameObject MortGeulGlacon;
    [SerializeField] private CombatZone _combatZone;
    

     void Start()
    {
        fightStart = false;
    // Pour eviter de mettre le test de range dans l'update on va faire un scipt secondaire qui invoque les action avec un sphereCollider en trigger
        OnRange+=PrepareAttack;
        NewDestination();
    }
    public virtual void Update()
    {
        MiseAngle();
        if (!HeatSystem.isAlive)
        {
            if (MortGeulGlacon != null)
            {
                foreach (var door in _combatZone.doors)
                {
                    door.OpenDoor();
                }
                
                MortGeulGlacon.SetActive(true);
                MortGeulGlacon.transform.position = transform.position + new Vector3(0,1.5f,0); 
            }
            
            Destroy(gameObject);
        }
        
        
        if (fightStart && !IsAttacking)
        {
            
            BeforeMove();
        }
        else if (!fightStart && !IsPatrol)
        {
            Vector3 distance = patrol[nextDestination].position - transform.position;
            if (Math.Abs(distance.magnitude) < 1 && !isIdle)
            {
              StartCoroutine(NewDestination());
                
            }
            else if (!isIdle)
            {
                Move(patrol[nextDestination]);
            }
            
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

        animator.SetBool("IsMoving",true);
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.position += dir*speed*Time.deltaTime;
        
        
        if (!IsAttacking) transform.LookAt(target);
    }
      private void MiseAngle()
    {
        float currentRotation = (transform.rotation * Quaternion.Inverse(MainCamera.transform.rotation) ).eulerAngles.y;
        animator.SetFloat("x",angleToInt(currentRotation));
    }

    private int angleToInt(float angle)
    {
        float curAngle = 360 / 8; 
        angle -= curAngle/2;
        if (angle < 0) angle += 360;

        return Mathf.FloorToInt(angle / curAngle);

    }
    public IEnumerator NewDestination()
    {
        isIdle=true;
        animator.SetBool("IsMoving",false);
        yield return new WaitForSeconds(4f);
        System.Random rnd = new System.Random();
        nextDestination  = rnd.Next(0, patrol.Count);
        isIdle=false;
        yield return null;
        
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
    animator.SetTrigger("Attack");
    yield return new WaitForEndOfFrame();

    yield return new WaitForSeconds(attackSpeed*0.5f);

    
    speed = 15;
    float i = 0.2f;
        while (i > 0)
        {
            i-=Time.deltaTime;
            Move(player);
            yield return null;
        }
    speed = 5;
    yield return new WaitForSeconds(attackSpeed*0.2f);
    var col = zoneAttack.GetComponent<Collider>();
    col.enabled = true;

    yield return new WaitForSeconds(0.1f);
    zoneAttack.GetComponent<DetectionAttack>().canAttack = true;
    col.enabled = false;
    zoneAttack.SetActive(false);
    IsAttacking = false;
    
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
