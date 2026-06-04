using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NavigationTD : MonoBehaviour
{
    [Header("Boutons")]
    public List<GameObject> boutons = new List<GameObject>();

    [Header("Sélecteur de tour")]
    public GameObject chooseTower;
    public List<Image> cibles = new List<Image>();
    public List<Sprite> spriteSelectionne = new List<Sprite>();
    public List<Sprite> spriteNotSelectionne = new List<Sprite>();

    [Header("Tour Prefab")]
    public int tourBasePrice;
    public List<GameObject> tourBase = new List<GameObject>();
    public int TourInvokFamillierBasePrice;
    public List<GameObject> TourInvokFamillier = new List<GameObject>();
    public int TourGlacePrice;
    public List<GameObject> TourGlace = new List<GameObject>();
    public int TourFeuPrice;
    public List<GameObject> TourFeu = new List<GameObject>();
    public int AneantisseurPrice;
    public List<GameObject> Aneantisseur = new List<GameObject>();

    [Header("Curseur")]
    public GameObject curseur;
    [Header("Reference")]
    public GameManagerTD gameManagerTD;

    Vector2 curseurInitialPos;
    Vector2 currentButtonPosition;
    int currentSecteur = -1;
    int tourBaseInt;
    int TourGlaceInt;
    int TourFeuInt;
    int AneantisseurInt;
    int TourInvokFamillierInt;
    bool isChoosing;
    bool asChoose;
    bool asSelected;

    void Start()
    {
        SetNavigation(Navigation.Mode.Automatic);
        if (boutons.Count > 0)
            EventSystem.current.SetSelectedGameObject(boutons[0]);
    }

    void SetNavigation(Navigation.Mode mode)
    {
        foreach (GameObject go in boutons)
        {
            Button b = go.GetComponent<Button>();
            if (b == null) continue;
            b.navigation = new Navigation { mode = mode };
        }
    }

    public void ChooseTower(Transform other)
    {
        if (isChoosing) return;

        chooseTower.SetActive(true);
        currentButtonPosition = other.position;
        chooseTower.transform.position = other.position;
        curseurInitialPos = curseur.transform.position;
        asChoose = false;

        SetNavigation(Navigation.Mode.None);
        StartCoroutine(WaitForChoosing());
    }

    IEnumerator WaitForChoosing()
    {
        isChoosing = true;
       

        while (!asChoose)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                ExitChoosing();
                break;
            }

            UpdateCurseur();
            UpdateSelection();
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)CheckConfirmation();
            

            yield return null;
        }

        SetNavigation(Navigation.Mode.Automatic);
    }

    void ExitChoosing()
    {
        asChoose = true;
        isChoosing = false;
        chooseTower.SetActive(false);
        curseur.transform.position = curseurInitialPos;
        ResetAllSprites();
    }

    void UpdateCurseur()
    {
        Vector2 stick = Gamepad.current.leftStick.ReadValue();
        curseur.transform.position = curseurInitialPos + stick * 100f;

        float magnitude = stick.magnitude;

        if (magnitude > 0.8f)
            OnStickActive(stick);
        else if (magnitude < 0.4f)
            OnStickNeutral();
    }

    void OnStickActive(Vector2 stick)
    {
        asSelected = true;
        int newSecteur = GetSecteur(stick);

        if (newSecteur != currentSecteur)
        {
            if (currentSecteur >= 0)
                SetCibleSprite(currentSecteur, false);

            currentSecteur = newSecteur;
            SetCibleSprite(currentSecteur, true);
        }
    }

    void OnStickNeutral()
    {
        if (!asSelected) return;
        asSelected = false;
        if (currentSecteur >= 0)
            SetCibleSprite(currentSecteur, false);
    }

    void UpdateSelection()
    {
        
    }

    void CheckConfirmation()
    {
        if (!asSelected) return;

        switch (currentSecteur)
        {
            case 0: 
            gameManagerTD.UpdateEnergie(false,TourInvokFamillierBasePrice); 
            int position = boutons.IndexOf(EventSystem.current.currentSelectedGameObject);
            TourInvokFamillier[position].SetActive(true);
            isChoosing = false;
            ExitChoosing();
            break;

            case 1:
            gameManagerTD.UpdateEnergie(false,tourBasePrice); 
            tourBase[tourBaseInt].SetActive(true);
            tourBase[tourBaseInt].transform.position = currentButtonPosition;
            tourBaseInt++;
            isChoosing = false;
            ExitChoosing();
            break;

            case 2: 
            gameManagerTD.UpdateEnergie(false,TourGlacePrice); 
            TourGlace[TourGlaceInt].SetActive(true);
            TourGlace[TourGlaceInt].transform.position = currentButtonPosition;
            TourGlaceInt++;
            isChoosing = false;
            ExitChoosing();
            break;

            case 3: 
            gameManagerTD.UpdateEnergie(false,TourFeuPrice); 
            TourFeu[TourFeuInt].SetActive(true);
            TourFeu[TourFeuInt].transform.position = currentButtonPosition;
            TourFeuInt++;
            isChoosing = false;
            ExitChoosing();
            break;

            case 4: 
            gameManagerTD.UpdateEnergie(false,AneantisseurPrice); 
            Aneantisseur[AneantisseurInt].SetActive(true);
            Aneantisseur[AneantisseurInt].transform.position = currentButtonPosition;
            AneantisseurInt++;
            isChoosing = false;
            ExitChoosing();
            break;
        }
    }


    int GetSecteur(Vector2 stick)
    {
        float angle = Mathf.Atan2(stick.y, stick.x) * Mathf.Rad2Deg;
        float angleFromTop = (angle - 90f + 360f) % 360f;
        return Mathf.RoundToInt(angleFromTop / 72f) % 5;
    }

    void SetCibleSprite(int index, bool selected)
    {
        cibles[index].sprite = selected
            ? spriteSelectionne[index]
            : spriteNotSelectionne[index];
    }

    void ResetAllSprites()
    {
        for (int i = 0; i < cibles.Count; i++)
            SetCibleSprite(i, false);
        currentSecteur = -1;
        asSelected = false;
    }
}