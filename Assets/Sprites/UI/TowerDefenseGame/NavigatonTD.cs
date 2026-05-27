using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigatonTD : MonoBehaviour
{
     public List<GameObject> boutons = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       SetupNavigation();
        if (boutons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(boutons[0]);
        } 
    }

    void SetupNavigation()
    {
        for (int i = 0; i < boutons.Count; i++)
        {
            Button b = boutons[i].GetComponent<Button>();
            if (b == null) continue;

            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Automatic;

            
            if (i > 0)
                nav.selectOnUp = boutons[i - 1].GetComponent<Button>();
                

            if (i < boutons.Count - 1)
                nav.selectOnDown = boutons[i + 1].GetComponent<Button>();
            b.navigation = nav;
        }
    }
}
