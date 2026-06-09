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
	public float margeBad = 75f;

	private NoteLongue noteLongueActive;
	private NoteScroller scrollerGlobal;

	void Start()
	{
		laCaseImage = GetComponent<Image>();
		if (laCaseImage != null) laCaseImage.color = couleurNormale;

		scrollerGlobal = FindFirstObjectByType<NoteScroller>();
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
			if (scrollerGlobal != null)
			{
				// On envoie la vitesse brute (ex: 600f), le deltaTime est géré dans NoteLongue
				noteLongueActive.ReduireBande(scrollerGlobal.vitesseDefilement);
			}
			else
			{
				noteLongueActive.ReduireBande(600f);
			}
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