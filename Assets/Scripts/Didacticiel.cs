using System.Collections.Generic;
using UnityEngine;

public class Didacticiel : MonoBehaviour
{
    public static Didacticiel Instance { get; private set; }

    [SerializeField]
    private List<GameObject> popupsTuto;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        HideAllPopups();
    }

    public void DisplayPopup(int index)
    {
        HideAllPopups();

        if (index >= 0 && index < popupsTuto.Count && popupsTuto[index] != null)
        {
            popupsTuto[index].SetActive(true);
        }
    }

    public void HideAllPopups()
    {
        foreach (GameObject go in popupsTuto)
        {
            if (go != null)
            {
                go.SetActive(false);
            }
        }
    }
}