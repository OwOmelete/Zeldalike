using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SlotManager : MonoBehaviour
{
    public int ColsCount = 3;
    public int RowsCount = 3;

    public GameObject P_Slot;

    public Vector3 slotPosition;
    public int slotOffset = 100;

    private Vector3 defaultSlotPosition;

    public RectTransform inventory;
    public Vector2 deplacementVertical;

    public static Canvas Canvas;

    public FamillierData[] testFamilliers;

    private List<Slot> slots = new List<Slot>();

    private Vector2 targetPosition;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    void Awake()
    {
        Canvas = GameObject.Find("InventoryCanvas").GetComponent<Canvas>();
    }

    void Start()
    {
        defaultSlotPosition = slotPosition;
        CreateSlots();
        SetupNavigation();
    }

    void Update()
    {
        HandleScroll();

        if (Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            AddRandomFamilliers(1);
        }
    }
    void CreateSlots()
    {
        int count = 0;

        for (int i = 0; i < ColsCount; i++)
        {
            for (int j = 0; j < RowsCount; j++)
            {
                GameObject go = Instantiate(P_Slot, transform);

                Slot slot = go.GetComponent<Slot>();
                slot.id = count + 1;
                slot.slotManager = this;

                slots.Add(slot);

                //if (count < testFamilliers.Length)
                // slot.SetFamillier(testFamilliers[count]);

                //Au chargement je peux charger mes famillier stocker dans mes player pref ici

                count++;

                go.transform.localPosition =
                    new Vector3(slotPosition.x, slotPosition.y, 0);

                slotPosition.x += slotOffset;
            }

            slotPosition.x = defaultSlotPosition.x;
            slotPosition.y -= slotOffset + 30;
        }
    }
    public void AddRandomFamilliers(int amount)
{
    if (testFamilliers == null || testFamilliers.Length == 0)
        return;

    for (int i = 0; i < amount; i++)
    {
        FamillierData random = testFamilliers[Random.Range(0, testFamilliers.Length)];

        AddFamillier(random); 
    }
}

   public void AddFamillier(FamillierData famillier)
{
    FamillierData newFamillier = Instantiate(famillier);
    newFamillier.humeur = 0; 
    //Il faudrait mettre les famillier dans une liste pour enregistrer plus facilement apres

    foreach (Slot s in slots)
    {
        if (s.currentfamillier == null)
        {
            s.SetFamillier(newFamillier);
            return;
        }
    }

    Debug.Log("Inventaire plein !");
}
public FamillierData CreateRuntimeFamillier(FamillierData baseData)
{
    FamillierData instance = Instantiate(baseData);
    instance.humeur = 0;
    return instance;
}
    void HandleScroll()
    {
        if (!isMoving) return;

        inventory.anchoredPosition = Vector2.Lerp(
            inventory.anchoredPosition,
            targetPosition,
            Time.deltaTime * moveSpeed
        );

        if (Vector2.Distance(inventory.anchoredPosition, targetPosition) < 0.1f)
        {
            inventory.anchoredPosition = targetPosition;
            isMoving = false;
        }
    }
    public void OnSlotSelected(int slotId)
    {
        int row = (slotId - 1) / RowsCount;

        targetPosition = new Vector2(
            inventory.anchoredPosition.x,
            row * deplacementVertical.y
        );

        isMoving = true;
    }
    void SetupNavigation()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Button b = slots[i].GetComponent<Button>();
            if (!b) continue;

            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;

            int row = i / RowsCount;
            int col = i % RowsCount;

            if (col > 0) nav.selectOnLeft = slots[i - 1].GetComponent<Button>();
            if (col < RowsCount - 1) nav.selectOnRight = slots[i + 1].GetComponent<Button>();
            if (row > 0) nav.selectOnUp = slots[i - RowsCount].GetComponent<Button>();
            if (row < ColsCount - 1) nav.selectOnDown = slots[i + RowsCount].GetComponent<Button>();

            b.navigation = nav;
        }

        if (slots.Count > 0)
            EventSystem.current.SetSelectedGameObject(slots[0].gameObject);
    }
}