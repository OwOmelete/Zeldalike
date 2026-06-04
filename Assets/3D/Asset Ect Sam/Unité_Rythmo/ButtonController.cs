using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	private Image laCaseImage;
	public Color couleurNormale;
	public Color couleurAppuye;
	public KeyCode toucheAssignee;
	public Transform conteneurNotes;

	[Header("Seuils de Précision (en Pixels)")]
	public float margePerfect = 20f;
	public float margeGood = 45f;
	public float margeBad = 75f; // Équivalent à ton ancienne distanceTolerance

	private NoteLongue noteLongueActive;

	void Start()
	{
		laCaseImage = GetComponent<Image>();
		if (laCaseImage != null) laCaseImage.color = couleurNormale;
	}

	void Update()
	{
		if (Input.GetKeyDown(toucheAssignee))
		{
			if (laCaseImage != null) laCaseImage.color = couleurAppuye;
			VerifierHit();
		}

		if (Input.GetKey(toucheAssignee) && noteLongueActive != null)
		{
			noteLongueActive.ReduireBande(400f);
		}

		if (Input.GetKeyUp(toucheAssignee))
		{
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

			// On vérifie d'abord si l'objet est bien sur notre couloir X
			if (distanceX < 50f)
			{
				// On applique le barème selon la distance en Y
				if (distanceY <= margeBad)
				{
					string verdict = "BAD";
					if (distanceY <= margePerfect) verdict = "PERFECT";
					else if (distanceY <= margeGood) verdict = "GOOD";

					// Traitement de la note selon sa nature
					NoteLongue scriptNoteLongue = dechet.GetComponent<NoteLongue>();

					if (scriptNoteLongue != null)
					{
						noteLongueActive = scriptNoteLongue;
						noteLongueActive.EnclencherMaintien();
						GameManager.instance.DeclencherJugement(verdict); // Le verdict tombe !
					}
					else
					{
						Destroy(dechet.gameObject);
						GameManager.instance.DeclencherJugement(verdict); // Le verdict tombe !
					}
					break;
				}
			}
		}
	}
}