
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public int damage = 10;

   void Start()
{
    Destroy(gameObject, 2f);
}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touché");

            // TODO : appliquer dégâts au joueur
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Stalactite"))
        {
            Debug.Log("Stalactite detruit");


            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        // Si tu veux aussi détruire sur les murs
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        } 
    }
  
}