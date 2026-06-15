using UnityEngine;

public class DashDisable : MonoBehaviour
{
    public bool canDash;
    public DeplacementUnite2D deplacementUnite2D;
    void OnTriggerEnter2D(Collider2D collision)
    {
        deplacementUnite2D.isDashing=canDash;
    }
}
