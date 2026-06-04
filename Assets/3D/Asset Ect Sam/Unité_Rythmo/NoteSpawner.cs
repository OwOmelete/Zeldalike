using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	[Header("Les Prefabs de Déchets")]
	public GameObject dechetNormalPrefab;
	public GameObject dechetLongPrefab; // 💾 TA NOUVELLE CASE !

	[System.Serializable]
	public struct NoteData
	{
		public float tempsCode;
		public int piste;
		public bool estUneNoteLongue; // Plus simple : vrai ou faux !
	}

	[Header("LA PARTITION DU JEU")]
	public NoteData[] partition;

	[Header("LES 4 CASES BLANCHES")]
	public RectTransform[] mesPistesUI;

	private int indexNoteActuelle = 0;
	private float minuteurAudio = 0f;

	void Update()
	{
		if (GameManager.instance == null || !GameManager.instance.jeuACommence) return;

		minuteurAudio = GameManager.instance.laMusique.time;

		if (indexNoteActuelle < partition.Length)
		{
			if (minuteurAudio >= partition[indexNoteActuelle].tempsCode)
			{
				SpawnDechet(partition[indexNoteActuelle].piste, partition[indexNoteActuelle].estUneNoteLongue);
				indexNoteActuelle++;
			}
		}
	}

	void SpawnDechet(int numPiste, bool longue)
	{
		if (mesPistesUI.Length == 0 || numPiste < 0 || numPiste >= mesPistesUI.Length) return;

		// On choisit le bon prefab selon la partition
		GameObject prefabAUtiliser = longue ? dechetLongPrefab : dechetNormalPrefab;
		if (prefabAUtiliser == null) return;

		GameObject nouveauDechet = Instantiate(prefabAUtiliser);
		nouveauDechet.transform.SetParent(GameManager.instance.leScroller.transform, false);
		nouveauDechet.transform.localScale = Vector3.one;

		RectTransform rectDechet = nouveauDechet.GetComponent<RectTransform>();
		if (rectDechet != null)
		{
			rectDechet.anchorMin = new Vector2(0.5f, 0.5f);
			rectDechet.anchorMax = new Vector2(0.5f, 0.5f);
			rectDechet.pivot = new Vector2(0.5f, 0.5f);

			float positionXExacte = mesPistesUI[numPiste].localPosition.x;
			float hauteurScroller = GameManager.instance.leScroller.transform.localPosition.y;
			float positionYCalculee = 450f - hauteurScroller;

			rectDechet.anchoredPosition = new Vector2(positionXExacte, positionYCalculee);
		}
	}
}