using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public static event Action<Transform> OnFall;
    public static event Action<Transform> Invo;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnFall?.Invoke(respawnPoint);
        }
    }
}
