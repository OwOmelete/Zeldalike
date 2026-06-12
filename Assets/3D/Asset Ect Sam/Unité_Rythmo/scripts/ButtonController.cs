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

	[Header("Effets UI (FX)")]
	[Tooltip("Glisse ton Prefab d'Image UI (avec son Animator) ici")]
	public GameObject fxImpactUiPrefab;

	private NoteLongue noteLongueActive;
	private NoteScroller scrollerGlobal;

	// 🎯 VARIABLES POUR LE FX MAINTIEN
	private GameObject fxActuelInstance;
	private Animator fxAnimator;

	void Start()
	{
		laCaseImage = GetComponent<Image>();
		if (laCaseImage != null) laCaseImage.color = couleurNormale;

		scrollerGlobal = FindFirstObjectByType<NoteScroller>();
	}

	void Update()
	{
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
				break;

			case 1: // LT
				estAppuyeCeFrame = Gamepad.current.leftTrigger.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.leftTrigger.isPressed;
				break;

			case 2: // RB
				estAppuyeCeFrame = Gamepad.current.rightShoulder.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.rightShoulder.isPressed;
				break;

			case 3: // RT
				estAppuyeCeFrame = Gamepad.current.rightTrigger.wasPressedThisFrame;
				estEnfonceCeFrame = Gamepad.current.rightTrigger.isPressed;
				break;
		}

		// On détecte le relâchement dès que la gâchette/bouton n'est plus enfoncé
		if (!estEnfonceCeFrame && laCaseImage.color == couleurAppuye)
		{
			estRelacheCeFrame = true;
		}

		if (estAppuyeCeFrame)
		{
			laCaseImage.color = couleurAppuye;
			VerifierHit();
		}

		// 🔥 LOGIQUE DU MAINTIEN
		if (estEnfonceCeFrame && noteLongueActive != null)
		{
			noteLongueActive.ReduireBande(scrollerGlobal.vitesseDefilement);

			// Si on a un Animator sur le FX, on lui dit de passer sur l'anim de boucle
			if (fxAnimator != null)
			{
				fxAnimator.SetBool("estEnMaintien", true);
			}
		}

		if (estRelacheCeFrame)
		{
			laCaseImage.color = couleurNormale;

			// 🔥 ARRET DU FX : Si on lâche ou qu'on rate, on coupe proprement
			CouperLeFX();

			if (noteLongueActive != null)
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

					NoteLongue scriptNoteLongue = dechet.GetComponent<NoteLongue>();

					// 💥 INSTANTIATION DE L'IMPACT
					if (verdict != "BAD" && fxImpactUiPrefab != null)
					{
						// On nettoie d'abord l'ancien FX s'il y en avait un par sécurité
						CouperLeFX();

						// On crée le nouvel impact sous le bouton
						fxActuelInstance = Instantiate(fxImpactUiPrefab, transform.position, Quaternion.identity, transform);
						fxAnimator = fxActuelInstance.GetComponent<Animator>();

						// Si c'est une note SIMPLE, le FX s'autodétruit après 0.3s
						if (scriptNoteLongue == null)
						{
							Destroy(fxActuelInstance, 0.3f);
						}
					}

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

	// Petite fonction utilitaire pour nettoyer proprement l'effet à l'écran
	void CouperLeFX()
	{
		if (fxAnimator != null)
		{
			fxAnimator.SetBool("estEnMaintien", false);
		}

		if (fxActuelInstance != null)
		{
			// Si c'était une note longue, on détruit directement l'objet quand on lâche
			Destroy(fxActuelInstance);
			fxActuelInstance = null;
			fxAnimator = null;
		}
	}
}