using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoolVibrations : MonoBehaviour
{
    public static CoolVibrations Instance { get; private set; }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    
    bool vibrating = false;
    private float timer;
    private void Update()
    {
        
        if (vibrating && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (vibrating && timer <= 0)
        {
            NoVibrations();
        }
    }

    public void CoolVibrate(float duration, float leftStrength, float rightStrength)
    {
        timer = duration;
        vibrating = true;
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(leftStrength, rightStrength);
        }
    }

    void NoVibrations()
    {
        vibrating = false;
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }
}
