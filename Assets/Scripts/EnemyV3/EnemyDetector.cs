using System;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private EnemyController enemyControllerReference;
    [SerializeField] private float attackRange = 1f;

    private IAttack _attackInterface;
    private Transform _playerTransform;

    private void Awake()
    {
        _attackInterface = GetComponentInParent<IAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log(other.name);

        _playerTransform = other.transform;

        enemyControllerReference.SetPlayer(_playerTransform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerTransform = null;

        enemyControllerReference.SetPlayer(null);
        enemyControllerReference.SetState(EnemyController.State.IDLE);
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        float distanceToPlayer =
            Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            enemyControllerReference.SetState(EnemyController.State.ATTACK);

            Debug.Log("try attack");

            _attackInterface?.TryAttack();
        }
    }
}