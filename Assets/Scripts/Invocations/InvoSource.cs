using System;
using System.Collections;
using UnityEngine;

public class InvoSource : MonoBehaviour
{
    private bool canInteract;
    [SerializeField] private Collider col;
    [SerializeField] private GameObject go;
    [SerializeField] private GameObject Tuyaux;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material instanceMaterial;
    public float vitesseDePropagation;
    float ratio ;
    
    public static event Action ReleaseInvos;
    
    void Start()
    {
        ratio=vitesseDePropagation;
        instanceMaterial = new Material(baseMaterial);
        Tuyaux.GetComponent<Renderer>().material = instanceMaterial;
        instanceMaterial.SetFloat("_Ice_Progression",1);
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
            releaseInvos();
            StartCoroutine(TurnOnPipe());
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
    private void releaseInvos()
    {
        col.enabled = false;

        Collider parentCol = transform.parent != null 
            ? transform.parent.GetComponent<Collider>() 
            : null;

        if (parentCol != null)
        {
            parentCol.enabled = false;
        }

        int childCount = transform.childCount;

        for (int i = childCount - 1; i > -1; i--)
        {
            Transform t = transform.GetChild(i);
            t.gameObject.SetActive(true);
        }
        
        ReleaseInvos?.Invoke();
        Destroy(go);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
