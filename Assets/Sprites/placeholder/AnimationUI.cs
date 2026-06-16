using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class AnimationUI : MonoBehaviour
{
    public Animator Left;
    public Animator Right;
    public bool Switch;
    public List<Material>materialList = new List<Material>();
    public Color32 colorFroid;
    public Color32 colorChaud;
    [ColorUsage(true, true)]
    public Color myHdrColorHot;
    [ColorUsage(true, true)]
    public Color myHdrColorCold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch = false;
        OnRightTrigger();
        foreach(Material m in materialList)
        {
            m.SetColor("_EmissionColor",myHdrColorHot);
            m.SetColor("_BaseColor",colorChaud);
        }
    }
    void Update()
    {
        if(Gamepad.current.leftShoulder.wasPressedThisFrame && Switch) OnLeftTrigger();
        if(Gamepad.current.rightShoulder.wasPressedThisFrame && !Switch) OnRightTrigger();
    }

    private void OnLeftTrigger()
    {
        Left.SetTrigger("Open");
        Right.SetTrigger("Close");
        Switch=false;
        foreach(Material m in materialList)
        {
            m.SetColor("_EmissionColor",myHdrColorCold);
            m.SetColor("_BaseColor",colorFroid);
        }
            
    }

    private void OnRightTrigger()
    {
            Left.SetTrigger("Close");
            Right.SetTrigger("Open");
            Switch=true;
            foreach(Material m in materialList)
        {
            m.SetColor("_EmissionColor",myHdrColorHot);
            m.SetColor("_BaseColor",colorChaud);
        }
    }
    
}
