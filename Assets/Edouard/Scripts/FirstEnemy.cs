using UnityEngine;

public class FirstEnemy : MonoBehaviour
{
    #region Stats
    [Header("Stats")]
    [SerializeField] private int maxHealthPoints;
    private int currentHealthPoints;

    [SerializeField] private int damagePoints;
    
    [SerializeField] private float movementSpeed;
    #endregion

    private bool hasDecision =  false;
    private int aiDecision;
    
    enum states
    {
        Idle,
        Chasing,
        Attacking,
        Charging
    }
    
    [Header("Settings")]
    [SerializeField] private int detectionRadius;
    [SerializeField] private int attackRadius;
    [SerializeField] private int decisionCooldown;

    private Transform target;

    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("No target");
            FindTarget();
            return;
        }
        BehaviourLogic();
    }

    private void FindTarget()
    {
        Debug.Log("SearchingTarget");
        Collider[] detectionArea = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in detectionArea)
        {
            if (col.CompareTag("Player"))
            {
                target = col.transform;
                Debug.Log("Target Found");
                break;
            }
        }
    }
    
    private void BehaviourLogic()
    {
        float distanceFromTarget = Vector3.Distance(transform.position, target.position);

        if (!hasDecision)
        {
            aiDecision = Roll();
            hasDecision = true;
        }
        
        if (distanceFromTarget < attackRadius)
        {
            hasDecision = true;
            PrepareAttack();
        }

        if (distanceFromTarget > attackRadius && distanceFromTarget < detectionRadius)
        {
            //Chasing
            Debug.Log("ChasingPlayer");
            gameObject.transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
            gameObject.transform.LookAt(target);
        }
        
        if (distanceFromTarget > detectionRadius)
        {
            target = null;
            hasDecision = false;
        }
    }

    private void PrepareAttack()
    {
        Debug.Log("Preparing Attack");
        Attack();
    }
    private void Attack()
    {
        Debug.Log("Attacking");
        
        hasDecision = false;
    }
    
    private int Roll()
    {
        int number = Random.Range(0, 100);
        return number;
    }
}
