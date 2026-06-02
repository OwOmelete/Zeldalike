using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationUI : MonoBehaviour
{
    public Animator Left;
    public Animator Right;
    public bool Switch;
    public Material material;
    public Color32 colorFroid;
    public Color32 colorChaud;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch = false;
        material.SetColor("_EmissionColor",colorFroid);
    }
    void Update()
    {
        if(Gamepad.current.leftShoulder.wasPressedThisFrame && !Switch) OnLeftTrigger();
        if(Gamepad.current.rightShoulder.wasPressedThisFrame && Switch) OnRightTrigger();
    }

    private void OnLeftTrigger()
    {

            Left.SetTrigger("Close");
            Right.SetTrigger("Open");
            Switch=true;
            material.SetColor("_EmissionColor",colorChaud);
            
    }

    private void OnRightTrigger()
    {
        Left.SetTrigger("Open");
        Right.SetTrigger("Close");
        Switch=false;
        material.SetColor("_EmissionColor",colorFroid);
    }
    
}
