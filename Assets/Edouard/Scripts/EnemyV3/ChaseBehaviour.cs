using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Chase")]
public class ChaseBehaviour : EnemyBehaviourSO
{
    public override void Execute(EnemyController enemy)
    {
        if (enemy.Player == null) return;
        if (enemy.CurrentState != EnemyController.State.CHASE) return;

        Vector3 target = enemy.Player.position;

        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            target,
            enemy.Stats.moveSpeed * Time.deltaTime
        );

        Vector3 lookDir = target - enemy.transform.position;
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
            enemy.transform.rotation = Quaternion.LookRotation(lookDir);
    }
}