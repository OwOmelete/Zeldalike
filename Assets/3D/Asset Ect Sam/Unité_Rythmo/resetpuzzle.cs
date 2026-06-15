using System.Collections.Generic;
using UnityEngine;

public class resetpuzzle : MonoBehaviour
{
    public List<GameObject> switch1 = new();
    public List<GameObject> switch2 = new();
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            foreach(GameObject go in switch1)
            {
                go.SetActive(true);
            }
            foreach(GameObject go in switch2)
            {
                go.SetActive(false);
            }
        }
    }
}
