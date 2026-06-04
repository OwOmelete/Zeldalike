using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	[Header("Configuration du Déchet")]
	public GameObject dechetPrefab;

	[System.Serializable]
	public struct NoteData
	{
		public float tempsCode; // Temps en secondes dans la musique (ex: 1.5)
		public int piste;       // Couloir (0, 1, 2 ou 3)
	}

	[Header("LA PARTITION DU JEU")]
	public NoteData[] partition;

	[Header("LES 4 CASES BLANCHES (Glisser Zone_Piste 1 à 4 ici)")]
	public RectTransform[] mesPistesUI;

	private int indexNoteActuelle = 0;
	private float minuteurAudio = 0f;

	void Update()
	{
		// Sécurité : On attend que le jeu et la musique aient démarré
		if (GameManager.instance == null || !GameManager.instance.jeuACommence) return;

		// On suit le temps exact et ultra précis de l'AudioSource
		minuteurAudio = GameManager.instance.laMusique.time;

		// Si on a encore des notes à faire spawner dans la partition
		if (indexNoteActuelle < partition.Length)
		{
			// Dès que la musique atteint le temps de la note
			if (minuteurAudio >= partition[indexNoteActuelle].tempsCode)
			{
				SpawnDechetFidèle(partition[indexNoteActuelle].piste);
				indexNoteActuelle++; // On passe à la note suivante
			}
		}
	}

	void SpawnDechetFidèle(int numPiste)
	{
		if (dechetPrefab == null || mesPistesUI.Length == 0) return;
		if (numPiste < 0 || numPiste >= mesPistesUI.Length) return;

		// 1. On crée le clone du déchet
		GameObject nouveauDechet = Instantiate(dechetPrefab);

		// 2. On le range direct dans le conteneur pour qu'il descende avec le tapis roulant
		nouveauDechet.transform.SetParent(GameManager.instance.leScroller.transform, false);
		nouveauDechet.transform.localScale = Vector3.one;

		RectTransform rectDechet = nouveauDechet.GetComponent<RectTransform>();
		if (rectDechet != null)
		{
			// On centre les ancres pour la UI
			rectDechet.anchorMin = new Vector2(0.5f, 0.5f);
			rectDechet.anchorMax = new Vector2(0.5f, 0.5f);
			rectDechet.pivot = new Vector2(0.5f, 0.5f);

			// 3. On récupère le X exact de la case blanche du bas
			float positionXExacte = mesPistesUI[numPiste].localPosition.x;

			// 4. COMPENSATION : On regarde de combien le tapis est déjà descendu
			float hauteurScroller = GameManager.instance.leScroller.transform.localPosition.y;

			// On fait 450 (le haut de l'écran) MOINS la hauteur du scroller.
			// Comme le scroller descend en négatif, soustraire un nombre négatif va AJOUTER de la hauteur.
			float positionYCalculee = 450f - hauteurScroller;

			// 5. On applique la position finale
			rectDechet.anchoredPosition = new Vector2(positionXExacte, positionYCalculee);
		}
	}
}