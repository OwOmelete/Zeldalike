using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	private Image laCaseImage;
	public Color couleurNormale;
	public Color couleurAppuye;
	public KeyCode toucheAssignee;

	// On va lui donner le conteneur pour aller chercher les déchets
	public Transform conteneurNotes;

	// La distance max en pixels pour valider un coup (la tolérance du timing)
	public float distanceTolerance = 75f;

	void Start()
	{
		laCaseImage = GetComponent<Image>();
		laCaseImage.color = couleurNormale;
	}

	void Update()
	{
		// Effet visuel des touches
		if (Input.GetKeyDown(toucheAssignee))
		{
			laCaseImage.color = couleurAppuye;
			VerifierHit();
		}

		if (Input.GetKeyUp(toucheAssignee))
		{
			laCaseImage.color = couleurNormale;
		}
	}

	void VerifierHit()
	{
		// On regarde tous les déchets qui descendent dans le conteneur
		foreach (Transform dechet in conteneurNotes)
		{
			// On calcule l'écart vertical (Y) entre cette case blanche et le déchet
			float distanceY = Mathf.Abs(transform.position.y - dechet.position.y);

			// On vérifie aussi s'ils sont bien alignés horizontalement (X) sur la même piste
			float distanceX = Mathf.Abs(transform.position.x - dechet.position.x);

			if (distanceY <= distanceTolerance && distanceX < 50f)
			{
				Destroy(dechet.gameObject);
				Debug.Log("TOUCHÉ EN RYTHME !");
				GameManager.instance.NoteTouchee();
				break; // On arrête la boucle pour ne détruire qu'un déchet à la fois
			}
		}
	}
}