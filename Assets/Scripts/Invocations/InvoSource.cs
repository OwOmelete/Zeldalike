using System;
using UnityEngine;

public class InvoSource : MonoBehaviour
{
    private bool canInteract;
    [SerializeField] private Collider col;
    [SerializeField] private GameObject go;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canInteract)
        {
            releaseInvos();
        }
    }

    private void releaseInvos()
    {
        col.enabled = false;
        int childCount = transform.childCount;
        
        
        for (int i = childCount-1; i > -1; i--)
        {
            Transform t = transform.GetChild(i);
            t.gameObject.SetActive(true);
            t.SetParent(null);
        }
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
