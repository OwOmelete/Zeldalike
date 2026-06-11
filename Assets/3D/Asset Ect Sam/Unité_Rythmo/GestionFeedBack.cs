using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionFeedBack : MonoBehaviour
{
    public List<GameObject> input = new();
   
    void Update()
    {
        if (Gamepad.current.leftShoulder.ReadValue() > 0.2f)
        {
            input[0].SetActive(true);
        }
        else input[0].SetActive(false);
        
        
        if (Gamepad.current.leftTrigger.isPressed)
        {
            input[1].SetActive(true);
        }
        else input[1].SetActive(false);


        if (Gamepad.current.rightShoulder.ReadValue() > 0.2f)
        {
            input[2].SetActive(true);
        }
        else input[2].SetActive(false);


        if (Gamepad.current.rightTrigger.isPressed)
        {
            input[3].SetActive(true);
        }
        else input[3].SetActive(false);
    
    }
}
