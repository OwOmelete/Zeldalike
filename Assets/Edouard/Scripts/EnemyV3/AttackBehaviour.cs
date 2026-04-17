using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Attack")]
public class AttackBehaviour : EnemyBehaviourSO
{
    public override void Execute(BasicEnemyV3 enemy)
    {
        if (!enemy.IsAttacking)
        {
            enemy.StartCoroutine(enemy.AttackRoutine());
        }
    }
}
