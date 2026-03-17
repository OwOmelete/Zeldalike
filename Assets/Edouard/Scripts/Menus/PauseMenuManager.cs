using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private Button toSettingsButton;
    [SerializeField] private Button toMainMenuButton;
    
    public static PauseMenuManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        toSettingsButton.onClick.AddListener(() =>
        {
            Debug.Log("Settings Button Pressed");
            GlobalEvents.ButtonPressed();
            GlobalEvents.SettingsButtonPressed();
        });
        toMainMenuButton.onClick.AddListener(() =>
        {
            Debug.Log("Main Menu Button Pressed");
        });

    }

    private void OnEnable()
    {
        GlobalEvents.OnPauseButtonPressed += TogglePauseMenu;
    }
    
    void TogglePauseMenu()
    {
        pauseMenuCanvas.SetActive(!pauseMenuCanvas.activeSelf);
        Debug.Log($"IsPaused: {pauseMenuCanvas.activeSelf}");
        if (pauseMenuCanvas.activeSelf){Time.timeScale = 1;}
        else{Time.timeScale = 0;}
    }
}
