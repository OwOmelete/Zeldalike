using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    Button startButton;
    Button settingsButton;
    Button exitButton;
    
    void StartGame()
    {
        Debug.Log("Start Game");
    }
    
    void OpenSettings()
    {
        Debug.Log("Open Settings");
    }
    void CloseSettings()
    {}
    
    void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
