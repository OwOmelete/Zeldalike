using UnityEngine;
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
	public KeyCode boutonEpauleManette;     // JoystickButton4 (LB) ou JoystickButton5 (RB)

	public Transform conteneurNotes;

	[Header("Seuils de Précision (en Pixels)")]
	public float margePerfect = 20f;
	public float margeGood = 45f;
	public float margeBad = 75f;

	private NoteLongue noteLongueActive;
	private NoteScroller scrollerGlobal;
	private bool gachetteEnfonceeAuFramePrecedent = false;

	void Start()
	{
		laCaseImage = GetComponent<Image>();
		if (laCaseImage != null) laCaseImage.color = couleurNormale;

		scrollerGlobal = FindFirstObjectByType<NoteScroller>();
	}

	void Update()
	{
		bool estAppuyeCeFrame = false;
		bool estEnfonceCeFrame = false;
		bool estRelacheCeFrame = false;

		// 1. GESTION DES GACHETTES ARRIÈRE ANALOGIQUES (LT / RT)
		if (estUneGachetteArriere && !string.IsNullOrEmpty(nomAxeManette))
		{
			float valeurAxe = Input.GetAxisRaw(nomAxeManette);
			bool gachettePressee = valeurAxe > 0.5f;

			if (gachettePressee && !gachetteEnfonceeAuFramePrecedent) estAppuyeCeFrame = true;
			if (gachettePressee) estEnfonceCeFrame = true;
			if (!gachettePressee && gachetteEnfonceeAuFramePrecedent) estRelacheCeFrame = true;

			gachetteEnfonceeAuFramePrecedent = gachettePressee;
		}

		// 2. GESTION DES TOUCHES CLAVIER ET ÉPAULES (LB / RB)
		if (Input.GetKeyDown(toucheAssignee) || Input.GetKeyDown(boutonEpauleManette)) estAppuyeCeFrame = true;
		if (Input.GetKey(toucheAssignee) || Input.GetKey(boutonEpauleManette)) estEnfonceCeFrame = true;
		if (Input.GetKeyUp(toucheAssignee) || Input.GetKeyUp(boutonEpauleManette)) estRelacheCeFrame = true;

		// 🎮 REACTION AUX INPUTS
		if (estAppuyeCeFrame)
		{
			if (laCaseImage != null) laCaseImage.color = couleurAppuye;
			VerifierHit();
		}

		if (estEnfonceCeFrame && noteLongueActive != null)
		{
			if (scrollerGlobal != null)
			{
				noteLongueActive.ReduireBande(scrollerGlobal.vitesseDefilement);
			}
			else
			{
				noteLongueActive.ReduireBande(600f);
			}
		}

		if (estRelacheCeFrame)
		{
			if (Input.GetKey(toucheAssignee) || Input.GetKey(boutonEpauleManette) || (estUneGachetteArriere && Input.GetAxisRaw(nomAxeManette) > 0.5f)) return;

			if (laCaseImage != null) laCaseImage.color = couleurNormale;

			if (noteLongueActive != null)
			{
				Debug.Log("❌ RELÂCHÉ TROP TÔT !");
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