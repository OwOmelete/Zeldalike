using System;
using UnityEngine;
using UnityEngine.UI;


public class NextLevel : MonoBehaviour
{
    public GameObject ActualLV;
    public GameObject NextLV;
    public GameObject FamillierStock;
    public Vector3 spawnNextMap;
    Combat2D player;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        ActualLV.SetActive(false);
        NextLV.SetActive(true);

        player = collision.GetComponent<Combat2D>();
        player.SpawnPoint.position = spawnNextMap;
        player.deplacementUnite2D = NextLV.GetComponent<DeplacementUnite2D>();
        player.SpawnPoint.transform.position = spawnNextMap;
        NextLV.transform.position = spawnNextMap;
        FamillierStock.transform.SetParent(NextLV.transform);
        }
       
    }
}
