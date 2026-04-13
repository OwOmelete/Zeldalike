using System;

public static class GlobalEvents
{
    #region ButtonEvents

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
    
    public static event Action OnPauseSelecting;

    public static void PauseSelecting(int mood)
    {
        OnPauseSelecting?.Invoke(/*mood.ToString()*/);
    }

    #endregion

    #region EnemyEvents

    public static event Action OnEnemyAttack;

    public static void EnemyAttack()
    {
        OnEnemyAttack?.Invoke();
    }

    public static event Action OnEnemyMove;

    public static void EnemyMove()
    {
        OnEnemyMove?.Invoke();
    }

    #endregion
}