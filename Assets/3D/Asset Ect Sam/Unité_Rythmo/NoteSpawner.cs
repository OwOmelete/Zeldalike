using UnityEngine;
using System.Globalization;

public class NoteSpawner : MonoBehaviour
{
	[Header("Les Prefabs de Déchets")]
	public GameObject dechetNormalPrefab;
	public GameObject dechetLongPrefab;

	[Header("Le Fichier de la Partition")]
	public TextAsset fichierPartition;

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
		// Forcer le scroller à (0,0) au tout début pour calibrer
		if (GameManager.instance != null && GameManager.instance.leScroller != null)
		{
			GameManager.instance.leScroller.transform.localPosition = Vector3.zero;
		}

		ChargerPartitionDepuisTexte();

		if (conteneurNotes == null && GameManager.instance != null && GameManager.instance.leScroller != null)
		{
			conteneurNotes = GameManager.instance.leScroller.GetComponent<RectTransform>();
		}
	}

	void ChargerPartitionDepuisTexte()
	{
		if (fichierPartition == null) return;

		string[] lignes = fichierPartition.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
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
	}

	void Update()
	{
		if (GameManager.instance == null || !GameManager.instance.jeuACommence || partition == null || partition.Length == 0) return;

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

		if (indexNoteActuelle < partition.Length)
		{
			float tempsApparitionAnticipe = partition[indexNoteActuelle].tempsFrappeVoulu - tempsDeTrajet;

			if (minuteurAudio >= tempsApparitionAnticipe)
			{
				SpawnDechet(partition[indexNoteActuelle]);
				indexNoteActuelle++;
			}
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

		if (rectDechet != null)
		{
			// FORCE LE PIVOT ET LES ANCRES À ÊTRE PILE SYNCHRO AVEC LA PISTE
			rectDechet.anchorMin = new Vector2(0.5f, 0f);
			rectDechet.anchorMax = new Vector2(0.5f, 0f);
			rectDechet.pivot = new Vector2(0.5f, 0.5f);

			// Positionnement horizontal direct en local pour éviter les décalages d'écrans
			float positionX = pisteCible.localPosition.x;

			// Calcul Y basé sur la hauteur de spawn fixe
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