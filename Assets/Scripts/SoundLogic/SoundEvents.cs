using System;
using UnityEngine;
public static class SoundEvents
{
    public static event Action OnButtonPress;

    public static void MenuButtonPress()
    {
        OnButtonPress?.Invoke();
    }
}
