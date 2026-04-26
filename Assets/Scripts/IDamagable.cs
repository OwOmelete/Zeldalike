using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage, int attackID, InvoDataInstance data);
}
