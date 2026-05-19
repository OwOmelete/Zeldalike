using UnityEngine;
using System.Collections;

public class DetectionAttack : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    public bool canAttack ;

    void Start()
    {
        canAttack=true;
    }
    void OnTriggerStay(Collider hit)
    {
          if (canAttack)
        {
        playerHealth.takeDamage(5);
        Debug.Log("joueur touché");
        canAttack=false;
        }
    }
}
