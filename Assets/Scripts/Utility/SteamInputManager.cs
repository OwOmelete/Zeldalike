using Steamworks;
using UnityEngine;

public class SteamInputManager : MonoBehaviour
{
    private InputHandle_t[] _controllers = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];
    private int _controllerCount = 0;

    void Start()
    {
        if (!SteamManager.Initialized) return;
        SteamInput.Init(false);
    }

    void Update()
    {
        if (!SteamManager.Initialized) return;
        SteamInput.RunFrame();
        _controllerCount = SteamInput.GetConnectedControllers(_controllers);
    }

    public InputHandle_t[] GetControllers() => _controllers;
    public int ControllerCount => _controllerCount;
}