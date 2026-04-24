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
        Destroy(gameObject,0.5f);
        scoreEffect.text = "+" + scoreManager.Click.ToString();
    }

}
