using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
	[Header("Panneau UI de Pause")]
	public GameObject panneauPause;

	private bool jeuEstEnPause = false;

	void Update()
	{
		// Déclenche la pause avec la touche Échap (clavier)
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameManager.instance != null && GameManager.instance.jeuACommence)
			{
				if (jeuEstEnPause) ReprendreJeu();
				else MettreEnPause();
			}
		}
	}

	public void ReprendreJeu()
	{
		if (panneauPause != null) panneauPause.SetActive(false);

		Time.timeScale = 1f; // Relance le temps d'Unity
		jeuEstEnPause = false;

		// Relance la musique
		if (GameManager.instance != null && GameManager.instance.laMusique != null)
		{
			GameManager.instance.laMusique.UnPause();
		}
	}

	public void MettreEnPause()
	{
		if (panneauPause != null) panneauPause.SetActive(true);

		Time.timeScale = 0f; // Gèle le temps d'Unity
		jeuEstEnPause = true;

		// Met la musique en pause
		if (GameManager.instance != null && GameManager.instance.laMusique != null)
		{
			GameManager.instance.laMusique.Pause();
		}
	}

	public void RecommencerNiveau()
	{
		Time.timeScale = 1f; // TRÈS IMPORTANT : On remet le temps à正常 avant de recharger !

		// 🎯 CORRECTION : On récupère dynamiquement le nom exact de la scène active pour la recharger de zéro
		string nomSceneActuelle = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(nomSceneActuelle);
	}

	public void RetourMenu()
	{
		Time.timeScale = 1f; // On remet le temps à 1

		// 🎯 SÉCURITÉ : Remplace "MenuSelection" par le nom EXACT de ta scène si elle s'appelle autrement (ex: "Menu", "MainMenu")
		SceneManager.LoadScene("UI_PC_WindowsXP");
	}
}