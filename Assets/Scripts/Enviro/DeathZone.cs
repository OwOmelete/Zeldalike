using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public static event Action<Transform> OnFall;
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("coucou");
            OnFall?.Invoke(respawnPoint);
        }
    }
}
