using UnityEngine;

public class NoteScroller : MonoBehaviour
{
	public float vitesseDefilement = 700f;
	public bool jeuDemarre = false;

	void Update()
	{
		if (!jeuDemarre) return;

		// Le tapis descend à vitesse constante sur l'axe Y
		transform.position -= new Vector3(0f, vitesseDefilement * Time.deltaTime, 0f);
	}
	public void changeBool()
	{
		jeuDemarre=true;
	}
}