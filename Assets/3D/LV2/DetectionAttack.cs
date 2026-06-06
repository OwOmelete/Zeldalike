using UnityEngine;
using System.Collections;

public class DetectionAttack : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] float damage;
    public bool canAttack ;
    private GameObject player;

    void Start()
    {
        player = InvoManager.Instance.gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        canAttack=true;
    }
    void OnTriggerStay(Collider hit)
    {
          if (canAttack && hit.CompareTag("Player"))
        {
        playerHealth.takeDamage(damage);
        canAttack=false;
        }
    }
}
