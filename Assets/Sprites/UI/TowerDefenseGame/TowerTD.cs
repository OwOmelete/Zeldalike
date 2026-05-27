using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTD : MonoBehaviour
{
    [Header("StatsTours")]
    [SerializeField] float damage;
    [SerializeField] float attackSpeed;
    
    [SerializeField] List <EnnemyTD> ennemyTD = new List <EnnemyTD>();
    //EnnemyTD ennemyTD;
    [Header("Contexte")]
    
    bool isInRange;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInRange = true;
            ennemyTD.Add(other.GetComponent<EnnemyTD>()) ;
            StartCoroutine(Attack());
        }
    }
    void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Enemy"))
        {
            ennemyTD.Remove(other.GetComponent<EnnemyTD>()) ;
            //ennemyTD = null;
            StopCoroutine(Attack());
        } 
    }
    IEnumerator Attack()
    {
        while (isInRange&&ennemyTD[0] != null)
        {
           
              ennemyTD[0].takeDammage(damage); 
              if (ennemyTD[0].life <= 0)
            {
                ennemyTD.Remove(ennemyTD[0]);
            }
            //if (ennemyTD[0] != null) StopCoroutine(Attack());
            yield return new WaitForSeconds(attackSpeed);  
            
        }
        
    }
}
