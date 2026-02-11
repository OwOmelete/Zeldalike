using System;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private bool toggleMagnet;

    public bool OutsideMagnetToggle
    {
        get => toggleMagnet;
        set => toggleMagnet = value;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Automates" && toggleMagnet)
        {
            other.transform.position = Vector3.Lerp(other.transform.position, gameObject.transform.position, force);
        }
    }
}
