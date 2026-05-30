using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TourInvokFamillier : MonoBehaviour
{
[SerializeField] List<Transform> zoneSpawn = new List<Transform>();
    void OnEnable()
    {
        StartCoroutine(TourInvok());
    }
    IEnumerator TourInvok()
    {
        while (true)
        {
            int monNombreAleatoire = Random.Range(0, zoneSpawn.Count);
            yield return new WaitForSeconds(10f);
        }
    }
}
