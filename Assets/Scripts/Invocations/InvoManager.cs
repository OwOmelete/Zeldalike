using System;
using System.Collections.Generic;
using UnityEngine;

public class InvoManager : MonoBehaviour
{
    public List<InvoDataInstance> InvoList = new List<InvoDataInstance>();
    public InvoAttack InvoAttack;
    
    public static event Action<InvoDataInstance> OnAttack;


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnAttack?.Invoke(InvoList[0]);
        }
    }
}
