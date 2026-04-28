using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadDebug : MonoBehaviour
{
    void Update()
    {
        if (Gamepad.current == null)
        {
            Debug.Log("Aucune manette détectée");
            return;
        }

        // Stick gauche
        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
        if (leftStick.magnitude > 0.2f)
        {
            Debug.Log("Stick gauche : " + leftStick);
        }

        // Stick droit
        Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
        if (rightStick.magnitude > 0.2f)
        {
            Debug.Log("Stick droit : " + rightStick);
        }

        // D-pad
        Vector2 dpad = Gamepad.current.dpad.ReadValue();
        if (dpad != Vector2.zero)
        {
            Debug.Log("D-Pad : " + dpad);
        }

        // Boutons principaux
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            Debug.Log("A / Croix appuyé");

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
            Debug.Log("B / Rond appuyé");

        if (Gamepad.current.buttonWest.wasPressedThisFrame)
            Debug.Log("X / Carré appuyé");

        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            Debug.Log("Y / Triangle appuyé");

        // Start / Select
        if (Gamepad.current.startButton.wasPressedThisFrame)
            Debug.Log("Start appuyé");

        if (Gamepad.current.selectButton.wasPressedThisFrame)
            Debug.Log("Select / Back appuyé");

        // Gâchettes
        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
            Debug.Log("LB / L1 appuyé");

        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
            Debug.Log("RB / R1 appuyé");

        if (Gamepad.current.leftTrigger.ReadValue() > 0.2f)
            Debug.Log("LT / L2 : " + Gamepad.current.leftTrigger.ReadValue());

        if (Gamepad.current.rightTrigger.ReadValue() > 0.2f)
            Debug.Log("RT / R2 : " + Gamepad.current.rightTrigger.ReadValue());
    }
}