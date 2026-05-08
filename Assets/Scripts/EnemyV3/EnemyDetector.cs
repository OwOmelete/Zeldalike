using System;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{

    [SerializeField] private EnemyController enemyControllerReference;
    
    private IAttack _attackInterface;
    
    private Transform _playerTransform;
    
    private void Awake()
    {
        _attackInterface = GetComponentInParent<IAttack>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) {return;}
        
        Debug.Log(other.name);
        
        _playerTransform = other.transform;
        enemyControllerReference.SetPlayer(_playerTransform);
        
        _attackInterface?.TryAttack();
    }
    
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) {return;}
        
        _playerTransform = null;
        enemyControllerReference.SetPlayer(null);
        enemyControllerReference.SetState(EnemyController.State.IDLE);
    }

    void Update()
    {
        if (_playerTransform == null) {return;}
        
        enemyControllerReference.SetState(EnemyController.State.ATTACK);
        
        _attackInterface?.TryAttack();
    }
}
