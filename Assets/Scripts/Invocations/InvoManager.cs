using System;
using System.Collections.Generic;
using UnityEngine;

public class InvoManager : MonoBehaviour
{
    public List<InvoDataInstance> InvoList = new List<InvoDataInstance>();


    private void OnEnable()
    {
        InvoBehaviour.OnInvoSpawn += AddInvocation;
    }

    private void OnDisable()
    {
        InvoBehaviour.OnInvoSpawn -= AddInvocation;
    }

    private void AddInvocation(InvoDataInstance instance)
    {
        InvoList.Add(instance);
    }
    
}
