using System;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public GameObject[] linkedBridges;
    public BridgesManager Manager;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Invo"))
        {
            Manager.activateBridges(linkedBridges);
        }
    }
}
