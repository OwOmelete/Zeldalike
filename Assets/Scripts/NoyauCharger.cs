using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoyauCharger : MonoBehaviour
{

    private bool Activated;

    [SerializeField] private Door door;
    
    public bool canInteract;

    [SerializeField]
    private GameObject icon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if(!Activated) icon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            if(!Activated) icon.SetActive(true);
        }
    }

    private void Update()
    {
        if (Gamepad.current.leftTrigger.isPressed && canInteract && !Activated)
        {
            NoyauManager.INSTANCE.increaseRemplissage();
            Activated = true;

            if (door != null)
            {
                door.OpenDoor();
            }
        }
    }
}
