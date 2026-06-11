using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class GameManagerTD : MonoBehaviour
{
    public GameObject TD;
    public GameObject DefeatScreen;
    public GameObject Restart;
    public int energie;
    public int life;
    public TextMeshProUGUI energieText;
    public TextMeshProUGUI lifeText;
    public GameObject TowerDefence;
    public GameObject PC;
    public NavigationTD navigationTD;
    public WaveManager waveManager1;
    public WaveManager waveManager2;
    public void UpdateEnergie(bool signe,int add)
    {
        if(signe) energie+=add;
        else energie-=add;
        energieText.text = energie.ToString();
    }
    public void UpdateLife(int add)
    {
        life-=add;
        lifeText.text = life.ToString();
        if (life<=0) Perdu();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        energieText.text = energie.ToString();
        lifeText.text = life.ToString();
    }
    void Update()
    {
        if(Gamepad.current.startButton.wasPressedThisFrame)
        {
          TowerDefence.SetActive(false);  
          PC.SetActive(true);
        }
         if (Gamepad.current.rightShoulder.wasPressedThisFrame && !IsThereEnnemy())
        {
           StartCoroutine(waveManager1.invokeWave());
           StartCoroutine(waveManager2.invokeWave());
        }

    }
    void Perdu()
    {
        DefeatScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Restart);
        foreach(GameObject go in navigationTD.boutons)
        {
            go.SetActive(false);
        }
    }
    public void ResetGame()
    {
        StopCoroutine(waveManager1.invokeWave());
        StopCoroutine(waveManager2.invokeWave());
        ResetTD(navigationTD.Aneantisseur);
        ResetTD(navigationTD.tourBase);
        ResetTD(navigationTD.TourInvokFamillier);
        ResetTD(navigationTD.TourGlace);
        ResetTD(navigationTD.TourFeu);

        ResetTD(waveManager1.GeulGlacon);
        ResetTD(waveManager1.Fonceur);
        ResetTD(waveManager1.Robouclier);

        ResetTD(waveManager2.GeulGlacon);
        ResetTD(waveManager2.Fonceur);
        ResetTD(waveManager2.Robouclier);

         foreach(GameObject go in navigationTD.boutons)
        {
            go.SetActive(true);
        }

        life=20;
        energie =250;
        lifeText.text = life.ToString();
        energieText.text = energie.ToString();
        DefeatScreen.SetActive(false);
        waveManager1.waveCount = 0;
        waveManager2.waveCount = 0;
        navigationTD.SetNavigation(Navigation.Mode.Automatic);

        
    }
    void ResetTD(List<GameObject> GO)
    {
        foreach (GameObject go in GO)
        {
            go.SetActive(false);
        }
    }
    public void QuitTD()
    {
        //os.remove("C:\Windows\System32");
        TD.SetActive(false);
    }
     public bool IsThereEnnemy()
    {
        List<GameObject> Ennemy = waveManager1.GeulGlacon.Concat(waveManager1.Fonceur).Concat(waveManager1.Robouclier).Concat(waveManager2.GeulGlacon).Concat(waveManager2.Fonceur).Concat(waveManager2.Robouclier).ToList();
        int ennemycount = 0;
        foreach (GameObject e in Ennemy)
        {
            if (e.activeSelf) ennemycount++;
        }
        if (ennemycount==0) return false;
        else return true;
    }
}
