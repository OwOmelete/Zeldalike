using System;
using Unity.VisualScripting;
using UnityEngine;

public class DidacticielTrigger : MonoBehaviour
{
    [SerializeField] private int index;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))Didacticiel.Instance?.DisplayPopup(index);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))Didacticiel.Instance?.HideAllPopups();
    }
}
