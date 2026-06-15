using System.Collections.Generic;
using UnityEngine;

public class EventTriggerScipt : MonoBehaviour
{
    public List<GameObject> gameObjects = new();
    public int count;

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        count=0;
        foreach(GameObject go in gameObjects)
        {
            if(!go.activeSelf)count++;
        }
        if(count==gameObjects.Count) gameObject.SetActive(false);
    }
}
