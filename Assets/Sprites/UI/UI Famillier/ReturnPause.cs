using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnPause : MonoBehaviour
{
    public uiManager uiManager;
    void Update()
    {
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            
            if(!uiManager.isInPC)uiManager.TogglePauseMenu();
            gameObject.SetActive(false);
        }
    }
}
