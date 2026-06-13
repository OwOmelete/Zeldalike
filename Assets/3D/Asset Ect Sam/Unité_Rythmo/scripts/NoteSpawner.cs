using UnityEngine;
using System.Globalization;

public class NoteSpawner : MonoBehaviour
{
	[Header("Les Prefabs de Déchets")]
	public GameObject dechetNormalPrefab;
	public GameObject dechetLongPrefab;

	[Header("LES 4 CASES BLANCHES")]
	public RectTransform[] mesPistesUI;

	[Header("Réglages")]
	public RectTransform conteneurNotes;
	public float hauteurSpawnY = 700f;

	private struct NoteData
	{
		public float tempsFrappeVoulu;
		public int piste;
		public bool estUneNoteLongue;
		public float dureeMaintien;
	}

	private NoteData[] partition;
	private int indexNoteActuelle = 0;
	private float minuteurAudio = 0f;
	private float tempsDeTrajet = 0f;
	private bool tempsTrajetCalcule = false;

	void Start()
	{
		if (GameManager.instance != null && GameManager.instance.leScroller != null)
		{
			GameManager.instance.leScroller.transform.localPosition = Vector3.zero;
		}

		if (conteneurNotes == null && GameManager.instance != null && GameManager.instance.leScroller != null)
		{
			conteneurNotes = GameManager.instance.leScroller.GetComponent<RectTransform>();
		}
	}

	public void ChargerPartitionDepuisTexte()
	{
		if (GameManager.instance == null || string.IsNullOrEmpty(GameManager.instance.textePartitionSelectionnee))
		{
			Debug.LogWarning("⚠️ Aucune partition reçue du GameManager !");
			return;
		}

		string[] lignes = GameManager.instance.textePartitionSelectionnee.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		partition = new NoteData[lignes.Length];

		for (int i = 0; i < lignes.Length; i++)
		{
			string[] elements = lignes[i].Split(',');
			if (elements.Length >= 4)
			{
				NoteData nouvelleNote = new NoteData();
				nouvelleNote.tempsFrappeVoulu = float.Parse(elements[0], CultureInfo.InvariantCulture);
				nouvelleNote.piste = int.Parse(elements[1]);
				nouvelleNote.estUneNoteLongue = elements[2] == "1";
				nouvelleNote.dureeMaintien = float.Parse(elements[3], CultureInfo.InvariantCulture);
				partition[i] = nouvelleNote;
			}
		}

		indexNoteActuelle = 0;

		// ✅ On calcule tempsDeTrajet ici directement, sans attendre le Update()
		float vitesse = GameManager.instance.leScroller.vitesseDefilement;
		if (vitesse > 0)
		{
			tempsDeTrajet = hauteurSpawnY / vitesse;
			tempsTrajetCalcule = true;
		}
		else
		{
			tempsTrajetCalcule = false;
		}
	}

	void Update()
	{
		if (GameManager.instance == null || !GameManager.instance.jeuACommence || partition == null || partition.Length == 0) return;

		// Calcul de secours au cas où ChargerPartitionDepuisTexte() aurait échoué
		if (!tempsTrajetCalcule)
		{
			float vitesse = GameManager.instance.leScroller.vitesseDefilement;
			if (vitesse > 0)
			{
				tempsDeTrajet = hauteurSpawnY / vitesse;
				tempsTrajetCalcule = true;
			}
			else return;
		}

		minuteurAudio = GameManager.instance.laMusique.time;

		// Boucle while : spawn toutes les notes en retard en une seule frame
		while (indexNoteActuelle < partition.Length)
		{
			float tempsApparitionAnticipe = partition[indexNoteActuelle].tempsFrappeVoulu - tempsDeTrajet;

			if (minuteurAudio >= tempsApparitionAnticipe)
			{
				SpawnDechet(partition[indexNoteActuelle]);
				indexNoteActuelle++;
			}
			else break; // La prochaine note n'est pas encore prête, on attend
		}
	}

	void SpawnDechet(NoteData donneesNote)
	{
		int indexPisteCode = donneesNote.piste - 1;
		if (mesPistesUI.Length == 0 || indexPisteCode < 0 || indexPisteCode >= mesPistesUI.Length) return;

		GameObject prefabAUtiliser = donneesNote.estUneNoteLongue ? dechetLongPrefab : dechetNormalPrefab;
		if (prefabAUtiliser == null) return;

		GameObject nouveauDechet = Instantiate(prefabAUtiliser, conteneurNotes);
		nouveauDechet.transform.localScale = Vector3.one;
		RectTransform rectDechet = nouveauDechet.GetComponent<RectTransform>();
		RectTransform pisteCible = mesPistesUI[indexPisteCode];
		
		noteData noteData = nouveauDechet.GetComponent<noteData>();
		noteData.piste = donneesNote.piste;

		if (rectDechet != null)
		{
			rectDechet.anchorMin = new Vector2(0.5f, 0f);
			rectDechet.anchorMax = new Vector2(0.5f, 0f);
			rectDechet.pivot = new Vector2(0.5f, 0.5f);

			float positionX = conteneurNotes.InverseTransformPoint(pisteCible.position).x;
			float positionYCalculee = hauteurSpawnY - conteneurNotes.anchoredPosition.y;

			rectDechet.anchoredPosition = new Vector2(positionX, positionYCalculee);

			if (donneesNote.estUneNoteLongue)
			{
				NoteLongue scriptNoteLongue = nouveauDechet.GetComponent<NoteLongue>();
				if (scriptNoteLongue != null && scriptNoteLongue.laBandeVerte != null)
				{
					float vitesseJeu = GameManager.instance.leScroller.vitesseDefilement;
					float hauteurCalculee = donneesNote.dureeMaintien * vitesseJeu;
					scriptNoteLongue.laBandeVerte.sizeDelta = new Vector2(scriptNoteLongue.laBandeVerte.sizeDelta.x, hauteurCalculee);
				}
			}
		}
	}
}