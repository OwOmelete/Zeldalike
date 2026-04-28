using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Attack")]
public class AttackBehaviour : EnemyBehaviourSO
{
    public override void Execute(EnemyController enemy)
    {
        var attack = enemy.GetComponent<EnemyAttack>();
        if (attack == null) return;

        attack.TryAttack();
    }
}