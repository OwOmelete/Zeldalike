using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // On ajoute TextMeshPro pour ton nouveau texte

public class EcranFin : MonoBehaviour
{
	[Header("Panneau Principal")]
	public GameObject panneauFinObject;

	[Header("Textes des Scores")]
	public TextMeshProUGUI texteScoreFinal; // Passé en TextMeshPro comme ton UI globale
	public TextMeshProUGUI texteMaxCombo;
	public TextMeshProUGUI textePerfects;
	public TextMeshProUGUI texteGoods;
	public TextMeshProUGUI texteBads;
	public TextMeshProUGUI texteMisses;

	[Header("Texte Humouristique")]
	public TextMeshProUGUI texteBlagueScore; // 💾 NOUVELLE CASE : Glisse ton texte d'ambiance ici !

	[Header("Système de Rang (Flames)")]
	public GameObject[] lesFlammesUI;

	void Start()
	{
		if (panneauFinObject != null) panneauFinObject.SetActive(false);
	}

	public void AfficherLesResultats(int score, int maxCombo, int nbPerfect, int nbGood, int nbBad, int nbMiss)
	{
		panneauFinObject.SetActive(true);

		if (texteScoreFinal != null) texteScoreFinal.text = score.ToString();
		if (texteMaxCombo != null) texteMaxCombo.text = "MAX COMBO : " + maxCombo.ToString();
		if (textePerfects != null) textePerfects.text = "PERFECT : " + nbPerfect.ToString();
		if (texteGoods != null) texteGoods.text = "GOOD : " + nbGood.ToString();
		if (texteBads != null) texteBads.text = "BAD : " + nbBad.ToString();
		if (texteMisses != null) texteMisses.text = "MISS : " + nbMiss.ToString();

		// 📝 ÉVALUATION DU TEXTE HUMOURISTIQUE SELON LE SCORE
		CalculerTexteHumour(score);

		StartCoroutine(AnimationFlammes(score));
	}

	void CalculerTexteHumour(int score)
	{
		if (texteBlagueScore == null) return;

		// 🎯 PALIERS DE TEXTE (À adapter selon tes envies !)
		if (score <= 2000)
		{
			texteBlagueScore.text = "<color=#FF0000>Un score éteint...</color>\nTes doigts ont laggé ou quoi ?";
		}
		else if (score > 2000 && score <= 6000)
		{
			texteBlagueScore.text = "<color=#FFA500>Petite étincelle.</color>\nC'est tiède, mais on va dire que c'est un début.";
		}
		else if (score > 6000 && score <= 10000)
		{
			texteBlagueScore.text = "<color=#FFFF00>Ça commence à chauffer !</color>\nLe rythme est là, le jury commence à hocher la tête.";
		}
		else // Score supérieur à 10000 (Comme ton run à 10900 !)
		{
			texteBlagueScore.text = "<color=#00FF00>Un score enflammé !</color>\nTu as brisé le clavier, Quincy Jones est fier de toi.";
		}
	}

	IEnumerator AnimationFlammes(int score)
	{
		foreach (GameObject flamme in lesFlammesUI)
		{
			flamme.SetActive(false);
		}

		yield return new WaitForSeconds(0.5f);

		if (score > 2000)
		{
			lesFlammesUI[0].SetActive(true);
		}

		yield return new WaitForSeconds(0.3f);

		if (score > 6000)
		{
			lesFlammesUI[1].SetActive(true);
		}

		yield return new WaitForSeconds(0.3f);

		if (score > 10000)
		{
			lesFlammesUI[2].SetActive(true);
		}
	}
}