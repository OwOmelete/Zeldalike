using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public List<Vector3> Vagues= new List<Vector3>();
    public List<GameObject> GeulGlacon= new List<GameObject>();
    int GeulGlaconCount;
    public List<GameObject> Fonceur= new List<GameObject>();
    int FonceurCount;
    public List<GameObject> Robouclier= new List<GameObject>();
    int RobouclierCount;
    public GameObject Boss;
    public int waveCount;
    public GameManagerTD gameManagerTD;
    // Update is called once per frame
    void Update()
    {
       
    }
    public IEnumerator invokeWave()
    {
       int x = (int) Math.Round(Vagues[waveCount].x);
       for(int i = 0 ; i < x ; i++)
        {
            GeulGlacon[GeulGlaconCount].SetActive(true);
            GeulGlacon[GeulGlaconCount].transform.position = transform.position;
            GeulGlaconCount++;
            if(GeulGlaconCount>=GeulGlacon.Count)GeulGlaconCount=0;
            yield return new WaitForSeconds(1f);
        }
       int y = (int) Math.Round(Vagues[waveCount].y);
       for(int i = 0 ; i < y ; i++)
        {
            Fonceur[FonceurCount].SetActive(true);
            Fonceur[FonceurCount].transform.position = transform.position;
            FonceurCount++;
            if(FonceurCount>=Fonceur.Count)FonceurCount=0;
            yield return new WaitForSeconds(1f);
        }
        int z = (int) Math.Round(Vagues[waveCount].z);
        for(int i = 0 ; i < z ; i++)
        {
            Robouclier[RobouclierCount].SetActive(true);
            Robouclier[RobouclierCount].transform.position = transform.position;
            RobouclierCount++;
            if(RobouclierCount>=Robouclier.Count)RobouclierCount=0;
            yield return new WaitForSeconds(1f);
        }
        waveCount++;
        if(waveCount>=Vagues.Count) waveCount=0;
    }
   
}
