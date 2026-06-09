using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTD : MonoBehaviour
{
    [Header("StatsTours")]
    [SerializeField] float damage;
    [SerializeField] float attackSpeed;
    [SerializeField] float reduceMoveSpeed;
     [SerializeField] float burnDammage;
    bool isAttacking;

    
    [SerializeField] List <EnnemyTD> ennemyTD = new List <EnnemyTD>();
    //EnnemyTD ennemyTD;
    [Header("Contexte")]
    
    bool isInRange;
    void Start()
    {
        isAttacking=false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")&&other.gameObject.activeSelf)
        {
            isInRange = true;
            ennemyTD.Add(other.GetComponent<EnnemyTD>()) ;
            if (!isAttacking)StartCoroutine(Attack());
        }
    }
    void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Enemy"))
        {
            Debug.Log("AsRemoved");
            ennemyTD.Remove(other.GetComponent<EnnemyTD>()) ;
            //ennemyTD = null;
            StopCoroutine(Attack());
            if (ennemyTD.Count == 0) isAttacking=false;
        } 
    }
    IEnumerator Attack()
    {
        isAttacking=true;
        while (isInRange&&ennemyTD.Count>0)
        {
           
              ennemyTD[0].takeDammage(damage); 
              if (reduceMoveSpeed>0) ennemyTD[0].Slow(reduceMoveSpeed); 
              if (burnDammage>0) ennemyTD[0].Burn(burnDammage); 
              if (!ennemyTD[0].gameObject.activeSelf)
            {
                Debug.Log("AsRemoved");
                ennemyTD.Remove(ennemyTD[0]);
            }
            //if (ennemyTD[0] != null) StopCoroutine(Attack());
            yield return new WaitForSeconds(attackSpeed);  
            
        }
        
    }
}
