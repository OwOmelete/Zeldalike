using UnityEngine;
using UnityEngine.InputSystem;

public class Cristaux2D : MonoBehaviour
{
  public Combat2D combat2D;
  public Famillier2D famillier2D;
  bool canInteract;
  bool isTaken;

    void Update()
    {
        if (canInteract && Gamepad.current.leftTrigger.ReadValue() > 0.2f&&!isTaken)
        {
            famillier2D.gameObject.SetActive(true);
             combat2D.familiers.Add(famillier2D);
             famillier2D.refToPlayer(combat2D);
             isTaken=true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
            combat2D = collision.gameObject.GetComponent<Combat2D>();
           
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
           
        }
    }
}
