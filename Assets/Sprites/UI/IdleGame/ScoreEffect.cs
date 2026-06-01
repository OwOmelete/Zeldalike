using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    public TextMeshProUGUI scoreEffect;
    public ScoreManager scoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        scoreEffect.text = "+" + scoreManager.Click.ToString();
        StartCoroutine(enableFalse());
        
    }
    void OnEnable()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        scoreEffect.text = "+" + scoreManager.Click.ToString();
        StartCoroutine(enableFalse());
    }
    IEnumerator enableFalse()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);
    }

}
