using UnityEngine;

public class PlayerDetection2D : MonoBehaviour
{
    public Enemy2D enemy2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy2D.AddPlayerRef(collision.GetComponent<Combat2D>());
        }
    }
}
