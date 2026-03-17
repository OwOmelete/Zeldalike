using UnityEngine;
using System;

public static class GlobalEvents
{
    public static event Action OnButtonPressed;

    public static void ButtonPressed()
    {
        OnButtonPressed?.Invoke();
    }
    
    public static event Action OnSettingsButtonPressed;

    public static void SettingsButtonPressed()
    {
        OnSettingsButtonPressed?.Invoke();
    }
    
    public static event Action OnPauseButtonPressed;

    public static void PauseButtonPressed()
    {
        OnPauseButtonPressed?.Invoke();
    }
}