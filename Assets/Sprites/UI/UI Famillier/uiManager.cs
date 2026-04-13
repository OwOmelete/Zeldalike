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
    public Animator Joystick;

    public float sliderSpeed = 2f;

    private bool actionTriggered = false;

    void Start()
    {
        animators = UiPauseMenu.GetComponentsInChildren<Animator>(true);
        stickImage = SelectStick.GetComponent<Image>();
    }

    void Update()
    {
        if (Gamepad.current == null) return;

        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePauseMenu();
        }

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

    // =========================
    // SLIDERS
    // =========================

    void IncreaseSlider(Slider target)
    {
        // Augmente le bon slider
        target.value += Time.deltaTime * sliderSpeed;

        // Diminue les autres
        if (target != reprendre) reprendre.value -= Time.deltaTime * sliderSpeed;
        if (target != option) option.value -= Time.deltaTime * sliderSpeed;
        if (target != quiter) quiter.value -= Time.deltaTime * sliderSpeed;
        if (target != famillier) famillier.value -= Time.deltaTime * sliderSpeed;

        ClampAll();
    }

    void DecreaseAllSliders()
    {
        reprendre.value -= Time.deltaTime * sliderSpeed;
        option.value -= Time.deltaTime * sliderSpeed;
        quiter.value -= Time.deltaTime * sliderSpeed;
        famillier.value -= Time.deltaTime * sliderSpeed;

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
            Joystick.SetTrigger("Option");
        }
         if (actionName=="Quitter")
        {
            ParentQuiter.SetActive(true);
            Joystick.SetTrigger("Quiter");
        }
         if (actionName=="Famillier")
        {
            ParentFamillier.SetActive(true);
            Joystick.SetTrigger("Famillier");
        }
         if (actionName=="Reprendre")
        {
            Joystick.SetTrigger("Reprendre");
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
            
        Joystick.SetTrigger("Open");
        }
        else
        {
            StartCoroutine(CloseMenu());
        }
    }

    void OpenMenu()
    {
        UiPauseMenu.SetActive(true);

        foreach (Animator anim in animators)
        {
            anim.Play("AnimUiopen");
        }

        ResetAllSliders();
    }

    IEnumerator CloseMenu()
    {
        foreach (Animator anim in animators)
        {
            anim.Play("AnimUiclose");
        }

        yield return new WaitForSeconds(1f);

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