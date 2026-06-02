using UnityEngine;

public class ObstacleTD : MonoBehaviour
{
    [Header("StatsObstacle")]
    public float life;
    float maxLife;
    public float attack;
    public float CouldownAttack;
    public bool isFighting;
    void Start()
    {
        maxLife = life;
    }
    public void TakeDamage(float dammage)
    {
        life -= dammage;
        if(life<=0) Die();
    }
    void Die()
    {
        
        gameObject.SetActive(false);
        life = maxLife;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
