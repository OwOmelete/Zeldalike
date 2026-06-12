using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionFeedBack : MonoBehaviour
{
    public List<GameObject> input = new();
    public Animator animator;
    bool AAction=false;
    bool XAction=false;
   
    void Update()
    {
        if (Gamepad.current.leftShoulder.ReadValue() > 0.2f)
        {
            input[0].SetActive(true);
            
           if (!AAction)
            {
                AAction =true;
                animator.SetTrigger("A"); 
            }
          
        }
        else
        {
            AAction =false;
            input[0].SetActive(false);
        }
        
      
        
        
        if (Gamepad.current.leftTrigger.isPressed)
        {
            input[1].SetActive(true);
           
        }
        else input[1].SetActive(false);
        if (Gamepad.current.leftTrigger.wasPressedThisFrame) animator.SetTrigger("B");


        if (Gamepad.current.rightShoulder.ReadValue() > 0.2f)
        {
            input[2].SetActive(true);
            if (!XAction)
            {
               animator.SetTrigger("X"); 
               XAction =true;
            }
            
        }
        else
        {
            input[2].SetActive(false);
            XAction =false;
        }
         


        if (Gamepad.current.rightTrigger.isPressed)
        {
            input[3].SetActive(true);
        }
        else input[3].SetActive(false);
        if (Gamepad.current.rightTrigger.wasPressedThisFrame)animator.SetTrigger("Y");
    
    }
}
