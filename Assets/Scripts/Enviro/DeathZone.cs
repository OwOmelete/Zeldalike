using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public static event Action OnFall;

    public static DeathZone Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void setRespawn(Transform t)
    {
        respawnPoint = t;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("coucou");
            OnFall?.Invoke();
        }
    }
    
}
