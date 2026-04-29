using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Chase")]
public class ChaseBehaviour : EnemyBehaviourSO
{
    public override void Execute(EnemyController enemy)
    {
        if (enemy.Player == null) return;

        Vector3 target = enemy.Player.position;

        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            target,
            enemy.Stats.moveSpeed * Time.deltaTime
        );

        Vector3 dir = target - enemy.transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
            enemy.transform.rotation = Quaternion.LookRotation(dir);
    }
}