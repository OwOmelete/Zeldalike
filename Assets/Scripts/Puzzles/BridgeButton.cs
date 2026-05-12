using System;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public Animator[] linkedBridges;
    public BridgesManager Manager;

    private float lastButtonPress= -5;
    private float buttonCooldown = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Invo"))
        {
            if (Time.time - lastButtonPress > buttonCooldown)
            {
                lastButtonPress = Time.time;
                Manager.activateBridges(linkedBridges);
            }
        }
    }
}
