using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttack
{
    public void TryAttack()
    {
        Debug.Log("Ranged attack");
    }
}
