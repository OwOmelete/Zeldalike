using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuCanvas;
    
    #region Singleton
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
    }
    #endregion
    
    
    private void OnEnable()
    {
        GlobalEvents.OnSettingsButtonPressed += ToggleSettings;
    }

    public void ToggleSettings()
    {
        settingsMenuCanvas.SetActive(!settingsMenuCanvas.activeSelf);
    }
}
