using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/RangedAttack")]
public class RangedAttackBehaviour : EnemyBehaviourSO
{
    public override void Execute(EnemyController enemy)
    {
        if (enemy.Player == null) return;

        var attack = enemy.GetComponent<EnemyRangedAttack>();
        if (attack == null) return;

        attack.TryFire();
    }
}