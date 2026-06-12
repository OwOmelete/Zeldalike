using UnityEngine;

public class SelectionDifficulte : MonoBehaviour
{
	[Header("Panneaux UI")]
	public GameObject panneauMenu;    // Glisse "Menu_Selection" ici
	public GameObject panneauJeu;     // Glisse "Interface_Jeu" ici
	public GameObject panneauEcranFin; // Glisse "Ecran_Fin" ici

	[Header("Références Jeu")]
	public NoteScroller leScroller;   // Glisse ton NoteScroller ici

	// Variables statiques pour l'Écran de fin et l'affichage
	public static int ScoreRequisEtoile = 5000;
	public static string ModeChoisi = "EASY";

	void Start()
	{
		// Au lancement, le menu est actif, le reste est masqué d'office
		if (panneauMenu != null) panneauMenu.SetActive(true);
		if (panneauJeu != null) panneauJeu.SetActive(false);
		if (panneauEcranFin != null) panneauEcranFin.SetActive(false);

		// On bloque le scroller et les inputs de jeu au démarrage
		if (leScroller != null) leScroller.jeuDemarre = false;
		if (GameManager.instance != null) GameManager.instance.jeuACommence = false;
	}

	public void ChoisirEasy()
	{
		ScoreRequisEtoile = 5000;
		ModeChoisi = "EASY";

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

		if (GameManager.instance != null && GameManager.instance.partitionHard != null)
		{
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionHard.text;
		}
		DemarrerPartie();
	}

	private void DemarrerPartie()
	{
		// 1. On cache le menu et l'écran de fin, on affiche le tapis de jeu
		if (panneauMenu != null) panneauMenu.SetActive(false);
		if (panneauEcranFin != null) panneauEcranFin.SetActive(false);
		if (panneauJeu != null) panneauJeu.SetActive(true);

		// 🔄 SÉCURITÉ : Réactivation et chargement forcé du Spawner
		NoteSpawner spawner = FindFirstObjectByType<NoteSpawner>();
		if (spawner != null)
		{
			spawner.enabled = true;
			spawner.ChargerPartitionDepuisTexte();
		}

		// 2. On réinitialise et lance la synchronisation audio/mécanique
		if (GameManager.instance != null)
		{
			GameManager.instance.ReinitialiserPartie(); // Nettoie le score précédent
			GameManager.instance.jeuACommence = true;

			if (GameManager.instance.laMusique != null)
			{
				GameManager.instance.laMusique.Stop();
				GameManager.instance.laMusique.Play();
			}
		}

		if (leScroller != null)
		{
			leScroller.jeuDemarre = true;
		}
	}
}