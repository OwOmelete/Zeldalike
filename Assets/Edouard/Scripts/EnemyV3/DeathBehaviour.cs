using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Death")]
public class DeathBehaviour : EnemyBehaviourSO
{
    public override void Execute(EnemyController enemy)
    {
        Object.Destroy(enemy.gameObject);
    }
}