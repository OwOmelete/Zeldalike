using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Death")]
public class DeathBehaviour : EnemyBehaviourSO
{
    public override void Execute(BasicEnemyV3 enemy)
    {
        Object.Destroy(enemy.gameObject);
    }
}
