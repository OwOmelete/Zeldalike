
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public BossManager bossManager;

   void Start()
{
    StartCoroutine(SetActiveFalse());
}
    void OnEnable()
    {
        StartCoroutine(SetActiveFalse());
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touché");
            bossManager.DealDammage();

            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Stalactite"))
        {
            Debug.Log("Stalactite detruit");


            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }

        /*// Si tu veux aussi détruire sur les murs
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        } */
    }
    IEnumerator SetActiveFalse()
    {
        yield return new WaitForSeconds(2f);
         gameObject.SetActive(false);
    }
  
}