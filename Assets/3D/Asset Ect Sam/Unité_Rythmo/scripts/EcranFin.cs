using UnityEngine;
using TMPro;

public class EcranFin : MonoBehaviour
{
	[Header("Panneaux UI")]
	public GameObject panneauEcranFin;

	[Header("Affichages des Statistiques (TextMeshPro)")]
	public TextMeshProUGUI affichageScoreFinal;
	public TextMeshProUGUI affichageMaxCombo;
	public TextMeshProUGUI affichagePerfects;
	public TextMeshProUGUI affichageGoods;
	public TextMeshProUGUI affichageBads;
	public TextMeshProUGUI affichageMisses;
	public TextMeshProUGUI affichageDifficulte;

	[Header("Zone pour la Phrase Bonus")]
	public TextMeshProUGUI affichagePhraseBonus; // Glisse ton texte du milieu ici !

	[Header("Visuels des Récompenses (Flammes / Étoiles)")]
	public GameObject flamme1;
	public GameObject flamme2;
	public GameObject flamme3;

	void Start()
	{
		if (panneauEcranFin != null) panneauEcranFin.SetActive(false);
	}

	public void AfficherLesResultats(int scoreFinal, int maxCombo, int perfects, int goods, int bads, int misses)
	{
		if (panneauEcranFin != null) panneauEcranFin.SetActive(true);

		if (affichageScoreFinal != null) affichageScoreFinal.text = "SCORE FINAL\n: " + scoreFinal.ToString();
		if (affichageMaxCombo != null) affichageMaxCombo.text = "MAX COMBO : " + maxCombo.ToString();
		if (affichagePerfects != null) affichagePerfects.text = "PERFECTS : " + perfects.ToString();
		if (affichageGoods != null) affichageGoods.text = "GOODS : " + goods.ToString();
		if (affichageBads != null) affichageBads.text = "BADS : " + bads.ToString();
		if (affichageMisses != null) affichageMisses.text = "MISSES : " + misses.ToString();

		if (affichageDifficulte != null) affichageDifficulte.text = "MODE : " + SelectionDifficulte.ModeChoisi;

		// Calcul du palier de flammes (Strictement identique à la jauge !)
		int palierScoreRequis = SelectionDifficulte.ScoreRequisEtoile;
		int flammesObtenues = 0;

		if (scoreFinal >= palierScoreRequis)
		{
			flammesObtenues = 3;
		}
		else if (scoreFinal >= palierScoreRequis * 0.75f)
		{
			flammesObtenues = 2;
		}
		else if (scoreFinal >= palierScoreRequis * 0.40f)
		{
			flammesObtenues = 1;
		}
		else
		{
			flammesObtenues = 0;
		}

		// Remplacement avec tes phrases marrantes et personnalisées !
		if (affichagePhraseBonus != null)
		{
			if (flammesObtenues == 3)
			{
				affichagePhraseBonus.text = "<color=#FF4500>T'ES EN FEU !!! </color>";
			}
			else if (flammesObtenues == 2)
			{
				affichagePhraseBonus.text = "<color=#FFA500>TU CHAUFFES ! </color>";
			}
			else if (flammesObtenues == 1)
			{
				affichagePhraseBonus.text = "<color=#FFFF00>C'EST TIÈDE... </color>";
			}
			else
			{
				affichagePhraseBonus.text = "<color=#778899>T'ES ÉTEINT... </color>";
			}
		}

		ActualiserVisuelFlammes(flammesObtenues);
	}

	private void ActualiserVisuelFlammes(int nombreDeFlammes)
	{
		if (flamme1 != null) flamme1.SetActive(false);
		if (flamme2 != null) flamme2.SetActive(false);
		if (flamme3 != null) flamme3.SetActive(false);

		if (nombreDeFlammes >= 1 && flamme1 != null) flamme1.SetActive(true);
		if (nombreDeFlammes >= 2 && flamme2 != null) flamme2.SetActive(true);
		if (nombreDeFlammes >= 3 && flamme3 != null) flamme3.SetActive(true);
	}
}