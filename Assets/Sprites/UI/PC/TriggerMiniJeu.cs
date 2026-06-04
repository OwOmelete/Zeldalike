using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerMiniJeu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject Y;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        StartCoroutine(waitForInput());
        Y.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        StopCoroutine(waitForInput());
        Y.SetActive(false);
        }
    }
    IEnumerator waitForInput()
    {
        while (true)
        {
        if(Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            canvas.SetActive(true); 
        }
            
        yield return null; 
        }
    }
}
