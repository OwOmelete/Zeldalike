using UnityEngine;
using System.Collections;

public class DetectionAttack : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] float damage;
    public bool canAttack ;

    void Start()
    {
        canAttack=true;
    }
    void OnTriggerStay(Collider hit)
    {
          if (canAttack)
        {
        playerHealth.takeDamage(damage);
        canAttack=false;
        }
    }
}
