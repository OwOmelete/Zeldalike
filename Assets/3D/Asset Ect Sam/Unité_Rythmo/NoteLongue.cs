using UnityEngine;

public class NoteLongue : MonoBehaviour
{
	[Header("Visuel de la Jauge")]
	public RectTransform laBandeVerte;

	[Header("Visuel du Bout de la note (Optionnel)")]
	public RectTransform leBoutDeLaNote;

	private bool estMaintenue = false;

	void Update()
	{
		// SI LA NOTE EST MAINTENUE : On la fige sur place en compensant le mouvement global du scroller
		if (estMaintenue)
		{
			if (GameManager.instance != null && GameManager.instance.leScroller != null)
			{
				float vitesseScroller = GameManager.instance.leScroller.vitesseDefilement;
				transform.position += new Vector3(0f, vitesseScroller * Time.deltaTime, 0f);
			}
			return;
		}

		// SI ELLE N'EST PAS TOUCHÉE : Détection de la dead zone classique
		if (transform.position.y < -15f)
		{
			if (GameManager.instance != null && GameManager.instance.jeuACommence)
			{
				GameManager.instance.NoteRatee();
			}
			Destroy(gameObject);
		}
	}

	public void EnclencherMaintien()
	{
		estMaintenue = true;
	}

	public void ReduireBande(float vitesse)
	{
		if (laBandeVerte != null)
		{
			float nouvelleHauteur = laBandeVerte.sizeDelta.y - (Time.deltaTime * vitesse);

			if (nouvelleHauteur <= 0)
			{
				Debug.Log("✨ MAINTIEN PARFAIT !");
				GameManager.instance.NoteTouchee();
				Destroy(gameObject);
			}
			else
			{
				laBandeVerte.sizeDelta = new Vector2(laBandeVerte.sizeDelta.x, nouvelleHauteur);

				if (leBoutDeLaNote != null)
				{
					leBoutDeLaNote.anchoredPosition = new Vector2(0f, nouvelleHauteur);
				}
			}
		}
	}
}