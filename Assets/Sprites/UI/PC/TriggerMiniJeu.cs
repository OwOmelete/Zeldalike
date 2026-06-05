using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerMiniJeu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject Y;
    bool canAccess;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        canAccess=true;
        Y.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        canAccess=false;
        Y.SetActive(false);
        }
    }
    void Update()
    {
         if(Gamepad.current.buttonNorth.wasPressedThisFrame && canAccess )
        {
           if (canvas!=null) canvas.SetActive(true); 
        }
    }
}
