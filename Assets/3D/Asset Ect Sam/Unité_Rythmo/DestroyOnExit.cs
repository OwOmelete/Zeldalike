using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
	void Update()
	{
		// SÉCURITÉ : Si ce script a été désactivé par le ButtonController
		// (parce que le joueur est en train de maintenir la note), on ne fait rien !
		if (!this.enabled) return;

		// Si la note descend en dessous de la zone de l'écran (-15f)
		if (transform.position.y < -15f)
		{
			// On signale le raté au GameManager uniquement si le jeu a commencé
			if (GameManager.instance != null && GameManager.instance.jeuACommence)
			{
				GameManager.instance.NoteRatee();
			}

			// On détruit l'objet proprement
			Destroy(gameObject);
		}
	}
}