using System;
using System.Collections;
using UnityEngine;

public class Tuyaux : MonoBehaviour
{
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material instanceMaterial;
    public float vitesseDePropagation;
    float ratio ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ratio = vitesseDePropagation;
        instanceMaterial = new Material(baseMaterial);
        if (instanceMaterial != null)
        {
            GetComponent<Renderer>().material = instanceMaterial;
            instanceMaterial.SetFloat("_Ice_Progression", 1);
        }
        StartCoroutine(TurnOnPipe());
    }
    
    IEnumerator TurnOnPipe()
    {
        while (vitesseDePropagation > -7)
        {
            vitesseDePropagation-=Time.deltaTime;
            instanceMaterial.SetFloat("_Ice_Progression",vitesseDePropagation/ratio);
            yield return null;
        }
        yield return null;
    }
}
