using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using System;
using Unity.Entities.UniversalDelegates;

public class NavigationMenuStart : MonoBehaviour
{
    public List<GameObject> boutons = new List<GameObject>();
    public Animator Enter;
    public Animator Quit;
    public int curentButton;
    public int result;
    public bool canChange;
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();
    public List<Image> images = new List<Image>();
    bool changing;
    public GameObject quitter;
    public GameObject Menu;


    void Start()
    {
        SetupNavigation();
        if (boutons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(boutons[0]);
        }
        curentButton=50;
        result = curentButton % 5;
        canChange=true;
        changing=false;
    }
    
    void Update()
    {
        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
         if (leftStick.y < -0.2f && canChange)
        {
            canChange=false;     
            StartCoroutine(waitforSeconde(1));
            Quit.SetTrigger(1.ToString());
            image.enabled=false;
            curentButton++;
            result = Math.Abs(curentButton % 5);
        }
         if (leftStick.y > 0.2f && canChange)
        {
            canChange=false; 
            StartCoroutine(waitforSeconde(-1));
            Quit.SetTrigger((-1).ToString());
            image.enabled=false;
            curentButton--;
            result = Math.Abs(curentButton % 5);
        }
        else if (leftStick.y < 0.2f && leftStick.y > -0.2f && !changing)
        {
           canChange=true; 
        } 
    }
    IEnumerator waitforSeconde(int i)
    {
        changing=true;
        yield return new WaitForSecondsRealtime(0.25f);
        Enter.SetTrigger(i.ToString());
        yield return new WaitForSecondsRealtime(0.15f);
        image.enabled=true;
        image.sprite = sprites[result];
        images[result].enabled=true;
        EventSystem.current.SetSelectedGameObject(boutons[result]);
        images[ Math.Abs((curentButton+1) % 5)].enabled=false;
        images[Math.Abs((curentButton-1) % 5)].enabled=false;
        
        changing=false;
    }
    void SetupNavigation()
    {
        for (int i = 0; i < boutons.Count; i++)
        {
            Button b = boutons[i].GetComponent<Button>();
            if (b == null) continue;

            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            if (i > 0)
                nav.selectOnUp = boutons[i - 1].GetComponent<Button>();
                
            if (i < boutons.Count - 1)
                nav.selectOnDown = boutons[i + 1].GetComponent<Button>();
                Enter.SetTrigger(i+1);
                Quit.SetTrigger(i+1);
            b.navigation = nav;
        }
    }
    public void Quitter()
    {
        if (Menu.activeSelf)
        {
           quitter.SetActive(false); 
        }
        
    }
}