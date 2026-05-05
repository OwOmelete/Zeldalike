using UnityEngine;
using System.Collections;

public class BouclierAttack : MonoBehaviour, IAttack
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float cooldown = 1.5f;

    private float _lastAttackTime;
    private Coroutine _attackRoutine;

    private EnemyController enemyControllerReference;
    
    void Awake()
    {
        enemyControllerReference = GetComponentInParent<EnemyController>();

        if (enemyControllerReference == null) {Debug.LogError("EnemyController not found on " + gameObject.name);}
    }

    public void TryAttack()
    {
        TryFire();
    }

    void TryFire()
    {
        if (enemyControllerReference.Player == null) {return;}
        if (_attackRoutine != null) {return;}

        if (Time.time < _lastAttackTime + cooldown) {return;}

        _attackRoutine = StartCoroutine(Fire());
        _lastAttackTime = Time.time;
    }

    IEnumerator Fire()
    {
        enemyControllerReference.SetState(EnemyController.State.ATTACK);

        Vector3 targetPosition = enemyControllerReference.Player.position;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity
        );

        var p = proj.GetComponent<BouclierProjectile>();

        if (p == null)
        {
            Debug.LogError("Projectile missing BouclierProjectile script");
            yield break;
        }

        p.Init(targetPosition, enemyControllerReference.Stats.damage);

        yield return new WaitForSeconds(0.3f);

        _attackRoutine = null;

        if (enemyControllerReference.Player != null) 
        {enemyControllerReference.SetState(EnemyController.State.CHASE);}
        
        else {enemyControllerReference.SetState(EnemyController.State.IDLE);}
    }
}