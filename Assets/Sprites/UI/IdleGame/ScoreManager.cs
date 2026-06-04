using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    public uiManager uiManagerp;
    public GameObject IdleGame;
    public GameObject PC;
    public float Score;
    public float Click ;
    public GameObject Button;
    public List<float> PriceClickList = new List<float>();
    public List<float> PriceDPSList = new List<float>();
    public List<int> Rank = new List<int>();
    public List<Slider> slider = new List<Slider>();
    public TextMeshProUGUI ScoreEffect;
    public TextMeshProUGUI CritScore;
    public Vector3 Decalage;
    public Vector3 Spawn;
    private Vector3 Scale;
    public GameObject positionEffetScore;
    private float DPS;
    public List<TextMeshProUGUI> priceClickTextList = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> priceDPSTextList = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> RankTexte = new List<TextMeshProUGUI>();
    public List<GameObject> ScoreAnim = new List<GameObject>();
    public List<GameObject> Famillier = new List<GameObject>();

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI clickText;
    public int NBRInstantiate;
    int NBRfamillier;
    int ScoreAnimI;
    void Start()
    {
        ScoreAnimI=0;
        Click = 1 ;
        UpdateTexte();
        for(int i = 0; i < priceClickTextList.Count; i++)
        {
            UpdatePriceTexte(i);
            slider[i].value=0;
        }
        StartCoroutine(AutoClick());
    }
    void Update()
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            AddScore();
        }
        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
           UpgradeClick(2);
        }
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            UpgradeClick(1);
        }
        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            UpgradeClick(0);
        }
        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            UpgradeDPS(0);
        }
          if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            StartCoroutine(unActive());
            
        }
        
        Score+=DPS*Time.unscaledDeltaTime;
        UpdateTexte();
    }
    public void AddScore()
    {
        
        StartCoroutine(AddScoreLoop());
    }
    IEnumerator unActive()
    {
        IdleGame.SetActive(false);
        PC.SetActive(true);
        yield return null; 
    }
    IEnumerator AutoClick()
    {
        
        
        yield return new WaitForSecondsRealtime(10/(Rank[1]+1));
        StartCoroutine(AutoClick());
        if (Rank[1] > 0)
        { 
        AddScore();
        }
        else
        {
        yield return null; 
        }
        
    }
     IEnumerator AddScoreLoop()
    {
        for(int i = 0; i < Rank[0]+1; i++)
        {
        float Crit = UnityEngine.Random.Range(0f, 100f);
            if (Crit > 100 - Rank[2] * 5)
            {
                Score += Click*2;
                if (NBRInstantiate < 20)
                {
                TextMeshProUGUI childObject = Instantiate(CritScore, Decalage, Quaternion.identity); 
                childObject.rectTransform.parent = positionEffetScore.transform;
                childObject.rectTransform.localScale= new Vector3(2,2,2);
                ScoreAnim.Add(childObject.gameObject);
        
                yield return new WaitForSecondsRealtime(0.05f);
                NBRInstantiate++;
                yield return null; 
                }
                else
                {
                    ScoreAnim[ScoreAnimI].SetActive(true);
                    ScoreAnimI++;
                    if(ScoreAnimI>=ScoreAnim.Count) ScoreAnimI=0;
                     yield return new WaitForSecondsRealtime(0.05f);
                }
        
            }
            //Test

            else
            {
                 Score += Click;
                 if (NBRInstantiate < 20)
                {
                    TextMeshProUGUI childObject = Instantiate(ScoreEffect, Decalage, Quaternion.identity); 
                    childObject.rectTransform.parent = positionEffetScore.transform;
                    yield return new WaitForSecondsRealtime(0.05f);
                    NBRInstantiate++;
                    ScoreAnim.Add(childObject.gameObject);
                    yield return null; 
                }
                else
                {
                    ScoreAnim[ScoreAnimI].SetActive(true);
                    ScoreAnimI++;
                    if(ScoreAnimI>=ScoreAnim.Count) ScoreAnimI=0;
                    yield return new WaitForSecondsRealtime(0.05f);
                }
        
            }
       
        }
    }
    public void UpgradeClick(int upgradeNo)
    {
        if (Score >= PriceClickList[upgradeNo])
        { 
        Click += 1+(10*Rank[upgradeNo]);
        Score-=PriceClickList[upgradeNo];
        PriceClickList[upgradeNo]+=Mathf.Round(PriceClickList[upgradeNo]*0.1f);
        slider[upgradeNo].value += 0.05f;
        
        UpdatePriceTexte(upgradeNo);
        }
        if(slider[upgradeNo].value >= 1)
        {
            Rank[upgradeNo]+=1;
            RankTexte[upgradeNo].text = Rank[upgradeNo].ToString(); 
            PriceClickList[upgradeNo]=Mathf.Round(PriceClickList[upgradeNo]/1.5f);
            slider[upgradeNo].value = 0;
        }
    }
     public void UpgradeDPS(int upgradeNo)
    {
        if (Score >= PriceDPSList[upgradeNo])
        {
            DPS+=50;
            if (NBRfamillier < Famillier.Count)
            {
              Famillier[NBRfamillier].SetActive(true);
            NBRfamillier++;  
            }
            
            Score-= PriceDPSList[upgradeNo];
             PriceDPSList[upgradeNo]+=Mathf.Round( PriceDPSList[upgradeNo]*2.5f);
            UpdatePriceTexte(upgradeNo);
        }
    }
    void UpdateTexte()
    {
        clickText.text = Click.ToString();
        float value = Mathf.Max(Score * 0.01f, 0.01f);
        float logScale = Mathf.Log(value+2)/1.5f;

        Vector3 scale = new Vector3(logScale, logScale, logScale);
        Button.transform.localScale = scale;    
        if (Score < 1000)
        {
             scoreText.text = Mathf.Round(Score).ToString();
        }
       else
        {
             scoreText.text = Mathf.Round(Score).ToString("0,00E0");
        }
       
        
    }
    void UpdatePriceTexte(int upgradeNo)
    {
        priceClickTextList[upgradeNo].text = PriceClickList[upgradeNo].ToString();
        priceDPSTextList[0].text =  PriceDPSList[0].ToString();
    }
}
