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
        if(!LeftTrigger)  OnLeftTrigger();
        if(!RightTrigger) OnRightTrigger();
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
