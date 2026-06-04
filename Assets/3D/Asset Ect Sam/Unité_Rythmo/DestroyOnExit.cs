using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
	void Update()
	{
		// On vérifie la vraie position à l'écran (World Space)
		// Si le déchet descend en dessous du bas de l'écran (Y < -10)
		if (transform.position.y < -5f) // Si -5f est encore trop bas, essaie -2f ou -3f
		{
			// On prévient le GameManager que la note est ratée avant de la détruire
			if (GameManager.instance != null)
			{
				GameManager.instance.NoteRatee();
			}

			// On détruit le déchet
			Destroy(gameObject);
		}
	}
}