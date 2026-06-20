using System;
using UnityEngine;

public class BridgeButton : MonoBehaviour, IDamagable
{
    public Animator[] linkedBridges;
    public GameObject[] linkedColliders;
    public BridgesManager Manager;
    public Animator levierAnimator;

    private float lastButtonPress= -5;
    private float buttonCooldown = 0.5f;

    public void TakeDamage(float damage, int attackID, InvoDataInstance data)
    {
        if (Time.time - lastButtonPress > buttonCooldown)
        {
            lastButtonPress = Time.time;
            Manager.activateBridges(linkedBridges);
            Manager.activateColliders(linkedColliders);
            if (levierAnimator) levierAnimator.SetBool("LevierActivator", !levierAnimator.GetBool("LevierActivator"));
        }
    }
}
