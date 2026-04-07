using System;
using UnityEngine;

public class InteractibleButton : MonoBehaviour
{


    [SerializeField] private Renderer _renderer;
    [SerializeField] private Elevator elevator;
    
    private bool canInteract = false;
    




    private void OnTriggerEnter(Collider other)
    {
        elevator.Activate();
        _renderer.material.color = Color.yellowNice;
    }
    
}
