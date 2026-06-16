using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [Header("UI")]
    public GameObject panneauPause;
    public GameObject button;

    [Header("Retour Menu")]
    public GameObject PC;
    public GameObject JeuDeRythme;

    private bool jeuEstEnPause = false;

    void Update()
    {
        bool pausePressed =
            Input.GetKeyDown(KeyCode.Escape) ||
            (Gamepad.current != null &&
             Gamepad.current.startButton.wasPressedThisFrame);

        if (!pausePressed)
            return;

        /*if (GameManager.instance == null ||
            !GameManager.instance.jeuACommence)
            return;*/

        if (jeuEstEnPause)
            ReprendreJeu();
        else
            MettreEnPause();
    }

    public void MettreEnPause()
    {
        jeuEstEnPause = true;

        if (panneauPause != null)
            panneauPause.SetActive(true);

        if (button != null)
            EventSystem.current.SetSelectedGameObject(button);

        Time.timeScale = 0f;

        if (GameManager.instance != null &&
            GameManager.instance.laMusique != null)
        {
            GameManager.instance.laMusique.Pause();
        }
    }

    public void ReprendreJeu()
    {
        jeuEstEnPause = false;

        if (panneauPause != null)
            panneauPause.SetActive(false);

        Time.timeScale = 1f;

        if (GameManager.instance != null &&
            GameManager.instance.laMusique != null)
        {
            GameManager.instance.laMusique.UnPause();
        }
    }

    public void RecommencerNiveau()
    {
        // Ferme le menu pause proprement avant de relancer
        jeuEstEnPause = false;
        Time.timeScale = 1f;

        if (panneauPause != null)
            panneauPause.SetActive(false);

        // Délègue le vrai reset à SelectionDifficulte
        SelectionDifficulte sel = FindFirstObjectByType<SelectionDifficulte>();
        if (sel != null)
            sel.Recommencer();
    }

    public void RetourMenu()
    {
        Time.timeScale = 1f;

        if (GameManager.instance != null &&
            GameManager.instance.laMusique != null)
        {
            GameManager.instance.laMusique.Stop();
        }

        jeuEstEnPause = false;

        if (panneauPause != null)
            panneauPause.SetActive(false);

        if (PC != null)
            PC.SetActive(true);

        if (JeuDeRythme != null)
            JeuDeRythme.SetActive(false);
    }
}