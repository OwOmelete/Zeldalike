using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ecrantitreScipt : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Image glacon;
    public Animator animator;
    public Animator animatorglobal;
    public int etat;
    public int spriteactulle;
    public Image Flame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        etat=0;
        spriteactulle=0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            BreakIce();
        }
    }
    void BreakIce()
    {
        etat++;
        animator.SetTrigger("break");
        animatorglobal.SetTrigger("break");
        if (etat >= 4)
        {
            etat=0;
            spriteactulle++;
            glacon.sprite = sprites[spriteactulle];
        }
        if (spriteactulle >= 3)
        {
            glacon.enabled=false;
            Flame.enabled=true;
        }
    }
}
