using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationUI : MonoBehaviour
{
    public Animator Left;
    public Animator Right;
    public bool Switch;
    public InputActionReference LeftTrigger;
    public InputActionReference RightTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch = false;
    }
    void Update()
    {
        if(Gamepad.current.leftTrigger.ReadValue() > 0.2f) OnLeftTrigger();
        if(Gamepad.current.rightTrigger.ReadValue() > 0.2f) OnRightTrigger();
    }

    private void OnLeftTrigger()
    {
        
            Left.SetTrigger("Close");
            Right.SetTrigger("Open");
            Switch=true;
    }

    private void OnRightTrigger()
    {
        Left.SetTrigger("Open");
        Right.SetTrigger("Close");
        Switch=false;
    }
    
}
