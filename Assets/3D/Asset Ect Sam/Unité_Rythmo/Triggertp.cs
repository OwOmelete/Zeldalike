using UnityEngine;

public class Triggertp : MonoBehaviour
{
   public DeplacementUnite2D deplacementUnite2D;
   public GameObject spawn;
   public GameObject switch1;
   public GameObject switch2;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            deplacementUnite2D.gameObject.transform.position -= new Vector3 (spawn.transform.position.x-640f,spawn.transform.position.y-400f,0);
            switch1.SetActive(false);
            switch2.SetActive(true);
        }
    }
}
