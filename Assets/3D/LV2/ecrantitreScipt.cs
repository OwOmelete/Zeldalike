using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ecrantitreScipt : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Image glacon;
    public Animator animator;
    public Animator animatorglobal;
    public int etat;
    public int spriteactulle;
    public Image Flame;
    bool isBreaking;
    public GameObject Menu;
    public GameObject rechauffer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        etat=0;
        spriteactulle=0;
    }

    // Update is called once per frame
    void Update()
    {
         if (Gamepad.current.leftTrigger.ReadValue() > 0.2f&& !isBreaking)
        {
            isBreaking=true;
            BreakIce();
        }
        else if(Gamepad.current.leftTrigger.ReadValue() < 0.2f) isBreaking=false;
    }
    void BreakIce()
    {
        if (spriteactulle <= 3)
        {
            
        animator.SetTrigger("break");
        animatorglobal.SetTrigger("break");
        etat++;
        if (etat >= 4)
        {
            etat=0;
            glacon.sprite = sprites[spriteactulle];
            spriteactulle++;
        }
        if (spriteactulle >= 3)
        {
            glacon.enabled=false;
            Flame.enabled=true;
            Menu.SetActive(true);
            rechauffer.SetActive(false);
            spriteactulle++;
        }  
        }
       
    }
}
