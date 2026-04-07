using System;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    private bool canInteract = false;

    [SerializeField] private GameObject EndScreen;
    [SerializeField] private EndScreen _endScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (canInteract)
        {
            End();
        }
    }

    private void End()
    {
        EndScreen.SetActive(true);
    }

    public void Activate()
    {
        canInteract = true;
    }
}
