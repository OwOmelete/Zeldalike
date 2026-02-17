using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
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
    
    private SettingsMenuState _settingsMenuState;

    enum SettingsMenuState
    {
        active,
        inactive,
    }
    
    private void OnEnable()
    {
        GlobalEvents.OnSettingsButtonPressed += OpenSettings;
    }

    public void OpenSettings()
    {
        Debug.Log("Open Settings");
        gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        Debug.Log("Close Settings");
        gameObject.SetActive(false);
    }
}
