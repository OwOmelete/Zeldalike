using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationUI : MonoBehaviour
{
    public Animator Left;
    public Animator Right;
    public bool Switch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.rightShoulder.wasPressedThisFrame && Switch==false)
        {
            Left.SetTrigger("Close");
            Right.SetTrigger("Open");
            Switch=true;
        }
         if (Gamepad.current.leftShoulder.wasPressedThisFrame && Switch==true)
        {
            Left.SetTrigger("Open");
            Right.SetTrigger("Close");
             Switch=false;
        }
    }
}
