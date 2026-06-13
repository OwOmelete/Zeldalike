using UnityEngine;
 
public class SelectionDifficulte : MonoBehaviour
{
	[Header("Panneaux UI")]
	public GameObject panneauMenu;
	public GameObject panneauJeu;
	public GameObject panneauEcranFin;
 
	// Plus besoin de "leScroller" ici, on passe tout par GameManager
 
	public static int ScoreRequisEtoile = 5000;
	public static string ModeChoisi = "EASY";
 
	void Start()
	{
		if (panneauMenu != null) panneauMenu.SetActive(true);
		if (panneauJeu != null) panneauJeu.SetActive(false);
		if (panneauEcranFin != null) panneauEcranFin.SetActive(false);
 
		if (GameManager.instance != null)
		{
			if (GameManager.instance.leScroller != null) GameManager.instance.leScroller.jeuDemarre = false;
			GameManager.instance.jeuACommence = false;
		}
	}
 
	public void ChoisirEasy()
	{
		ScoreRequisEtoile = 5000;
		ModeChoisi = "EASY";
		if (GameManager.instance != null && GameManager.instance.partitionEasy != null)
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionEasy.text;
		DemarrerPartie();
	}
 
	public void ChoisirNormal()
	{
		ScoreRequisEtoile = 10000;
		ModeChoisi = "NORMAL";
		if (GameManager.instance != null && GameManager.instance.partitionNormal != null)
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionNormal.text;
		DemarrerPartie();
	}
 
	public void ChoisirHard()
	{
		ScoreRequisEtoile = 20000;
		ModeChoisi = "HARD";
		if (GameManager.instance != null && GameManager.instance.partitionHard != null)
			GameManager.instance.textePartitionSelectionnee = GameManager.instance.partitionHard.text;
		DemarrerPartie();
	}
 
	private void DemarrerPartie()
	{
		if (GameManager.instance == null) return;
 
		NoteScroller scroller = GameManager.instance.leScroller;
 
		// ⛔ STOP : On gèle tout avant de nettoyer
		if (scroller != null) scroller.jeuDemarre = false;
		GameManager.instance.jeuACommence = false;
 
		// UI
		if (panneauMenu != null) panneauMenu.SetActive(false);
		if (panneauEcranFin != null) panneauEcranFin.SetActive(false);
		if (panneauJeu != null) panneauJeu.SetActive(true);
 
		// 🧹 NETTOYAGE : Détruire toutes les notes et remettre le scroller à zéro
		if (scroller != null)
		{
			Transform conteneur = scroller.transform;
			for (int i = conteneur.childCount - 1; i >= 0; i--)
				Destroy(conteneur.GetChild(i).gameObject);
 
			conteneur.localPosition = Vector3.zero;
		}
 
		// 🔄 Spawner
		NoteSpawner spawner = FindFirstObjectByType<NoteSpawner>();
		if (spawner != null)
		{
			spawner.enabled = true;
			spawner.ChargerPartitionDepuisTexte();
		}
 
		// 🎵 Reset audio
		if (GameManager.instance.laMusique != null)
		{
			GameManager.instance.laMusique.Stop();
			GameManager.instance.laMusique.time = 0f;
			GameManager.instance.laMusique.Play();
		}
 
		// Scores
		GameManager.instance.ReinitialiserPartie();
 
		// ▶️ GO — en dernier
		GameManager.instance.jeuACommence = true;
		if (scroller != null) scroller.jeuDemarre = true;
	}

 
 
	// 🔁 À brancher sur le bouton "Recommencer" du menu pause et de l'écran de fin
	public void Recommencer()
	{
		DemarrerPartie();
	}
}