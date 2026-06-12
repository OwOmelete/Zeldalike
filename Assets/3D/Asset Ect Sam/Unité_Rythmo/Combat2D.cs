using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat2D : MonoBehaviour
{
    public List<GameObject> enemies = new();
    public List<GameObject> familiersGo = new();
    public List<Famillier2D> familiers = new();
    public DeplacementUnite2D deplacementUnite2D;
    public Transform SpawnPoint;

    public float speedFamillier = 10f;

    private int currentFamilier;
    bool tryingRespawn;

    void Start()
    {
        foreach(GameObject f in familiersGo)
        {
           familiers.Add(f.GetComponent<Famillier2D>()) ;
        }
    }
    private void Update()
    {
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !enemies.Contains(collision.gameObject))
        {
            enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemies.Remove(collision.gameObject);
        }
    }

    private void Attack()
    {
        if (familiers.Count == 0 || enemies.Count == 0)
            return;

        enemies.RemoveAll(e => e == null);

        if (enemies.Count == 0)
            return;

        Famillier2D familier = familiers[currentFamilier];

        if (familier == null || familier.IsBusy)
        {
            NextFamilier();
            return;
        }

        familier.StartAttack(enemies[0], this, speedFamillier);

        NextFamilier();
    }

    private void NextFamilier()
    {
        currentFamilier++;

        if (currentFamilier >= familiers.Count)
            currentFamilier = 0;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle")&&!deplacementUnite2D.isdash&&!tryingRespawn)
        {
            StartCoroutine(RetourSpawn());
            //deplacementUnite2D.transform.position = SpawnPoint.position;
        }
    }
    IEnumerator RetourSpawn()
    {
        tryingRespawn=true;
        float i = 0.3f;
        bool willRespawn=true;
        while (i > 0)
        {
            i-=Time.deltaTime;
            if (Gamepad.current.rightTrigger.ReadValue() > 0.2f || deplacementUnite2D.isDashing)
            {
                tryingRespawn=false;
                willRespawn =false;
            } 
            yield return null;
        }
        if(willRespawn)deplacementUnite2D.transform.position = SpawnPoint.position;
        tryingRespawn=false;
    }
}