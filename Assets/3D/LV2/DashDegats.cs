using UnityEngine;

public class DashDegats : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    public Fonceur fonceur;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
  void OnTriggerEnter(Collider other)
    {
        
        if (fonceur.IsAttacking && !fonceur.asAttack && other.CompareTag("Player"))
        {
        Debug.Log("Il a attaquer !!!");
        fonceur.playerHealth.takeDamage(20);
        fonceur.asAttack = true;
        }
    }
}
