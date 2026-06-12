using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    public float maxlife;
    public float life;
    bool fightStart=false;
    public GameObject playerpref;
    public GameObject enemy;
    public float speed;


    void Start()
    {
        OnEnable();
    }
    void OnEnable()
    {
        life=maxlife;
        fightStart=false;
    }
    void Update()
    {
        if (fightStart)
        {
            Vector3 dir = ( playerpref.transform.position-enemy.transform.position ).normalized;
            enemy.transform.position += dir*Time.deltaTime*speed;
        }
    }
    public void TakeDamage()
    {
        life-=5;
        if(life<=0) Die();
    }
    void Die()
    {

        gameObject.SetActive(false);
    }
    public void AddPlayerRef(Combat2D player)
    {
        playerpref=player.gameObject;
        fightStart=true;
    }
}
