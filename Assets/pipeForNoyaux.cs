using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PipeForNoyaux : MonoBehaviour
{
    private bool canInteract;
    [SerializeField] private Collider col;
    [SerializeField] private GameObject go;
    [SerializeField] private GameObject Tuyaux;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material instanceMaterial;
    public float vitesseDePropagation;
    float ratio ;
    
    void Start()
    {
        if (Tuyaux != null)
        {
            ratio=vitesseDePropagation;
            instanceMaterial = new Material(baseMaterial);
            Tuyaux.GetComponent<Renderer>().material = instanceMaterial;
            instanceMaterial.SetFloat("_Ice_Progression",1); 
        }
    }

    private void OnEnable()
    {
        InvoManager.FireWaveAction += Interact;
    }

    private void OnDisable()
    {
        InvoManager.FireWaveAction -= Interact;
    }
    

    private void Interact()
    {
        if(canInteract) 
        {
            if (Tuyaux != null) StartCoroutine(TurnOnPipe());
        }
        
    }
    
    IEnumerator TurnOnPipe()
    {
        
        while (vitesseDePropagation > -2)
        {
            vitesseDePropagation-=Time.deltaTime;
            instanceMaterial.SetFloat("_Ice_Progression",vitesseDePropagation/ratio);
            yield return null;
        }
        yield return null;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FireWave"))
        {
            Interact();
            if (Tuyaux != null) StartCoroutine(TurnOnPipe());
        }
        /*if (other.CompareTag("Player"))
        {
            canInteract = true;
        }*/
    }
    
    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }*/
}
