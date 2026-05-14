using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AttackOnRange : MonoBehaviour
{
     [SerializeField] EnnemyRework ennemi;
     [SerializeField] bool canAttack;

    void Start()
    {
        canAttack=true;
    }
    void OnTriggerStay(Collider other)
    {
         if (canAttack)
        {
        ennemi.OnRange.Invoke();
        StartCoroutine(Cooldown());
        }
    }
    IEnumerator Cooldown()
    {
        canAttack=false;
        yield return new WaitForSeconds(3f);
        canAttack=true;
        yield return new WaitForEndOfFrame();
    }

}
