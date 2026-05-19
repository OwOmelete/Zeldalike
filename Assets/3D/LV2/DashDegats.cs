using UnityEngine;

public class DashDegats : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    public Fonceur fonceur;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
  void OnTriggerEnter(Collider other)
    {
        
        if (fonceur.IsAttacking)
        {
        Debug.Log("Il a attaquer !!!");
        playerHealth.takeDamage(20);
        }
    }
}
