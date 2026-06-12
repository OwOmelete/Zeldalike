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
		if (estMaintenue)
		{
			if (GameManager.instance != null && GameManager.instance.leScroller != null)
			{
				float vitesseScroller = GameManager.instance.leScroller.vitesseDefilement;
				transform.position += new Vector3(0f, vitesseScroller * Time.deltaTime, 0f);
				if (gameObject.GetComponent<noteData>() != null)
				{
					gameObject.GetComponent<noteData>().speedLateral = new Vector3(0, 0, 0);
				}
			}
			return;
		}

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
			// Le calcul est maintenant parfaitement calé sur le temps et la vitesse du scroller
			float nouvelleHauteur = laBandeVerte.sizeDelta.y - (vitesse * Time.deltaTime);

			if (nouvelleHauteur <= 0)
			{
				Debug.Log("✨ MAINTIEN PARFAIT !");
				GameManager.instance.NoteTouchee();
				Destroy(gameObject);
			}
			else
			{
				// On modifie SEULEMENT la hauteur de la bande.
				laBandeVerte.sizeDelta = new Vector2(laBandeVerte.sizeDelta.x, nouvelleHauteur);

				// 🎯 Le bout (la queue) étant enfant de la bande avec des ancres en haut (1,1),
				// il va suivre le mouvement descendat automatiquement sans qu'on ait besoin d'y toucher ici !
			}
		}
	}
}