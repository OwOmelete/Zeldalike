using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputs : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
    }
    
    private void OnToggleSettings()
    {
        GlobalEvents.SettingsButtonPressed();
    }
}
