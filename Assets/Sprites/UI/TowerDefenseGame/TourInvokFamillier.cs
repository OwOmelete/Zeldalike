using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class TourInvokFamillier : MonoBehaviour
{
[SerializeField] SplineContainer zoneSpawn;
public GameObject origine;
[SerializeField] List<GameObject> unite = new List<GameObject>();
int uniteCount;
    void Start()
    {
         //StartCoroutine(TourInvok());
    }
    void OnEnable()
    {
        StartCoroutine(TourInvok());
    }
    IEnumerator TourInvok()
    {
        while (true)
        {
            
            if (unite[uniteCount].activeSelf)
            {
                uniteCount++;
                
                yield return null;
            }
            else
            {
                float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
                unite[uniteCount].SetActive(true);
                zoneSpawn.Evaluate(randomValue, out float3 position, out float3 tangent, out float3 upVector);
                Vector3 SpawnPoint = zoneSpawn.transform.TransformPoint(position)/2;
                unite[uniteCount].transform.position = SpawnPoint;
                Vector3 dir = origine.transform.position - SpawnPoint;
                unite[uniteCount].transform.position -= dir*0.8f;
                uniteCount++;
                yield return new WaitForSeconds(10f); 
            }
            
        }
    }
}
