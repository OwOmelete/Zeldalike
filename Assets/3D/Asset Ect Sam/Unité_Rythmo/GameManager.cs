using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[Header("Scripts et Audio")]
	public AudioSource laMusique;
	public NoteScroller leScroller;

	public bool jeuACommence = false;

	[Header("Gameplay")]
	public int scoreActuel = 0;
	public int comboActuel = 0;

	[Header("Interface UI (TextMeshPro)")]
	public TextMeshProUGUI affichageScore;
	public TextMeshProUGUI affichageCombo;
	public TextMeshProUGUI affichageJugement; // 💾 NOUVELLE CASE !

	// Miniteur pour faire disparaître le texte de jugement après un court instant
	private float minuteurJugement = 0f;

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
			// Système de disparition du texte "PERFECT/GOOD" après 0.5 seconde
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

	// Gestion des différents types de réussites
	public void DeclencherJugement(string type)
	{
		if (affichageJugement == null) return;

		if (type == "PERFECT")
		{
			scoreActuel += 150; // Plus de points !
			comboActuel++;
			affichageJugement.text = "<color=#00FF00>PERFECT !</color>"; // Vert
		}
		else if (type == "GOOD")
		{
			scoreActuel += 100;
			comboActuel++;
			affichageJugement.text = "<color=#FFFF00>GOOD</color>"; // Jaune
		}
		else if (type == "BAD")
		{
			scoreActuel += 50;
			comboActuel = 0; // Le combo se brise sur un Bad !
			affichageJugement.text = "<color=#FF00FF>BAD</color>"; // Violet
		}
		else if (type == "MISS")
		{
			comboActuel = 0;
			affichageJugement.text = "<color=#FF0000>MISS...</color>"; // Rouge
		}

		minuteurJugement = 0.5f; // Le texte reste visible une demi-seconde
		MettreAJourInterface();
	}

	// Gardien du score classique (utilisé pour la fin des notes longues)
	public void NoteTouchee()
	{
		scoreActuel += 100;
		comboActuel++;
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