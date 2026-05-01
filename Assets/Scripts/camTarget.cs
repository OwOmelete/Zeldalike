using System;
using UnityEngine;

public class camTarget : MonoBehaviour
{
    public static camTarget Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }
}
