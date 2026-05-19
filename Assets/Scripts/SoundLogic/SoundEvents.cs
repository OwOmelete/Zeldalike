using System;
public static class SoundEvents
{
    public static event Action OnButtonPress;

    public static void MenuButtonPress()
    {
        OnButtonPress?.Invoke();
    }
}
