using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    private GameObject informations;

    public GameObject P_FeunetreIformation;
    public RotateCameraHolder cameraRotate;

    private FamillierData currentFamillier;
    private Slot currentSlot;

    void Awake()
    {
        Slot.inventoryManager = this;
    }

    void Update()
    {
        if (Gamepad.current == null) return;
        if (informations == null || currentFamillier == null) return;

        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            Transform main = informations.transform.Find("Main");

            if (main != null)
            {
                Animator animator = main.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Pet");
                }
            }
        }
    }

    // =========================
    // PET ANIMATION CALLBACK
    // =========================
    public void OnPetAnimationEnd()
    {
        if (currentFamillier == null) return;

        currentFamillier.humeur = Mathf.Clamp(currentFamillier.humeur + 1, 0, 10);

        UpdateHumeurUI(currentFamillier);

        if (currentSlot != null)
            currentSlot.Updatehumeur(currentFamillier);
    }

    // =========================
    // TOOLTIP
    // =========================
    public void ShowToolTip(FamillierData famillier, Slot slot)
    {
        if (famillier == null || P_FeunetreIformation == null)
            return;

        currentFamillier = famillier;
        currentSlot = slot;

        if (informations != null) return;

        informations = Instantiate(P_FeunetreIformation);
        informations.transform.SetParent(SlotManager.Canvas.transform, false);

        cameraRotate?.ResetCamera();

        // NAME
        SetText("FamillierName", famillier.famillierName);
        SetText("Metier", famillier.famillierMetier);

        // ICONS
        SetImageColor("colorPref", famillier.colorPref);
        SetImageSprite("PenseA", famillier.PenseA);
        SetImageSprite("Icon", famillier.icon);

        // MODEL
        RawImage model = FindRawImage("ModelDisplay");
        if (model != null)
            model.texture = famillier.modelRenderTexture;

        SetupAnimationHandler();

        UpdateHumeurUI(famillier);
    }

    public void HideToolTip()
    {
        if (informations == null) return;

        Destroy(informations);
        informations = null;
        currentFamillier = null;
        currentSlot = null;
    }

    // =========================
    // HUMEUR UI
    // =========================
    private void UpdateHumeurUI(FamillierData famillier)
    {
        if (informations == null) return;

        Image img = FindImage("Humeur");
        if (img == null || famillier.humeurSprite == null) return;

        int h = famillier.humeur;

        if (h <= 3) img.sprite = famillier.humeurSprite[0];
        else if (h <= 6) img.sprite = famillier.humeurSprite[1];
        else if (h <= 9) img.sprite = famillier.humeurSprite[2];
        else img.sprite = famillier.humeurSprite[3];
    }

    // =========================
    // HELPERS
    // =========================
    void SetupAnimationHandler()
    {
        Transform main = informations.transform.Find("Main");
        if (main == null) return;

        PetAnimationHandler handler = main.GetComponent<PetAnimationHandler>();
        if (handler != null)
            handler.inventoryManager = this;
    }

    void SetText(string name, string value)
    {
        TMP_Text t = informations.transform.Find(name)?.GetComponent<TMP_Text>();
        if (t != null) t.text = value;
    }

    void SetImageSprite(string name, Sprite s)
    {
        Image i = informations.transform.Find(name)?.GetComponent<Image>();
        if (i != null) i.sprite = s;
    }

    void SetImageColor(string name, Color c)
    {
        Image i = informations.transform.Find(name)?.GetComponent<Image>();
        if (i != null) i.color = c;
    }

    Image FindImage(string name)
    {
        return informations.transform.Find(name)?.GetComponent<Image>();
    }

    RawImage FindRawImage(string name)
    {
        return informations.transform.Find(name)?.GetComponent<RawImage>();
    }
}