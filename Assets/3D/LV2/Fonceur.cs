using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Fonceur : EnnemyRework
{
    public Transform Cible ;
    public bool dash=false;
    public bool canAttack=true;
    public PlayerHealth playerHealth;
    public bool asAttack;

    void Awake()
    {
        canAttack=true;
    }
         private void MiseAngle()
{
    Vector3 direction = player.transform.position - transform.position;
    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f)
        return;

    float angle = Vector3.SignedAngle(
        MainCamera.transform.forward,
        direction.normalized,
        Vector3.up
    );

    if (angle < 0)
        angle += 360f;

    int dir = AngleToInt(angle);

    Vector2[] directions =
    {
        new Vector2( 0, -1),
        new Vector2(-1, -1),
        new Vector2(-1,  0), 
        new Vector2(-1,  1), 
        new Vector2( 0,  1), 
        new Vector2( 1,  1), 
        new Vector2( 1,  0), 
        new Vector2( 1, -1)
    };

    animator.SetFloat("x", directions[dir].x);
    animator.SetFloat("y", directions[dir].y);
}

private int AngleToInt(float angle)
{
    const int nbDirections = 8;
    float sectorSize = 360f / nbDirections;

    angle += sectorSize * 0.5f;
    angle %= 360f;

    return Mathf.FloorToInt(angle / sectorSize);
}
    public override void Update()
    {
        if(!animator.GetBool("isStun"))MiseAngle();
        if (!HeatSystem.isAlive)
        {
            if (MortGeulGlacon != null)
            {
                MortGeulGlacon.SetActive(true);
                MortGeulGlacon.transform.position = transform.position + new Vector3(0,1.5f,0); 
            }
            if (_combatZone!= null)
            {
                foreach (var door in _combatZone.doors)
                {
                    door.OpenDoor();
                } 
            }


            Destroy(gameObject,0.5f);
        }
        if (dash)
        {
            Debug.Log("Move");
            Move(Cible);
        }
        if (fightStart && !IsAttacking)
        {
            
            BeforeMove();
        }
        else if (!fightStart && !IsPatrol)
        {
           if(patrol.Count>0) 
           {
            Vector3 distance = patrol[nextDestination].position - transform.position;
            if (Math.Abs(distance.magnitude) < 1)
            {
                NewDestination();
            }
            Move(patrol[nextDestination]);
        }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        player = InvoManager.Instance.transform;
        playerHealth = InvoManager.Instance._playerHealth;
        

    }


    void OnSceneLoaded()
    {
        
    }

    public override void BeforeMove()
    {
        StartCoroutine(DistanceToPlayer());
        
        if(distanceToPlayerActual> 8 && !IsAttacking)
            {
                 Move(player);
            }
        else if (!IsAttacking&&canAttack)
        {
            Cible.position = player.position-transform.position;
            Vector3 dir = (Cible.transform.position).normalized;
            float distance = 15f; 
            Cible.position = transform.position + dir * distance;
            
            StartCoroutine(Attack1());
        }
        else
        {
            StartCoroutine(Turn(player));
        }
        
    }
    
    IEnumerator Cooldown()
    {
        canAttack=false;
        yield return new WaitForSeconds(4);
        canAttack=true;
        yield return new WaitForEndOfFrame();
    }
    
     IEnumerator Turn(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir = new Vector3 (dir.z,0,-dir.x);
        dir.Normalize();
        transform.position += dir * (speed * Time.deltaTime);
        yield return new WaitForSeconds(3f);
        
    }
    public override IEnumerator Attack1()
    {
        IsAttacking = true;
        animator.SetTrigger("Charging");
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("isCharging",true);
    
    zoneAttack.SetActive(true); 
    updateZonneAttack();
    yield return new WaitForEndOfFrame();

    yield return new WaitForSeconds(attackSpeed*0.5f);

    
    speed = 20;
    dash=true;

    yield return new WaitForSeconds(0.5f);
    dash=false;
    speed = 10;
    yield return new WaitForSeconds(attackSpeed*0.2f);
    var col = zoneAttack.GetComponent<Collider>();
    col.enabled = true;
    animator.SetBool("isCharging",false);
    animator.SetBool("isStun",true);

    yield return new WaitForSeconds(0.1f);
    zoneAttack.GetComponent<DetectionAttack>().canAttack = true;
    col.enabled = false;
    zoneAttack.SetActive(false);
    
    yield return new WaitForSeconds(2f);
    animator.SetBool("isStun",false);
    IsAttacking = false;
    asAttack = false;
    
    StartCoroutine(Cooldown());
    
    }
     public override void Move(Transform target)
    {
        animator.SetBool("isMoving",true);
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.position += dir*speed*Time.deltaTime;
    }
}
