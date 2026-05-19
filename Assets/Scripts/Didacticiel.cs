using System;
using System.Collections.Generic;
using UnityEngine;

public class Didacticiel : MonoBehaviour
{
    public static Didacticiel Instance { get; private set; }

    [SerializeField]
    private List<GameObject> popupsTuto;
    [SerializeField] GameObject popUpKillEnemy;
    
    /*Le but est d'activer la popuk KillEnemy quand l'objet interactible est brisé.
     Même système pour le popup de dégelage qu'il faut retirer une fois qu'un glacon a été dégelé.*/
    
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
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        HideAllPopups();
    }

    public void DisplayKillEnemyPopup(int lol)
    {
        popUpKillEnemy.SetActive(true);
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