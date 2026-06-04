using UnityEngine;

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

	void Awake()
	{
		instance = this;

		// SÉCURITÉ 1 : Si on a oublié de glisser l'AudioSource, le script la cherche tout seul sur l'objet
		if (laMusique == null)
		{
			laMusique = GetComponent<AudioSource>();
		}
	}

	void Update()
	{
		if (!jeuACommence)
		{
			// Dès qu'on appuie sur une touche pour lancer le jeu
			if (Input.anyKeyDown)
			{
				// SÉCURITÉ 2 : On vérifie si l'AudioSource a bien une musique dedans
				if (laMusique == null)
				{
					Debug.LogError("🚨 BUG : L'AudioSource est INTROUVABLE sur le GameManager !");
					return;
				}

				if (laMusique.clip == null)
				{
					Debug.LogError("🚨 BUG : Il n'y a AUCUN fichier musique dans la case 'AudioClip' de l'AudioSource !");
					return;
				}

				if (leScroller == null)
				{
					Debug.LogError("🚨 BUG : L'objet 'Conteneur_Notes' (NoteScroller) n'est pas glissé dans le GameManager !");
					return;
				}

				// Si toutes les sécurités passent, on lance le jeu !
				jeuACommence = true;
				leScroller.jeuDemarre = true;
				laMusique.Play();

				Debug.Log("✅ TOUT EST OK : La musique '" + laMusique.clip.name + "' se lance et le scroller démarre !");
			}
		}
	}

	public void NoteTouchee()
	{
		scoreActuel += 100;
		comboActuel++;
		Debug.Log("⭐ Score : " + scoreActuel + " | Combo : " + comboActuel);
	}

	public void NoteRatee()
	{
		comboActuel = 0;
		Debug.Log("❌ RATÉ ! Le combo retombe à 0.");
	}
}