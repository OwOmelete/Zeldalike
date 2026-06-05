using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[Header("Scripts et Audio")]
	public AudioSource laMusique;
	public NoteScroller leScroller;
	public EcranFin scriptEcranFin;

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

	private float minuteurJugement = 0f;
	private float dureeTotaleDuMorceau = 0f; // ⏱️ Calculé automatiquement maintenant !

	void Awake()
	{
		instance = this;

		if (laMusique == null)
		{
			laMusique = GetComponent<AudioSource>();
		}
	}

	void Start()
	{
		MettreAJourInterface();
		if (affichageJugement != null) affichageJugement.text = "";

		// 🎵 CALCULE LA DURÉE AUTOMATIQUEMENT SELON LE MP3 CHARGÉ
		if (laMusique != null && laMusique.clip != null)
		{
			dureeTotaleDuMorceau = laMusique.clip.length;
		}
	}

	void Update()
	{
		if (!jeuACommence)
		{
			if (Input.anyKeyDown)
			{
				if (laMusique == null || laMusique.clip == null || leScroller == null)
				{
					return;
				}

				jeuACommence = true;
				leScroller.jeuDemarre = true;
				laMusique.Play();
			}
		}
		else
		{
			// 🏁 FIN AUTOMATIQUE ET DYNAMIQUE
			// Si le temps de la musique dépasse sa durée totale (moins une micro-marge de sécurité de 0.2s)
			// ou si l'audio ne joue plus, on déclenche l'écran de fin.
			float tempsActuelMusique = laMusique.time;

			if ((tempsActuelMusique >= dureeTotaleDuMorceau - 0.2f || !laMusique.isPlaying) && !finDePartieDeclenchee)
			{
				finDePartieDeclenchee = true;
				jeuACommence = false;

				if (leScroller != null) leScroller.jeuDemarre = false;

				if (scriptEcranFin != null)
				{
					scriptEcranFin.AfficherLesResultats(scoreActuel, maxComboAtteint, totalPerfect, totalGood, totalBad, totalMiss);
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
		if (affichageScore != null) affichageScore.text = "SCORE: " + scoreActuel.ToString();

		if (affichageCombo != null)
		{
			if (comboActuel > 0) affichageCombo.text = "COMBO x" + comboActuel.ToString();
			else affichageCombo.text = "";
		}
	}
}