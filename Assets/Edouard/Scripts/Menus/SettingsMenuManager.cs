using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuCanvas;
    [SerializeField] private Button backFromSettingsButton;
    
    public static SettingsMenuManager Instance;
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
        
        backFromSettingsButton.onClick.AddListener(ToggleSettings);
    }
    
    
    private void OnEnable()
    {
        GlobalEvents.OnSettingsButtonPressed += ToggleSettings;
    }
    

    public void ToggleSettings()
    {
        Debug.Log($"Settings toggled {settingsMenuCanvas.activeSelf}");
        settingsMenuCanvas.SetActive(!settingsMenuCanvas.activeSelf);
    }
}
