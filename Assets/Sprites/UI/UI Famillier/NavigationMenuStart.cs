using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NavigationMenuStart : MonoBehaviour
{
    public List<GameObject> boutons = new List<GameObject>();

    void Start()
    {
        SetupNavigation();

        // 🔥 sélectionne le premier bouton au start
        if (boutons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(boutons[0]);
        }
    }
    public void OnSetActive()
    {
        EventSystem.current.SetSelectedGameObject(boutons[0]);
    }

    void SetupNavigation()
    {
        for (int i = 0; i < boutons.Count; i++)
        {
            Button b = boutons[i].GetComponent<Button>();
            if (b == null) continue;

            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;

            // 🔼 Bouton du haut
            if (i > 0)
                nav.selectOnUp = boutons[i - 1].GetComponent<Button>();

            // 🔽 Bouton du bas
            if (i < boutons.Count - 1)
                nav.selectOnDown = boutons[i + 1].GetComponent<Button>();

            b.navigation = nav;
        }
    }
}