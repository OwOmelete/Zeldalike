using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemies/Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public float maxHealth;
    public int damage;
    public float moveSpeed;
}