using UnityEngine;

public abstract class EnemyBehaviourSO : ScriptableObject
{
    public abstract void Execute(BasicEnemyV3 enemy);
}