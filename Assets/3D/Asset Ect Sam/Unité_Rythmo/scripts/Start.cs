using UnityEngine;
using UnityEngine.InputSystem;

public class Start : MonoBehaviour
{
    public GameObject go;
    public GameObject PC;
    void Update()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            go.SetActive(false);
            PC.SetActive(true);
        } 
    }
}
