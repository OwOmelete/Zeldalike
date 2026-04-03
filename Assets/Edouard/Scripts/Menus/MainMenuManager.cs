using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            Debug.Log("Start Button Pressed");
            GlobalEvents.ButtonPressed();
            StartGame();
        });
        settingsButton.onClick.AddListener(() =>
        {
            Debug.Log("Settings Button Pressed");
            GlobalEvents.ButtonPressed();
            GlobalEvents.SettingsButtonPressed();
        });
        exitButton.onClick.AddListener(() =>
        {
            Debug.Log("Exit Button Pressed");
            GlobalEvents.ButtonPressed();
            ExitGame();
        });
    }
    
    void StartGame()
    { Debug.Log("Start Game"); }
    
    void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
