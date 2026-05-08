using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttack
{
    public void TryAttack()
    {
        Debug.Log("Melee attack");
    }
}
