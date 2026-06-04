using UnityEngine;

public class NoteScroller : MonoBehaviour
{
	// La vitesse à laquelle le déchet va descendre
	public float vitesseDefilement;

	// Variable pour savoir si le jeu a commencé (la musique est lancée)
	public bool jeuDemarre = false;

	void Update()
	{
		// Si le jeu n'est pas lancé, on ne fait rien
		if (!jeuDemarre)
		{
			return;
		}

		// Fait descendre l'objet sur l'axe Y (vers le bas) à chaque frame
		transform.position -= new Vector3(0f, vitesseDefilement * Time.deltaTime, 0f);
	}
}