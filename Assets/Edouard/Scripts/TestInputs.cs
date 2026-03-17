using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputs : MonoBehaviour
{
    void OnPauseMenu()
    {
        Debug.Log("PAUSE MENU");
        GlobalEvents.PauseButtonPressed();
    }
}
