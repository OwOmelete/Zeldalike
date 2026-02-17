using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    Button startButton;
    Button settingsButton;
    Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            GlobalEvents.ButtonPressed();
            StartGame();
        });
        settingsButton.onClick.AddListener(() =>
        {
            GlobalEvents.ButtonPressed();
            GlobalEvents.SettingsButtonPressed();
        });
        exitButton.onClick.AddListener(() =>
        {
            GlobalEvents.ButtonPressed();
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
