using System;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public Transform target;
    public Image sprite;
    public Sprite targetHot;
    public Sprite targetCold;
    public bool isHot;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        updateTarget();

    }

    void updateTarget()
    {
        if (isHot)
        {
            sprite.sprite = targetHot;
        }
        else
        {
            sprite.sprite = targetCold;
        }
        if (target) transform.position = target.transform.position;
        else sprite.enabled = false;
    }
}
