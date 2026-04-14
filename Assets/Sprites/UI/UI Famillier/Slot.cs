using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler
{
    public FamillierData currentfamillier;

    private Image icon;
    private Image fond;
    private Image humeur;

    public static InventoryManager inventoryManager;

    public int id;
    public SlotManager slotManager;

    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    void Awake()
    {
        humeur = transform.GetChild(2).GetComponent<Image>();
        icon = transform.GetChild(1).GetComponent<Image>();
        fond = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetFamillier(FamillierData famillier)
    {
        currentfamillier = famillier;

        if (famillier == null)
        {
            icon.enabled = false;
            humeur.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = famillier.icon;
        famillier.humeur=0;

        Updatehumeur(famillier);
    }

    public void Updatehumeur(FamillierData famillier)
    {
        if (famillier == null || famillier.humeurSprite == null) return;

        int h = famillier.humeur;

        if (h <= 3) humeur.sprite = famillier.humeurSprite[0];
        else if (h <= 6) humeur.sprite = famillier.humeurSprite[1];
        else if (h <= 9) humeur.sprite = famillier.humeurSprite[2];
        else humeur.sprite = famillier.humeurSprite[3];
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (fond != null) fond.color = selectedColor;

        slotManager.OnSlotSelected(id);

        inventoryManager?.ShowToolTip(currentfamillier, this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (fond != null) fond.color = normalColor;

        inventoryManager?.HideToolTip();
    }
}