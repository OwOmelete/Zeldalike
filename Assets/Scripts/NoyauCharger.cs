using System;
using UnityEngine;

public class NoyauCharger : MonoBehaviour
{

    private bool Activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!Activated && other.CompareTag("Player"))
        {
            Debug.Log("hihihihihi");
            NoyauManager.INSTANCE.increaseRemplissage();
            Activated = true;
        }
    }
}
