using UnityEngine;
using System.Collections;

public class DetectionAttack : MonoBehaviour
{
    [SerializeField] PlayerHealthComponent playerHealth;
    public bool canAttack ;

    void Start()
    {
        canAttack=true;
    }
    void OnTriggerStay(Collider hit)
    {
          if (canAttack)
        {
        playerHealth.TakeDamage(5);
        Debug.Log("joueur touché");
        canAttack=false;
        }
    }
}
