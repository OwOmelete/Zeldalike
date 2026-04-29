using UnityEngine;
using UnityEngine.InputSystem;
public class AnimatorController : MonoBehaviour
{
    public Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
        if (leftStick.magnitude > 0.2f)
        {
           animator.SetBool("IsWalking",true);
        }
        else animator.SetBool("IsWalking",false);
         animator.SetFloat("AxesX",-leftStick.x);
         animator.SetFloat("AxesY",-leftStick.y);
        // Stick droit
       

        // D-pad
        Vector2 dpad = Gamepad.current.dpad.ReadValue();
        if (dpad != Vector2.zero)
        {
            Debug.Log("D-Pad : " + dpad);
        }

        // Boutons principaux
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            animator.SetTrigger("AAction");

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
             animator.SetTrigger("BAction");

        if (Gamepad.current.buttonWest.wasPressedThisFrame)
             animator.SetTrigger("XAction");

        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
             animator.SetTrigger("YAction");

        // Start / Select
        if (Gamepad.current.startButton.wasPressedThisFrame)
            Debug.Log("Start appuyé");

        if (Gamepad.current.selectButton.wasPressedThisFrame)
            Debug.Log("Select / Back appuyé");

        if (Gamepad.current.leftTrigger.ReadValue() > 0.2f)
             animator.SetTrigger("HeatWave");

        if (Gamepad.current.rightTrigger.ReadValue() > 0.2f)
             animator.SetTrigger("Dash");
    }
}
