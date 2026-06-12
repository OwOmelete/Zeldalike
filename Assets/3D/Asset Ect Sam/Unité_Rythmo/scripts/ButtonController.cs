using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	private Image laCaseImage;
	public Color couleurNormale;
	public Color couleurAppuye;

	[Header("Contrôles Clavier")]
	public KeyCode toucheAssignee;          // F, G, H, J

	[Header("Contrôles Manette Xbox")]
	[Tooltip("Coche cette case si ce bouton utilise une gâchette arrière (LT ou RT)")]
	public bool estUneGachetteArriere = false;

	[Tooltip("Nom de l'axe Unity (ex: 'GachetteGauche' ou 'GachetteDroite')")]
	public string nomAxeManette;

	[Tooltip("Pour les boutons d'épaules classiques (LB ou RB)")]
	public int piste;   // JoystickButton4 (LB) ou JoystickButton5 (RB)

	public Transform conteneurNotes;

	[Header("Seuils de Précision (en Pixels)")]
	public float margePerfect = 35f;
	public float margeGood = 65f;
	public float margeBad = 95f;

	[Header("Effets de Particules (FX)")]
	[Tooltip("Le FX d'étincelles flash qui pop à chaque appui réussi")]
	public ParticleSystem fxImpactPrefab;
	[Tooltip("Le FX en boucle qui reste allumé pendant le maintien d'une note longue")]
	public ParticleSystem fxMaintienPrefab;

	private NoteLongue noteLongueActive;
	private NoteScroller scrollerGlobal;
	private ParticleSystem fxMaintienInstance; // Stocke l'effet en cours pour l'éteindre au relâchement

	void Start()
	{
		laCaseImage = GetComponent<Image>();
		if (laCaseImage != null) laCaseImage.color = couleurNormale;

		scrollerGlobal = FindFirstObjectByType<NoteScroller>();

		// On pré-installe le FX de maintien sous la touche pour qu'il soit prêt à cracher du feu
		if (fxMaintienPrefab != null)
		{
			fxMaintienInstance = Instantiate(fxMaintienPrefab, transform.position, Quaternion.identity, transform);
			fxMaintienInstance.Stop(); // Éteint par défaut
		}
	}

	void Update()
	{
		// SÉCURITÉ PAUSE : Bloque complètement les inputs si le jeu est mis en pause
		if (Time.timeScale == 0f) return;

		bool estAppuyeCeFrame = false;
		bool estEnfonceCeFrame = false;
		bool estRelacheCeFrame = false;

		if (Gamepad.current == null)
			return;

		switch (piste)
		{
			case 0: // LB
				estAppuyeCeFrame = Gamepad.current.leftShoulder.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.leftShoulder.isPressed;
				estRelacheCeFrame = Gamepad.current.leftShoulder.wasReleasedThisFrame;
				break;

			case 1: // LT
				estAppuyeCeFrame = Gamepad.current.leftTrigger.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.leftTrigger.isPressed;
				estRelacheCeFrame = Gamepad.current.leftTrigger.wasReleasedThisFrame;
				break;

			case 2: // RB
				estAppuyeCeFrame = Gamepad.current.rightShoulder.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.rightShoulder.isPressed;
				estRelacheCeFrame = Gamepad.current.rightShoulder.wasReleasedThisFrame;
				break;

			case 3: // RT
				estAppuyeCeFrame = Gamepad.current.rightTrigger.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.rightTrigger.isPressed;
				estRelacheCeFrame = Gamepad.current.rightTrigger.wasReleasedThisFrame;
				break;
		}

		// Appui initial
		if (estAppuyeCeFrame)
		{
			laCaseImage.color = couleurAppuye;
			VerifierHit();
		}

		// Maintien note longue
		if (estEnfonceCeFrame && noteLongueActive != null)
		{
			noteLongueActive.ReduireBande(scrollerGlobal.vitesseDefilement);

			// 🔥 Allume les particules de maintien si elles sont éteintes
			if (fxMaintienInstance != null && !fxMaintienInstance.isPlaying)
			{
				fxMaintienInstance.Play();
			}
		}

		// Relâchement ou fin de note longue
		if (estRelacheCeFrame || (noteLongueActive == null && fxMaintienInstance != null && fxMaintienInstance.isPlaying))
		{
			laCaseImage.color = couleurNormale;

			// 🔥 Éteint les particules de maintien dès qu'on lâche
			if (fxMaintienInstance != null)
			{
				fxMaintienInstance.Stop();
			}

			if (estRelacheCeFrame && noteLongueActive != null)
			{
				GameManager.instance.DeclencherJugement("MISS");
				Destroy(noteLongueActive.gameObject);
				noteLongueActive = null;
			}
		}
	}

	void VerifierHit()
	{
		foreach (Transform dechet in conteneurNotes)
		{
			float distanceY = Mathf.Abs(transform.position.y - dechet.position.y);
			float distanceX = Mathf.Abs(transform.position.x - dechet.position.x);

			if (distanceX < 50f)
			{
				if (distanceY <= margeBad)
				{
					string verdict = "BAD";
					if (distanceY <= margePerfect) verdict = "PERFECT";
					else if (distanceY <= margeGood) verdict = "GOOD";

					// 💥 Déclenche le FX d'impact (uniquement si on ne rate pas complètement)
					if (verdict != "BAD" && fxImpactPrefab != null)
					{
						ParticleSystem impact = Instantiate(fxImpactPrefab, transform.position, Quaternion.identity);
						Destroy(impact.gameObject, 1f); // Nettoie la hiérarchie après 1 seconde
					}

					NoteLongue scriptNoteLongue = dechet.GetComponent<NoteLongue>();

					if (scriptNoteLongue != null)
					{
						noteLongueActive = scriptNoteLongue;
						noteLongueActive.EnclencherMaintien();
						GameManager.instance.DeclencherJugement(verdict);
					}
					else
					{
						Destroy(dechet.gameObject);
						GameManager.instance.DeclencherJugement(verdict);
					}
					break;
				}
			}
		}
	}
}