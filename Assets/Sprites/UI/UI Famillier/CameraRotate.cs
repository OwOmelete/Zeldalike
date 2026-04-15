using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCameraHolder : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrés par seconde
    private Quaternion initialRotation;

    void Awake()
    {
        // Stocke la rotation initiale au départ
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (Gamepad.current == null) return;

        // Tourne à droite tant que RB est maintenu
        if (Gamepad.current.rightShoulder.isPressed)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        // Tourne à gauche tant que LB est maintenu
        if (Gamepad.current.leftShoulder.isPressed)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    public void ResetCamera()
    {
        transform.rotation = initialRotation; // ← remet exactement à la rotation de départ
    }
}