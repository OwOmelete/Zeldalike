using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Steamworks;

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
            timer -= Time.unscaledDeltaTime;
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

        if (SteamManager.Initialized) 
        {
            InputHandle_t[] inputHandles = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];
            int controllerCount = SteamInput.GetConnectedControllers(inputHandles);

            ushort leftSpeed = (ushort)Mathf.Clamp(leftStrength * 65535f, 0f, 65535f);
            ushort rightSpeed = (ushort)Mathf.Clamp(rightStrength * 65535f, 0f, 65535f);

            for (int i = 0; i < controllerCount; i++)
            {
                SteamInput.TriggerVibration(inputHandles[i], leftSpeed, rightSpeed);
            }
        }
    }

    void NoVibrations()
    {
        vibrating = false;

        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }

        if (SteamManager.Initialized)
        {
            InputHandle_t[] inputHandles = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];
            int controllerCount = SteamInput.GetConnectedControllers(inputHandles);

            for (int i = 0; i < controllerCount; i++)
            {
                SteamInput.TriggerVibration(inputHandles[i], 0, 0);
            }
        }
    }
}