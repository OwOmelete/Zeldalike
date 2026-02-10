using System;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Automates")
        {
            other.transform.position = Vector3.Lerp(other.transform.position, gameObject.transform.position, force);
        }
    }
}
