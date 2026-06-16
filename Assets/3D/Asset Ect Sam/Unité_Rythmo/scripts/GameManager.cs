using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[Header("Fichiers de Partitions (Glisser les .txt ici)")]
	public TextAsset partitionEasy;
	public TextAsset partitionNormal;
	public TextAsset partitionHard;

	// Cette variable stockera le texte brut du fichier choisi par le menu
	[HideInInspector] public string textePartitionSelectionnee;

	[Header("Mode Éditeur (Création de Partition)")]
	[Tooltip("Coche cette case pour enregistrer tes pressions de touches dans la console pendant la musique.")]
	public bool modeEditeurActif = false;

	[Header("Scripts et Audio")]
	public AudioSource laMusique;
	public NoteScroller leScroller;
	public EcranFin scriptEcranFin;
	[Tooltip("Nombre de secondes avant la fin de la musique pour afficher l'écran (ex: 3.0 pour couper le fondu de fin)")]
	public float avanceDeclenchementFin = 3.0f; // 🌟 AJOUT : Coupe le morceau plus tôt pendant le fondu

	public bool jeuACommence = false;
	private bool finDePartieDeclenchee = false;

	[Header("Gameplay")]
	public int scoreActuel = 0;
	public int comboActuel = 0;
	public int maxComboAtteint = 0;

	[Header("Statistiques de Fin")]
	public int totalPerfect = 0;
	public int totalGood = 0;
	public int totalBad = 0;
	public int totalMiss = 0;

	[Header("Interface UI (TextMeshPro)")]
	public TextMeshProUGUI affichageScore;
	public TextMeshProUGUI affichageCombo;
	public TextMeshProUGUI affichageJugement;
	public UnityEngine.UI.Slider jaugeScore;

	private float minuteurJugement = 0f;
	private float dureeTotaleDuMorceau = 0f;

	private float[] tempsPressionTouche = new float[5]; // Index 1 à 4 pour les pistes F, G, H, J

	void Awake()
	{
		instance = this;

		if (laMusique == null)
		{
			laMusique = GetComponent<AudioSource>();
		}
	}

	public void OnEnable()
	{
		MettreAJourInterface();
		if (affichageJugement != null) affichageJugement.text = "";

		if (laMusique != null && laMusique.clip != null)
		{
			dureeTotaleDuMorceau = laMusique.clip.length;
		}
	}

	// Fonction de nettoyage appelée par le menu au lancement d'un niveau
	public void ReinitialiserPartie()
	{
		scoreActuel = 0;
		comboActuel = 0;
		maxComboAtteint = 0;
		totalPerfect = 0;
		totalGood = 0;
		totalBad = 0;
		totalMiss = 0;
		finDePartieDeclenchee = false;
		MettreAJourInterface();
	}

	void Update()
	{
		// 🕹️ ENREGISTREMENT : Lancement manuel de la musique pour le mode Éditeur
		if (!jeuACommence && modeEditeurActif)
		{
			if (Input.anyKeyDown)
			{
				if (laMusique != null && laMusique.clip != null)
				{
					jeuACommence = true;
					laMusique.Play();
					Debug.Log("--- DÉBUT DE L'ENREGISTREMENT AUTOMATIQUE ---");
				}
			}
		}

		// 🎹 MODE ÉDITEUR : Détection des touches (Simples & Longues)
		if (modeEditeurActif && jeuACommence)
		{
			float tempsActuel = (laMusique != null && laMusique.isPlaying) ? laMusique.time : Time.time;

			if (Input.GetKeyDown(KeyCode.F)) tempsPressionTouche[1] = tempsActuel;
			if (Input.GetKeyUp(KeyCode.F)) EnregistrerNote(tempsPressionTouche[1], tempsActuel, 1);

			if (Input.GetKeyDown(KeyCode.G)) tempsPressionTouche[2] = tempsActuel;
			if (Input.GetKeyUp(KeyCode.G)) EnregistrerNote(tempsPressionTouche[2], tempsActuel, 2);

			if (Input.GetKeyDown(KeyCode.H)) tempsPressionTouche[3] = tempsActuel;
			if (Input.GetKeyUp(KeyCode.H)) EnregistrerNote(tempsPressionTouche[3], tempsActuel, 3);

			if (Input.GetKeyDown(KeyCode.J)) tempsPressionTouche[4] = tempsActuel;
			if (Input.GetKeyUp(KeyCode.J)) EnregistrerNote(tempsPressionTouche[4], tempsActuel, 4);
		}

		// 🕹️ LOGIQUE DE JEU STANDARD
		if (!modeEditeurActif && jeuACommence)
		{
			float tempsActuelMusique = laMusique.time;

			// SÉCURITÉ : On ne valide la fin du morceau que si l'AudioSource a démarré (tempsActuelMusique > 0.5s)
			if (laMusique.isPlaying && tempsActuelMusique > 0.5f)
			{
				// 🎯 MODIFICATION : On utilise 'avanceDeclenchementFin' pour couper plus tôt avant la fin théorique du fichier
				if (tempsActuelMusique >= (dureeTotaleDuMorceau - avanceDeclenchementFin) && !finDePartieDeclenchee)
				{
					finDePartieDeclenchee = true;
					//jeuACommence = false;

					//if (leScroller != null) leScroller.jeuDemarre = false;

					// Désactivation automatique du spawner pour bloquer les déchets en fin de partie
					NoteSpawner spawner = FindFirstObjectByType<NoteSpawner>();
					if (spawner != null) spawner.enabled = false;

					// Force le GameObject à s'allumer avant d'appeler l'affichage des scores
					if (scriptEcranFin != null)
					{
						scriptEcranFin.gameObject.SetActive(true);
						scriptEcranFin.AfficherLesResultats(scoreActuel, maxComboAtteint, totalPerfect, totalGood, totalBad, totalMiss);
					}
				}
			}

			if (minuteurJugement > 0)
			{
				minuteurJugement -= Time.deltaTime;
				if (minuteurJugement <= 0 && affichageJugement != null)
				{
					affichageJugement.text = "";
				}
			}
		}
	}

	private void EnregistrerNote(float tempsDebut, float tempsFin, int piste)
	{
		float dureeMaintien = tempsFin - tempsDebut;
		if (dureeMaintien > 0.3f)
		{
			Debug.Log(tempsDebut.ToString("F1") + "," + piste + ",1," + dureeMaintien.ToString("F1"));
		}
		else
		{
			Debug.Log(tempsDebut.ToString("F1") + "," + piste + ",0,0");
		}
	}

	public void DeclencherJugement(string type)
	{
		if (affichageJugement == null) return;

		if (type == "PERFECT")
		{
			scoreActuel += 150;
			comboActuel++;
			totalPerfect++;
			affichageJugement.text = "<color=#00FF00>PERFECT !</color>";
		}
		else if (type == "GOOD")
		{
			scoreActuel += 100;
			comboActuel++;
			totalGood++;
			affichageJugement.text = "<color=#FFFF00>GOOD</color>";
		}
		else if (type == "BAD")
		{
			scoreActuel += 50;
			comboActuel = 0;
			totalBad++;
			affichageJugement.text = "<color=#FF00FF>BAD</color>";
		}
		else if (type == "MISS")
		{
			comboActuel = 0;
			totalMiss++;
			affichageJugement.text = "<color=#FF0000>MISS...</color>";
		}

		if (comboActuel > maxComboAtteint) maxComboAtteint = comboActuel;

		minuteurJugement = 0.5f;
		MettreAJourInterface();
	}

	public void NoteTouchee()
	{
		scoreActuel += 100;
		comboActuel++;
		if (comboActuel > maxComboAtteint) maxComboAtteint = comboActuel;
		MettreAJourInterface();
	}

	public void NoteRatee()
	{
		DeclencherJugement("MISS");
	}

	void MettreAJourInterface()
	{
		if (affichageScore != null) affichageScore.text = scoreActuel.ToString();

		if (affichageCombo != null)
		{
			if (comboActuel > 0) affichageCombo.text = comboActuel.ToString();
			else affichageCombo.text = "";
		}

		// 🎯 CALCUL PAR SEGMENTS BASÉ SUR TES VALEURS VISUELLES DE SLIDER
		if (jaugeScore != null)
		{
			float scoreMaxRequis = SelectionDifficulte.ScoreRequisEtoile;

			if (scoreMaxRequis > 0)
			{
				// Paliers de points (40% et 75%)
				float pointsPalier1 = scoreMaxRequis * 0.40f;
				float pointsPalier2 = scoreMaxRequis * 0.75f;

				float valeurSlider = 0f;

				if (scoreActuel <= pointsPalier1)
				{
					// Segment 1 : De 0 à l'Étoile 1 (Slider progresse de 0 à 0.236)
					float pourcentageSegment = (float)scoreActuel / pointsPalier1;
					valeurSlider = pourcentageSegment * 0.236f;
				}
				else if (scoreActuel <= pointsPalier2)
				{
					// Segment 2 : De l'Étoile 1 à l'Étoile 2 (Slider progresse de 0.236 à 0.542)
					float pointsDansCeSegment = scoreActuel - pointsPalier1;
					float tailleDuSegmentPoints = pointsPalier2 - pointsPalier1;
					float pourcentageSegment = pointsDansCeSegment / tailleDuSegmentPoints;

					valeurSlider = 0.236f + (pourcentageSegment * (0.542f - 0.236f));
				}
				else if (scoreActuel <= scoreMaxRequis)
				{
					// Segment 3 : De l'Étoile 2 à l'Étoile 3 (Slider progresse de 0.542 à 0.76)
					float pointsDansCeSegment = scoreActuel - pointsPalier2;
					float tailleDuSegmentPoints = scoreMaxRequis - pointsPalier2;
					float pourcentageSegment = pointsDansCeSegment / tailleDuSegmentPoints;

					valeurSlider = 0.542f + (pourcentageSegment * (0.76f - 0.542f));
				}
				else
				{
					// Au-delà du score max requis (Bonus) : Le Slider continue de monter de 0.76 à 1.0
					float pointsBonus = scoreActuel - scoreMaxRequis;
					float pourcentageBonus = pointsBonus / (scoreMaxRequis * 0.5f);
					valeurSlider = 0.76f + (pourcentageBonus * (1.0f - 0.76f));
				}

				jaugeScore.value = Mathf.Clamp01(valeurSlider);
			}
		}
	}
}