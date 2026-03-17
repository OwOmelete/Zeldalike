using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private Button settingsButton;
    
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
        
        settingsButton.onClick.AddListener(() =>
        {
            Debug.Log("Settings Button Pressed");
            GlobalEvents.ButtonPressed();
            GlobalEvents.SettingsButtonPressed();
        });

    }

    private void OnEnable()
    {
        GlobalEvents.OnPauseButtonPressed += TogglePauseMenu;
    }
    
    void TogglePauseMenu()
    {
        pauseMenuCanvas.SetActive(!pauseMenuCanvas.activeSelf);
    }
}
