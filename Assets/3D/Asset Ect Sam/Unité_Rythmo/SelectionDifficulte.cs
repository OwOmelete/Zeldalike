using UnityEngine;

public class SelectionDifficulte : MonoBehaviour
{
	[Header("Panneaux UI")]
	public GameObject panneauMenu;    // Glisse "Menu_Selection" ici
	public GameObject panneauJeu;     // Glisse "Interface_Jeu" ici

	[Header("Références Jeu")]
	public NoteScroller leScroller;   // Glisse ton NoteScroller ici

	// Variables statiques pour l'Écran de fin et l'affichage
	public static int ScoreRequisEtoile = 5000;
	public static string ModeChoisi = "EASY";

	void Start()
	{
		// Au lancement, le menu est actif et l'interface de jeu est masquée
		if (panneauMenu != null) panneauMenu.SetActive(true);
		if (panneauJeu != null) panneauJeu.SetActive(false);

		// On bloque le scroller et les inputs de jeu au démarrage
		if (leScroller != null) leScroller.jeuDemarre = false;
		if (GameManager.instance != null) GameManager.instance.jeuACommence = false;
	}

	public void ChoisirEasy()
	{
		ScoreRequisEtoile = 5000;
		ModeChoisi = "EASY";

		// On injecte directement le texte de la partition Easy dans le GameManager
		if (GameManager.instance != null && GameManager.instance.partitionEasy != null)
		{
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionEasy.text;
		}
		DemarrerPartie();
	}

	public void ChoisirNormal()
	{
		ScoreRequisEtoile = 10000;
		ModeChoisi = "NORMAL";

		// On injecte directement le texte de la partition Normal dans le GameManager
		if (GameManager.instance != null && GameManager.instance.partitionNormal != null)
		{
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionNormal.text;
		}
		DemarrerPartie();
	}

	public void ChoisirHard()
	{
		ScoreRequisEtoile = 20000;
		ModeChoisi = "HARD";

		// On injecte directement le texte de la partition Hard dans le GameManager
		if (GameManager.instance != null && GameManager.instance.partitionHard != null)
		{
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionHard.text;
		}
		DemarrerPartie();
	}

	private void DemarrerPartie()
	{
		// 1. On cache le menu et on affiche l'interface de gameplay
		if (panneauMenu != null) panneauMenu.SetActive(false);
		if (panneauJeu != null) panneauJeu.SetActive(true);

		// 🔄 ON FORCE LE SPAWNER À CHARGER LES NOTES MAINTENANT QUE LE TEXTE EST DISPONIBLE
		NoteSpawner spawner = FindFirstObjectByType<NoteSpawner>();
		if (spawner != null)
		{
			spawner.ChargerPartitionDepuisTexte();
		}

		// 2. On lance la musique et le gameplay de manière synchronisée
		if (GameManager.instance != null)
		{
			GameManager.instance.jeuACommence = true;
			if (GameManager.instance.laMusique != null)
			{
				GameManager.instance.laMusique.Stop(); // Sécurité : on rembobine la musique
				GameManager.instance.laMusique.Play();
			}
		}

		if (leScroller != null)
		{
			leScroller.jeuDemarre = true;
		}
	}
}