using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class uiManager : MonoBehaviour
{
    public GameObject UiPauseMenu;
    public RectTransform SelectStick;
    public float amplitude = 40f;

    private Animator[] animators;
    public Sprite[] StickSprite;

    private Image stickImage;

    public Slider reprendre;
    public Slider option;
    public Slider quiter;
    public Slider famillier;
    public GameObject ParentFamillier;
    public GameObject ParentOption;
    public GameObject ParentQuiter;
    public GameObject ParentPause;
    public Animator Joystick;
    public Animator fond;
    public NavigationMenuStart navigationMenuStart;

    public float sliderSpeed = 2f;

    private bool actionTriggered = false;

    public static uiManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        DontDestroyOnLoad(transform.parent);
    }

    void Start()
    {
        animators = UiPauseMenu.GetComponentsInChildren<Animator>(true);
        stickImage = SelectStick.GetComponent<Image>();
    }

    void Update()
    {
        if (ParentFamillier.activeSelf || ParentOption.activeSelf || ParentQuiter.activeSelf || ParentPause.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
      
             if (Gamepad.current == null) return;

        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePauseMenuwithStart();
        }
  if (UiPauseMenu.activeSelf == true)
        {
        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();

        if (leftStick.magnitude < 0.1f)
        {
            SelectStick.anchoredPosition = Vector2.zero;

            if (StickSprite.Length > 4)
                stickImage.sprite = StickSprite[4];

            DecreaseAllSliders();
            actionTriggered = false;
            return;
        }

        SelectStick.anchoredPosition = leftStick * amplitude;

        if (leftStick.x > 0.8f)
        {
            stickImage.sprite = StickSprite[0];
            IncreaseSlider(option);
        }
        else if (leftStick.x < -0.8f)
        {
            stickImage.sprite = StickSprite[1];
            IncreaseSlider(famillier);
        }
        else if (leftStick.y > 0.8f)
        {
            stickImage.sprite = StickSprite[2];
            IncreaseSlider(reprendre);
        }
        else if (leftStick.y < -0.8f)
        {
            stickImage.sprite = StickSprite[3];
            IncreaseSlider(quiter);
        }
        else
        {
            stickImage.sprite = StickSprite[4];
            DecreaseAllSliders();
        }

        CheckSliderAction();
    }
        }
       

    // =========================
    // SLIDERS
    // =========================

    void IncreaseSlider(Slider target)
    {
        // Augmente le bon slider
        target.value += Time.unscaledDeltaTime * sliderSpeed;

        // Diminue les autres
        if (target != reprendre) reprendre.value -= Time.unscaledDeltaTime * sliderSpeed;
        if (target != option) option.value -= Time.unscaledDeltaTime * sliderSpeed;
        if (target != quiter) quiter.value -= Time.unscaledDeltaTime * sliderSpeed;
        if (target != famillier) famillier.value -= Time.unscaledDeltaTime * sliderSpeed;

        ClampAll();
    }

    void DecreaseAllSliders()
    {
        reprendre.value -= Time.unscaledDeltaTime * sliderSpeed;
        option.value -= Time.unscaledDeltaTime * sliderSpeed;
        quiter.value -= Time.unscaledDeltaTime * sliderSpeed;
        famillier.value -= Time.unscaledDeltaTime * sliderSpeed;

        ClampAll();
    }

    void ClampAll()
    {
        reprendre.value = Mathf.Clamp01(reprendre.value);
        option.value = Mathf.Clamp01(option.value);
        quiter.value = Mathf.Clamp01(quiter.value);
        famillier.value = Mathf.Clamp01(famillier.value);
    }

    // =========================
    // ACTIONS
    // =========================

    void CheckSliderAction()
    {
        if (actionTriggered) return;

        if (reprendre.value >= 1f)
        {
            actionTriggered = true;
            Debug.Log("Reprendre !");
            StartCoroutine(ExecuteAction("Reprendre"));
        }
        else if (option.value >= 1f)
        {
            actionTriggered = true;
            Debug.Log("Options !");
            StartCoroutine(ExecuteAction("Options"));
        }
        else if (quiter.value >= 1f)
        {
            actionTriggered = true;
            Debug.Log("Quitter !");
            StartCoroutine(ExecuteAction("Quitter"));
        }
        else if (famillier.value >= 1f)
        {
            actionTriggered = true;
            Debug.Log("Famillier !");
            StartCoroutine(ExecuteAction("Famillier"));
        }
    }

    IEnumerator ExecuteAction(string actionName)
    {
        Debug.Log("Open " + actionName + " Menu");
        if (actionName=="Options")
        {
            ParentOption.SetActive(true);
            Joystick.Play("JoystickOption");
        }
         if (actionName=="Quitter")
        {
            ParentQuiter.SetActive(true);
            navigationMenuStart.OnSetActive();
            Joystick.Play("JoystickQuiter");
        }
         if (actionName=="Famillier")
        {
            ParentFamillier.SetActive(true);
            Joystick.Play("JoystickFamillier");
        }
         if (actionName=="Reprendre")
        {
            Joystick.Play("JoystickReprendre");
        }
        yield return CloseMenu();

        
    }

    // =========================
    // MENU
    // =========================

    public void TogglePauseMenu()
    {
        if (!UiPauseMenu.activeSelf)
        {
            OpenMenu();
            stickImage.enabled = true;
            fond.SetTrigger("Open");
            Joystick.SetTrigger("Open");
            
        }
        else
        {
            StartCoroutine(CloseMenu());
        }
    }
   
    void OpenMenu()
    {
        Time.timeScale = 0;
        
        UiPauseMenu.SetActive(true);
        

        foreach (Animator anim in animators)
        {
            anim.Play("AnimUiopen");
        }

        ResetAllSliders();
    }
    
    public void TogglePauseMenuwithStart()
    {
        if (!UiPauseMenu.activeSelf)
        {
            OpenMenu();
            Joystick.Play("JoystickReprendre 0");
            
            stickImage.enabled = true;
            fond.SetTrigger("Open");
            
        }
        else
        {
            StartCoroutine(CloseMenu());
        }
    }

 

    IEnumerator CloseMenu()
    {
        fond.SetTrigger("Close");
        foreach (Animator anim in animators)
        {
            anim.Play("AnimUiclose");
        }

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1;

        UiPauseMenu.SetActive(false);
        stickImage.enabled = false;
    }

    void ResetAllSliders()
    {
        reprendre.value = 0;
        option.value = 0;
        quiter.value = 0;
        famillier.value = 0;

        actionTriggered = false;
    }
}