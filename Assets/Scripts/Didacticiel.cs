using System;
using System.Collections.Generic;
using UnityEngine;

public class Didacticiel : MonoBehaviour
{
    public static Didacticiel Instance { get; private set; }

    [SerializeField]
    private List<GameObject> popupsTuto;
    [SerializeField] GameObject popUpKillEnemy;
    [SerializeField] GameObject meltPopup;
    [SerializeField] GameObject meltTrigger;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        Interactible.OnBreak += DisplayKillEnemyPopup;
        InvoSource.ReleaseInvos += DisableMeltPopup;
    }

    private void OnDisable()
    {
        Interactible.OnBreak -= DisplayKillEnemyPopup;
        InvoSource.ReleaseInvos -= DisableMeltPopup;
    }

    private void Start()
    {
        HideAllPopups();
    }

    public void DisplayKillEnemyPopup(int lol)
    {
        popUpKillEnemy.SetActive(true);
    }

    public void DisableMeltPopup()
    {
        meltPopup.SetActive(false);
        meltTrigger.SetActive(false);
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
        popUpKillEnemy.SetActive(false);
    }
}