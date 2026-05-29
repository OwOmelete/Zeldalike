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

     private void OnLeftTrigger()
    {
            Left.SetTrigger("Close");
            Right.SetTrigger("Open");
            Switch=true;
    }

    private void OnLeftShoulder()
    {
        Left.SetTrigger("Open");
        Right.SetTrigger("Close");
        Switch=false;
    }
    
}
